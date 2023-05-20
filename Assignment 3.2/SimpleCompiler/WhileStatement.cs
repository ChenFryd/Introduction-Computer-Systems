using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class WhileStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> Body { get; private set; }

        public override void Parse(TokensStack sTokens)
        {

            string sDeclarationType = "while";
            Token ifToekn = sTokens.Pop();
            if (!(ifToekn is Statement) || (((Statement)ifToekn).Name != sDeclarationType))
                throw new SyntaxErrorException("Expected " + sDeclarationType + ", received " + ifToekn, ifToekn);

            Token openIfToekn = sTokens.Pop();
            if (!(openIfToekn is Parentheses) || (((Parentheses)openIfToekn).Name != '('))
                throw new SyntaxErrorException("Expected ( received " + openIfToekn, openIfToekn);

            //Now, we create the correct Expression type based on the top token in the stack
            Term = Expression.Create(sTokens);
            //We transfer responsibility of the parsing to the created expression
            Term.Parse(sTokens);

            Token t = sTokens.Pop();//)
            if (!(t is Parentheses) || ((Parentheses)t).Name != ')')
                throw new SyntaxErrorException("Expected ) received: " + t, t);
            t = sTokens.Pop();//{
            if (!(t is Parentheses) || ((Parentheses)t).Name != '{')
                throw new SyntaxErrorException("Expected { received: " + t, t);

            Body = new List<StatetmentBase>();
            while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses))
            { //as long as we are inside the loop
                //We create the correct Statement type (if, while, return, let) based on the top token in the stack
                StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                //And call the Parse method of the statement to parse the different parts of the statement 
                s.Parse(sTokens);
                Body.Add(s);
            }
            

            t = sTokens.Pop();//}
            if (!(t is Parentheses) || ((Parentheses)t).Name != '}')
                throw new SyntaxErrorException("Expected } received: " + t, t);
        }
  
        public override string ToString()
        {
            string sWhile = "while(" + Term + "){\n";
            foreach (StatetmentBase s in Body)
                sWhile += "\t\t\t" + s + "\n";
            sWhile += "\t\t}";
            return sWhile;
        }

    }
}
