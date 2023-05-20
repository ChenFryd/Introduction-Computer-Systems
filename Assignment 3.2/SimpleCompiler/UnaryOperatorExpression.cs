using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class UnaryOperatorExpression : Expression
    {
        public string Operator { get; set; }
        public Expression Operand { get; set; }

        public override string ToString()
        {
            return Operator + Operand;
        }

        public override void Parse(TokensStack sTokens)
        {
            Token tokenOperator = sTokens.Pop();
            if (!(tokenOperator is Operator))
                throw new SyntaxErrorException("Expected operator received: " + tokenOperator, tokenOperator);

            //regex to check if the operator is not one of the ones we want
            if (Regex.IsMatch(Char.ToString(((Operator)tokenOperator).Name), "[^-!]"))
                throw new SyntaxErrorException("Invalid operator, received: " + tokenOperator, tokenOperator);
            Operator = Char.ToString(((Operator)tokenOperator).Name);

            //Now, we create the correct Expression type based on the top token in the stack
            Operand =  Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            Operand.Parse(sTokens);

        }
    }
}
