/*
 * Ce module gère le parsing (construction d'objets depuis une chaîne de caractère) et l'évaluation
 * d'une chaîne de caractères dont la syntaxe est la suivante :
 * 
   EXPRESSION :=  SYMBOL
   | (OPERATOR EXPRESSION EXPRESSION)
   | (not EXPRESSION)
   OPERATOR := or | and
   SYMBOL := une chaîne de caractère sans espace
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using XamlPreprocessor;

namespace XamlPreprocessor.Evaluator
{
    /*
     * Attention ce code date d'il y au moins quatre ans. Il a été réalisé alors que j'étais à l'université
     * sur la base d'un TP calculatrice donné par le créateur du langage LISAAC selon une technique "performante" et
     * récursive dont je ne me souviens plus des détails.
     * 
     * Ce parseur mériterai d'être réécrit proprement depuis le début.
     * 
     */
    static class Evaluator
    {
        /// <summary>
        /// Renvoie une expression (booléenne) évaluable à partir d'une chaîne de caractère.
        /// Lance une MalFormedExpressionException si la chaîne n'est pas conforme à la syntaxe attendue.
        /// </summary>
        /// <param name="ExprString">L'expression à parser.</param>
        /// <returns>Une représentation évaluable sous forme d'objet de l'expression.</returns>
        public static Expression Parse(string ExprString)
        {
            int Cursor = -1;
            ExprString = ExprString.ToLowerInvariant();
            Expression Expr0;
            Expression Expr1;

            while (++Cursor < ExprString.Length)
            {
                switch (ExprString[Cursor])
                {
                    case '(':
                        string plop = ReadNextString(ExprString.Substring(++Cursor));
                        switch (plop)
                        {
                            case "or":
                                Cursor += 3;
                                Expr0 = Parse(ExprString.Substring(Cursor));
                                Cursor += NextExpressionLength(ExprString.Substring(Cursor));
                                Expr1 = Parse(ExprString.Substring(Cursor));
                                Cursor += NextExpressionLength(ExprString.Substring(Cursor));
                                return new OR(Expr0, Expr1);
                            case "and":
                                Cursor += 4;
                                Expr0 = Parse(ExprString.Substring(Cursor));
                                Cursor += NextExpressionLength(ExprString.Substring(Cursor));
                                Expr1 = Parse(ExprString.Substring(Cursor));
                                Cursor += NextExpressionLength(ExprString.Substring(Cursor));
                                return new AND(Expr0, Expr1);
                            case "not":
                                Cursor += 4;
                                return new NOT(Parse(ExprString.Substring(Cursor)));
                        }
                        break;
                    case ')':
                        Cursor++;
                        break;
                    case ' ':
                        break;
                    default:
                        return ReadNextValue(ExprString.Substring(Cursor));
                }
            }
            throw new MalFormedExpressionException(/*"Error in parsing : expression has wrong format."*/);
        }

        private static Expression ReadNextValue(string StrArg)
        {
            int Cursor = 0;
            string Value = "";
            while (Cursor < StrArg.Length && StrArg[Cursor] != ' ' && StrArg[Cursor] != ')')
            {
                Value += StrArg[Cursor];
                Cursor++;
            }
            return new ValueExpression(Value);
        }

        private static string ReadNextString(string StrArg)
        {
            int Cursor = 0;
            string Value = "";
            while (Cursor < StrArg.Length && StrArg[Cursor] != ' ' && StrArg[Cursor] != ')')
            {
                Value += StrArg[Cursor];
                Cursor++;
            }
            return Value;
        }

        private static int NextExpressionLength(string StrArg)
        {
            int Cursor = 0;  // permet de zapper les espaces
            int ParenthesisCount = 0;
            int Value = 0;
            if (StrArg[0] == '(')
            {
                do
                {
                    Value++;
                    if (StrArg[Cursor] == '(')
                    {
                        ParenthesisCount++;
                    }
                    if (StrArg[Cursor] == ')')
                    {
                        ParenthesisCount--;
                    }
                    Cursor++;
                } while (Cursor < StrArg.Length && ParenthesisCount > 0);
            }
            else
            {
                while (Cursor < StrArg.Length && StrArg[Cursor] != ' ')
                {
                    Value++;
                    Cursor++;
                }
            }
            return Value;
        }

        /// <summary>
        /// Contient des tests de certaines des fonctions de la classe.
        /// </summary>
        public static void Main()
        {
            /* Exemple d'expression bien formée :
               (OR csharp nemerle)
               (AND windows (OR c cpp))
            */
            // Test d'évaluation d'expression. Devrait imprimer True, True et False sur la console.
            Expression exprtest = new OR(new ValueExpression("csharp"), new ValueExpression("nemerle"));
            Console.WriteLine(exprtest.Evaluate("csharp"));
            Console.WriteLine(exprtest.Evaluate("nemerle"));
            Console.WriteLine(exprtest.Evaluate("scheme"));
            Console.WriteLine();
            // Test d'évaluation d'une expression. Devrait imprimer False puis True sur la console.
            exprtest = new AND(new ValueExpression("windows"), new OR(new ValueExpression("c"), new ValueExpression("cpp")));
            string[] tab = new string[3];
            tab[0] = "windows";
            Console.WriteLine(exprtest.Evaluate(tab));
            tab[1] = "c";
            Console.WriteLine(exprtest.Evaluate(tab));
            Console.WriteLine();
            // Test d'évaluation d'une expression. Devrait imprimer False sur la console.
            exprtest = new AND(new ValueExpression("windows"), new AND(new ValueExpression("c"), new NOT(new ValueExpression("cpp"))));
            tab[0] = "windows";
            tab[1] = "c";
            tab[1] = "cpp";
            Console.WriteLine(exprtest.Evaluate(tab));
            Console.WriteLine();
            // Test la lecture et le parsing d'expressions simples. Ces trois test impriment 'test' sur la console.
            Console.WriteLine(ReadNextValue("test").ToString());
            Console.WriteLine(ReadNextValue("test lol").ToString());
            Console.WriteLine(Parse("test lol").ToString());
            // Test de parsing d'expressions complexes. Ces expressions doivent être affichées sur la console.
            Console.WriteLine(Parse("(or test pwet)").ToString());
            Console.WriteLine(Parse("(and (or test (or pwet lol)) windows)").ToString());
            Console.WriteLine(Parse("(not (or (and scheme commonlisp) (not cpp)))").ToString());
        }
    }
}
