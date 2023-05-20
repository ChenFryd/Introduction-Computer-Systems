using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class LetStatement : StatetmentBase
    {
        public string Variable { get; set; }
        public Expression Value { get; set; }

        public override string ToString()
        {
            return "let " + Variable + " = " + Value + ";";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token t = sTokens.Pop();//}
            if (!(t is Statement) || ((Statement)t).Name != "let")
                throw new SyntaxErrorException("Expected Let received: " + t, t);
            Token varToken = sTokens.Pop();
            if (!(varToken is Identifier))
                throw new SyntaxErrorException("Expected Identifier received: " + t, t);
            Variable = ((Identifier)varToken).Name;

            t = sTokens.Pop(); //=
            if (!(t is Operator))
                throw new SyntaxErrorException("Expected Identifier received: " + t, t);
            if (!(((Operator)t).Name == '='))
                throw new SyntaxErrorException("Expected Operator = received: " + t, t);

            Value = Expression.Create(sTokens);
            Value.Parse(sTokens);

            t = sTokens.Pop(); //;
            if (!(t is Separator))
                throw new SyntaxErrorException("Expected ; received " + t, t);
            if (((Separator)t).Name != ';')
                throw new SyntaxErrorException("Expected ; received " + t, t);

        }

    }
}
