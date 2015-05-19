using System;
using System.Linq;

namespace XamlPreprocessor
{
    public static class DirectiveAttrAdd
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
}
