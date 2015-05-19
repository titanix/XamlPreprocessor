using System;
using System.Linq;
using XamlPreprocessor.Evaluator;

namespace XamlPreprocessor
{
    /// <summary>
    /// Classe statique qui fournit des méthodes permettant de tester si des chaînes de caractères
    /// encodent l'un ou l'autre des directives supportées par le préprocesseur.
    /// </summary>
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
                return XamlPreprocessor.Evaluator.Evaluator.Parse(temp);
            }
            catch (MalFormedExpressionException)
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
                return XamlPreprocessor.Evaluator.Evaluator.Parse(temp);
            }
            catch (MalFormedExpressionException)
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
}
