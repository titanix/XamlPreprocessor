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
            Preprocessor prepro = new Preprocessor();
            prepro.Symbols = this.Symbols.Split(';');
            return prepro.Run(File, OutName);
        }
    }

    public class PreprocessorTask_2 : Task
    {
        [Required]
        public string[] Symbols { get; set; }

        [Required]
        public string[] Files { get; set; }

        [Required]
        public string OutDir { get; set; }

        public override bool Execute()
        {
            Preprocessor prepro = new Preprocessor();
            bool res = true;
            foreach (string item in Files)
            {
                res &= prepro.Run(item, OutDir);
            }
            return res;
        }
    }
}
