using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get;  set; }
        public Expression Operand1 { get;  set; }
        public Expression Operand2 { get;  set; }

        public override string ToString()
        {
            return "(" + Operand1 + " " + Operator + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token t = sTokens.Pop();//(
            if (!(t is Parentheses) || ((Parentheses)t).Name != '(')
                throw new SyntaxErrorException("Expected ( received: " + t, t);
            //Now, we create the correct Expression type based on the top token in the stack
            Operand1 = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            Operand1.Parse(sTokens);

            Token tokenOperator = sTokens.Pop();
            if (!(tokenOperator is Operator))
                throw new SyntaxErrorException("Expected operator received: " + tokenOperator, tokenOperator);

            //regex to check if the operator is not one of the ones we want
            string pattern = @"[*+/%&|=<>!-]";
            string tokenOperatorName = Char.ToString(((Operator)tokenOperator).Name);
            if (!Regex.IsMatch(tokenOperatorName, pattern ))
                throw new SyntaxErrorException("Invalid operator, received: " + tokenOperator, tokenOperator);
            Operator = tokenOperatorName;


            //Now, we create the correct Expression type based on the top token in the stack
            Operand2 = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            Operand2.Parse(sTokens);

            t = sTokens.Pop();//)
            if (!(t is Parentheses) || ((Parentheses)t).Name != ')')
                throw new SyntaxErrorException("Expected ) received: " + t, t);

        }
    }
}
