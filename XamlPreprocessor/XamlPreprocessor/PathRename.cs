using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;

namespace XamlPreprocessor
{
    /// <summary>
    /// MS Build Task.
    /// https://msdn.microsoft.com/en-us/library/microsoft.build.utilities.task(v=vs.121).aspx
    /// https://msdn.microsoft.com/en-us/magazine/cc163589.aspx
    /// </summary>
    public class PathRename : Task
    {
        [Required]
        public string FilePath { get; set; }

        [Required]
        public string PathToRemove { get; set; }

        [Required]
        public string ProjectPath { get; set; }

        /*
         * Le fonctionnement de cette méthode est complètement hack : il s'agit de réécrire un chemin présent
         * dans les fichiers .g.i.cs générés par la phase MarkupCompile1 afin d'en supprimer le chemin du sous-dossier
         * par rapport à la racine de la solution qui y est ajouté au début (à cause de la façon dont j'utilisais la task).
         * 
         * En gros, ça remplace temp\file.xaml par file.xaml dans des fichiers g.i.cs.
         * 
         * C'est typiquement quelque chose à retravailler pour une intégration propre à d'autres types de projets .net.
         * 
         */
        public override bool Execute()
        {
            FilePath = ProjectPath + @"\" + FilePath;
            FilePath = FilePath.Replace(".xaml", ".g.i.cs");

            if (File.Exists(FilePath))
            {
                string content = "";
                using (StreamReader file = new StreamReader(FilePath))
                {
                    content = file.ReadToEnd();
                }
                File.Delete(FilePath);

                using (StreamWriter newFile = new StreamWriter(FilePath))
                {
                    string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string li in lines)
                    {
                        if (li.Trim().StartsWith("System.Windows.Application.LoadComponent(this, new System.Uri"))
                        {
                            string uri = li.Split('"')[1];
                            uri = uri.Replace(";component/" + PathToRemove.Replace('\\', '/'), ";component/");
                            newFile.WriteLine(String.Format("System.Windows.Application.LoadComponent(this, new System.Uri(\"{0}\", System.UriKind.Relative));", uri));
                        }
                        else
                        {
                            newFile.WriteLine(li);
                        }
                    }
                    return true;
                }
            }
            return true;
        }

    }
}
