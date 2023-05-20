using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Compiler
    {

        private Dictionary<string, int> m_dSymbolTable;
        private int m_cLocals;

        public static string[] Statements = { "function", "var", "let", "while", "if", "else", "return", "global" };
        public static string[] VarTypes = { "int", "char", "boolean", "array" };
        public static string[] Constants = { "true", "false", "null" };
        public static char[] Operators = new char[] { '*', '+', '-', '/', '<', '>', '&', '=', '|', '!' };
        public static char[] Parentheses = new char[] { '(', ')', '[', ']', '{', '}' };
        public static char[] Separators = new char[] { ',', ';', '\'' };
        public Compiler()
        {
            m_dSymbolTable = new Dictionary<string, int>();
            m_cLocals = 0;

        }

        public List<string> Compile(string sInputFile)
        {
            List<string> lCodeLines = ReadFile(sInputFile);
            List<Token> lTokens = Tokenize(lCodeLines);
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            JackProgram program = Parse(sTokens);
            return null;
        }

        private JackProgram Parse(TokensStack sTokens)
        {
            JackProgram program = new JackProgram();
            program.Parse(sTokens);
            return program;
        }

        public List<string> Compile(List<string> lLines)
        {

            List<string> lCompiledCode = new List<string>();
            foreach (string sExpression in lLines)
            {
                List<string> lAssembly = Compile(sExpression);
                lCompiledCode.Add("// " + sExpression);
                lCompiledCode.AddRange(lAssembly);
            }
            return lCompiledCode;
        }



        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }

        //Computes the next token in the string s, from the begining of s until a delimiter has been reached. 
        //Returns the string without the token.
        private string Next(string s, char[] aDelimiters, out string sToken, out int cChars)
        {
            cChars = 1;
            sToken = s[0] + "";
            if (aDelimiters.Contains(s[0]))
                return s.Substring(1);
            int i = 0;
            for (i = 1; i < s.Length; i++)
            {
                if (aDelimiters.Contains(s[i]))
                    return s.Substring(i);
                else
                    sToken += s[i];
                cChars++;
            }
            return null;
        }

        //Splits a string into a list of tokens, separated by delimiters
        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

        public List<Token> Tokenize(List<string> lCodeLines)
        {
            List<Token> lTokens = new List<Token>();
            //your code here
            for (int lineIndex = 0; lineIndex < lCodeLines.Count; lineIndex++) //iterate over lines of text
            {
                int wordPointer = 0;
                string codeLine = lCodeLines[lineIndex];
                if (codeLine.Contains("\t"))
                {
                    codeLine = codeLine.Substring(codeLine.LastIndexOf("\t") + 1);
                }
                if (codeLine.StartsWith("//"))
                    continue;
                char[] delimiterInLine = { ' ', '*', '+', '-', '/', '<', '>', '&', '=', '|', '!', '(', ')', '[', ']', '{', '}', ',', ';', '\'' };
                // List<string> listOfStringTokens = Split(codeLine, delimiterInLine);
                string currWord;
                String remainingLine = codeLine;
                int lastIndex = 0;
                while (remainingLine != "")
                {
                    lastIndex = lastIndex + wordPointer;
                    remainingLine = Next(remainingLine, delimiterInLine, out currWord, out wordPointer);
                    if (currWord == " ")
                        continue;

                    if (Regex.IsMatch(currWord, "[^a-zA-Z0-9(){}[]*/+-%&|=<>!;,']+")) //containing anywhere an illegal letter
                    { //containing an illegal letter
                        Token token_error = new Token();
                        token_error.Line = lineIndex;
                        token_error.Position = lastIndex;
                        throw new SyntaxErrorException("Incorrect character", token_error);
                    }
                    if (Statements.Contains(currWord))
                    {
                        Statement token_statment = new Statement(currWord, lineIndex, lastIndex);
                        lTokens.Add(token_statment);
                        continue;
                    }
                    else if (VarTypes.Contains(currWord))
                    {
                        VarType token_varType = new VarType(currWord, lineIndex, lastIndex);
                        lTokens.Add(token_varType);
                        continue;
                    }
                    else if (Constants.Contains(currWord))
                    {
                        Constant token_constant = new Constant(currWord, lineIndex, lastIndex);
                        lTokens.Add(token_constant);
                        continue;
                    }
                    if (currWord.Length == 1)
                    {
                        char char_curr_word = char.Parse(currWord);
                        if (Operators.Contains(char_curr_word))
                        {
                            Operator token_Operator = new Operator(char_curr_word, lineIndex, lastIndex);
                            lTokens.Add(token_Operator);
                            continue;
                        }
                        else if (Parentheses.Contains(char_curr_word))
                        {
                            Parentheses token_Parentheses = new Parentheses(char_curr_word, lineIndex, lastIndex);
                            lTokens.Add(token_Parentheses);
                            continue;
                        }
                        else if (Separators.Contains(char_curr_word))
                        {
                            Separator token_Separator = new Separator(char_curr_word, lineIndex, lastIndex);
                            lTokens.Add(token_Separator);
                            continue;
                        }
                    }
                    if (!Regex.IsMatch(currWord, "[a-zA-Z]+")) //is a number
                    {
                        Number token_constent = new Number(currWord, lineIndex, lastIndex);
                        lTokens.Add(token_constent);
                        continue;
                    }
                    if (Regex.IsMatch(currWord.Substring(0, 1), "[0-9]+") && Regex.IsMatch(currWord.Substring(1), "[a-zA-Z]+")) //the whole word containing a number at the first letter and a letter afterwards
                    {
                        Token token_error = new Token();
                        token_error.Line = lineIndex;
                        token_error.Position = lastIndex;
                        throw new SyntaxErrorException("Incorrect character", token_error);
                    }
                    else
                    { //we will do identifier
                        Identifier token_identifier = new Identifier(currWord, lineIndex, lastIndex);
                        lTokens.Add(token_identifier);
                        continue;
                    }
                }

            }
            return lTokens;
        }

    }
}
