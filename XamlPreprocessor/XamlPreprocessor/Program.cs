using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Saphir;
using System.IO;
using System.Xml;

namespace XamlPreprocessor
{
    /// <summary>
    /// C'est la classe qui gère la logique du préprocesseur. Elle expose une méthode Main qui permet de l'utiliser
    /// en ligne de commande.
    /// </summary>
    partial class Preprocessor
    {
        /// <summary>
        /// Listes des symboles de compilation que le préprocesseur doit prendre en compte.
        /// </summary>
        public string[] Symbols;

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.Error.WriteLine("Usage : prepro.exe SYMBOL_LIST input_file.xml output_file.xml");
                Console.Error.WriteLine("SYMBOL_LIST is a list of comma separated symbols.");
                Environment.Exit(10022);
            }
            Preprocessor prepro = new Preprocessor();
            prepro.Symbols = args[0].Split(';');

            try
            {
                XDocument xdoc = XDocument.Load(args[1]);
                prepro.ParseNamespaceDeclarations(xdoc);
                xdoc = prepro.Preprocess(xdoc);
                prepro.WriteFile(xdoc, args[2]);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Shit happened with file {0} because {1}", args[1], ex.Message);
                Environment.Exit(1);
            }
        }

        public bool Run(string path, string outpath = "")
        {
            if (String.IsNullOrEmpty(outpath))
            {
                outpath = path;
            }
            try
            {
                XDocument xdoc = XDocument.Load(path);
                ParseNamespaceDeclarations(xdoc);
                xdoc = Preprocess(xdoc);
                WriteFile(xdoc, outpath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Dictionary<string, XNamespace> Namespaces = new Dictionary<string, XNamespace>();

        public void ParseNamespaceDeclarations(XDocument xamlFile)
        {
            foreach (XAttribute rootAttr in xamlFile.Root.Attributes())
            {
                if (rootAttr.IsNamespaceDeclaration)
                {
                    if (!Namespaces.ContainsKey(rootAttr.Name.LocalName))
                    {
                        XNamespace tmp = rootAttr.Value;
                        Namespaces.Add(rootAttr.Name.LocalName, tmp);
                    }
                }

            }
        }

        /// <summary>
        /// Lit chaque noeuds XML d'un fichier xml. A chaque commentaire rencontré, 
        /// si le commentaire est une évaluation évaluée à vrai, le noeud suivant est conservé dans l'arbre,
        /// sinon il est retiré.
        /// </summary>
        /// <param name="xamlFile"></param>
        /// <returns></returns>
        private XDocument Preprocess(XDocument xamlFile)
        {
            string processMessage = "expression '{0}' evaluated at '{1}'";
            foreach (XNode elem in xamlFile.Root.DescendantNodes())
            {
                CommentType cType = GetNodeType(elem);
                Expression exp = new NullExpression();
                switch (cType)
                {
                    case CommentType.IGNORE:
                        break;
                    case CommentType.IF:
                        exp = Directives.ExtractExpressionIF((elem as XComment).Value);
                        bool eval = exp.Evaluate(Symbols);
                        if (!eval)
                        {
                            // Note : il semblerait qu'AfterSelf() ignore de toute façon les noeuds commentaires  
                            elem.ElementsAfterSelf().Where(e => e.NodeType != XmlNodeType.Comment).First().Remove();
                        }
                        (elem as XComment).Value = String.Format(processMessage, (elem as XComment).Value.Trim(), eval);
                        break;
                    case CommentType.LIF:
                        exp = Directives.ExtractExpressionLIF((elem as XComment).Value);
                        bool eval2 = exp.Evaluate(Symbols);
                        if (!eval2)
                        {
                            elem.NextNode.Remove();
                            elem.AddAfterSelf(new XComment(String.Format("deleted node was here")));
                        }
                        (elem as XComment).Value = String.Format(processMessage, (elem as XComment).Value.Trim(), eval2);
                        break;
                    case CommentType.ATTR_ADD:
                        string commValue = (elem as XComment).Value;
                        commValue = Directives.ExtractDirectiveAttrAdd(commValue);
                        string ns = Directives.ExtractNamespace(commValue);
                        string attrName = DirectiveATTR_ADD.ExtractAttributeName(commValue);
                        string attrValue = DirectiveATTR_ADD.ExtractAttributeValue(commValue);

                        XNode xn = elem.NextNode;
                        while (xn.NodeType != XmlNodeType.Element)
                        {
                            xn = xn.NextNode;
                        }
                        XElement xel = xn as XElement;

                        if (xel != null)
                        {
                            XNamespace xns;
                            if (ns.Equals(String.Empty))
                            {
                                xel.SetAttributeValue(attrName, attrValue);
                            }
                            else
                            {
                                Namespaces.TryGetValue(ns, out xns);
                                xel.SetAttributeValue(xns + attrName, attrValue);
                            }
                            (elem as XComment).Value = String.Format("attribute '{0}' added with value '{1}'", attrName, attrValue);
                        }

                        break;
                    case CommentType.ATTR_DEL:
                        string comment = (elem as XComment).Value;
                        string directive = Directives.ExtractDirectiveAttrDel(comment);
                        string name = DirectiveATTR_DEL.ExtractAttributeValue(directive);
                        ns = Directives.ExtractNamespace(directive);

                        xn = elem.NextNode;
                        while (xn.NodeType != XmlNodeType.Element)
                        {
                            xn = xn.NextNode;
                        }
                        xel = xn as XElement;

                        if (xel != null)
                        {
                            XNamespace xns;
                            if (ns.Equals(String.Empty))
                            {
                                xel.SetAttributeValue(name, null);
                            }
                            else
                            {
                                Namespaces.TryGetValue(ns, out xns);
                                xel.SetAttributeValue(xns + name, null);
                            }
                            (elem as XComment).Value = String.Format("attribute '{0}' removed", name);
                        }

                        break;
                    default:
                        break;
                }
            }
            return xamlFile;
        }

        /// <summary>
        /// Ecrit un fichier xaml à l'endroit ciblé en accolant un suffixe au nom de fichier.
        /// </summary>
        /// <param name="xdoc"></param>
        /// <param name="path"></param>
        private void WriteFile(XDocument xdoc, string path)
        {
            try
            {
                using (StreamWriter output = new StreamWriter(path))
                {
                    using (XmlWriter xout = XmlWriter.Create(output))
                    {
                        xdoc.WriteTo(xout);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Shit happened : {0}", ex.Message);
            }
        }

    }
}
