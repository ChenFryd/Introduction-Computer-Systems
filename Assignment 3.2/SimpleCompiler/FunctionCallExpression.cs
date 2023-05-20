using System;
using System.Collections.Generic;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Token idToken = sTokens.Pop();
            if (!(idToken is Identifier))
                throw new SyntaxErrorException("Expected Identifier, received " + idToken, idToken);
            FunctionName = ((Identifier)idToken).Name;

            Token openIfToekn = sTokens.Pop();
            if (!(openIfToekn is Parentheses) || (((Parentheses)openIfToekn).Name != '('))
                throw new SyntaxErrorException("Expected ( received " + openIfToekn, openIfToekn);

            Token tEnd = null;
            Args = new List<Expression>();
            do
            {
                Expression tExpt = Expression.Create(sTokens);
                //We transfer responsibility of the parsing to the created expression
                tExpt.Parse(sTokens);
                Args.Add(tExpt);
                tEnd = sTokens.Pop();
            }
            while (tEnd.ToString() == ",");

            if (!(tEnd is Parentheses))
                throw new SyntaxErrorException("Expected ) received: " + tEnd, tEnd);
            if (!(((Parentheses)tEnd).Name == ')'))
                throw new SyntaxErrorException("Expected ) received: " + tEnd, tEnd);
        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}