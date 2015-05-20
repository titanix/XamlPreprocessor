using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace XamlPreprocessor
{
    public class PreprocessorTask : Task
    {
        [Required]
        public string File { get; set; }

        [Required]
        public string Symbols { get; set; }

        public string OutName { get; set; }

        public override bool Execute()
        {
            Preprocessor prepro = new Preprocessor(runAsTask: true);
            prepro.Symbols = this.Symbols.Split(';');
            try {
                if (!prepro.Run(File, OutName))
                {
                    base.Log.LogError(String.Format("Error while processing file {0}", File));
                    return false;
                }
                return true;
            } catch(Exception e) {
                base.Log.LogError(String.Format("Error while processing file {0}; reason: {1}", File, e.Message));
                return false;
            }
        }
    }
}
