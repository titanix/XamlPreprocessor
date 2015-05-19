using System;
using System.Linq;

namespace XamlPreprocessor
{
    public static class DirectiveAttrDel
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
