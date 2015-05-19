using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Saphir;

namespace XamlPreprocessor
{
    public enum CommentType
    {
        IGNORE,
        IF,
        LIF, // ~IF
        ATTR_ADD,
        ATTR_DEL
    }

    partial class Preprocessor
    {
        public CommentType GetNodeType(XNode node)
        {
            if (node.NodeType == System.Xml.XmlNodeType.Comment)
            {
                XComment comm = node as XComment;
                string str = comm.Value;
                if (Directives.IsDirectiveIF(str))
                    return CommentType.IF;
                if (Directives.IsDirectiveLIF(str))
                    return CommentType.LIF;
                if (Directives.IsDirectiveATTR_ADD(str))
                    return CommentType.ATTR_ADD;
                if (Directives.IsDirectiveATTR_DEL(str))
                    return CommentType.ATTR_DEL;
                return CommentType.IGNORE;
            }
            else
            {
                return CommentType.IGNORE;
            }
        }
    }

    public static class Directives
    {
        const string IF_PREFIX = "IF[";
        const string LIF_PREFIX = "~IF[";
        const string ATTR_ADD_PREFIX = "ATTR-ADD[";
        const string ATTR_DEL_PREFIX = "ATTR-DEL[";
        const string COMMON_SUFFIX = "]";

        public static Expression ExtractExpressionIF(string str)
        {
            str = str.ToString().Trim();
            string temp = str.Substring(IF_PREFIX.Length, str.Length - (IF_PREFIX.Length + 1));
            try
            {
                return Evaluator.Parse(temp);
            }
            catch (MalFormedExpressionException e)
            {
                return new NullExpression();
            }
        }

        public static Expression ExtractExpressionLIF(string str)
        {
            str = str.ToString().Trim();
            string temp = str.Substring(LIF_PREFIX.Length, str.Length - (LIF_PREFIX.Length + 1));
            try
            {
                return Evaluator.Parse(temp);
            }
            catch (MalFormedExpressionException e)
            {
                return new NullExpression();
            }
        }

        /// <summary>
        /// À n'appeler que lorsqu'on est sûr qu'il s'agit bien qu'une directive ATTR-ADD car il n'y a pas de contrôle sur la taille de la chaîne.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ExtractDirectiveAttrAdd(string str)
        {
            str = str.ToString().Trim();
            return str.Substring(ATTR_ADD_PREFIX.Length, str.Length - (ATTR_ADD_PREFIX.Length + 1));
        }

        public static string ExtractDirectiveAttrDel(string str)
        {
            str = str.ToString().Trim();
            return str.Substring(ATTR_DEL_PREFIX.Length, str.Length - (ATTR_DEL_PREFIX.Length + 1));
        }

        public static bool IsDirectiveIF(string str)
        {
            str = str.Trim();
            if (str.Length < IF_PREFIX.Length)
                return false;
            if (str.Substring(0, IF_PREFIX.Length).Equals(IF_PREFIX) && str[str.Length - 1] == ']')
                return true;
            return false;
        }

        public static bool IsDirectiveLIF(string str)
        {
            str = str.Trim();
            if (str.Length < LIF_PREFIX.Length)
                return false;
            if (str.Substring(0, LIF_PREFIX.Length).Equals(LIF_PREFIX) && str[str.Length - 1] == ']')
                return true;
            return false;
        }

        public static bool IsDirectiveATTR_ADD(string str)
        {
            str = str.Trim();
            if (str.Length < ATTR_ADD_PREFIX.Length)
                return false;
            if (str.Substring(0, ATTR_ADD_PREFIX.Length).Equals(ATTR_ADD_PREFIX) && str[str.Length - 1] == ']')
                return true;
            return false;
        }

        public static bool IsDirectiveATTR_DEL(string str)
        {
            str = str.Trim();
            if (str.Length < ATTR_DEL_PREFIX.Length)
                return false;
            if (str.Substring(0, ATTR_DEL_PREFIX.Length).Equals(ATTR_DEL_PREFIX) && str[str.Length - 1] == ']')
                return true;
            return false;
        }

        public static string ExtractNamespace(string decl)
        {
            if (decl.Contains(':'))
            {
                return decl.Split(':')[0];
            }
            else
            {
                return String.Empty;
            }
        }

    }

    public static class DirectiveATTR_ADD
    {
        public static string ExtractAttributeName(string decl)
        {
            try
            {
                if (decl.Contains(':'))
                {
                    return decl.Split(':')[1].Split('=')[0];
                }
                else
                {
                    return decl.Split('=')[0];
                }
            }
            catch (IndexOutOfRangeException)
            {
                return String.Empty;
            }
        }

        public static string ExtractAttributeValue(string decl)
        {
            try
            {
                if (decl.Contains(':'))
                {
                    string[] array = decl.Split(':');
                    string temp = String.Join(":", array, 1, array.Length - 1);
                    array = temp.Split('=');
                    temp = String.Join("=", array, 1, array.Length - 1);
                    return temp.Substring(1, temp.Length - 2); // on enlève les guillemets qui entourent la valeur de l'attribut
                }
                else
                {
                    string[] array = decl.Split('=');
                    string res = String.Join("=", array, 1, array.Length - 1);
                    return res.Substring(1, res.Length - 2); // on enlève les guillemets qui entourent la valeur de l'attribut
                }
            }
            catch (IndexOutOfRangeException)
            {
                return String.Empty;
            }
        }
    }

    public static class DirectiveATTR_DEL
    {
        public static string ExtractAttributeValue(string decl)
        {
            try
            {
                if (decl.Contains(':'))
                {
                    string[] array = decl.Split(':');
                    return String.Join(":", array, 1, array.Length - 1);   
                }
                else
                {
                    return decl;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return String.Empty;
            }
        }
    }
}
