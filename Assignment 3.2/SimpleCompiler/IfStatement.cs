using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            string sDeclarationType = "if";
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
            if (!(t is Parentheses))
                throw new SyntaxErrorException("Expected ) received: " + t, t);
            if (((Parentheses)t).Name != ')')
                throw new SyntaxErrorException("Expected ) received: " + t, t);

            t = sTokens.Pop();//{
            if (!(t is Parentheses))
                throw new SyntaxErrorException("Expected { received: " + t, t);
            if (((Parentheses)t).Name != '{')
                throw new SyntaxErrorException("Expected { received: " + t, t);

            DoIfTrue = new List<StatetmentBase>();
            while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses)){ //as long as we are inside the loop
                //We create the correct Statement type (if, while, return, let) based on the top token in the stack
                StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                //And call the Parse method of the statement to parse the different parts of the statement 
                s.Parse(sTokens);
                DoIfTrue.Add(s);

            }
            
            t = sTokens.Pop();//}
            if (!(t is Parentheses))
                throw new SyntaxErrorException("Expected } received: " + t, t);
            if (((Parentheses)t).Name != '}')
                throw new SyntaxErrorException("Expected } received: " + t, t);

            DoIfFalse = new List<StatetmentBase>();
            if (sTokens.Count > 0 && sTokens.Peek() is Statement ) {
                if (((Statement)sTokens.Peek()).Name == "else")
                {

                    sTokens.Pop(); //we already checked else
                    t = sTokens.Pop();
                    if(!(t is Parentheses))
                        throw new SyntaxErrorException("Expected { received: " + t, t);
                    if(((Parentheses)t).Name != '{')
                        throw new SyntaxErrorException("Expected { received: " + t, t);
                    while (sTokens.Count > 0 && !(sTokens.Peek() is Parentheses)){
                        //We create the correct Statement type (if, while, return, let) based on the top token in the stack
                        StatetmentBase s = StatetmentBase.Create(sTokens.Peek());
                        //And call the Parse method of the statement to parse the different parts of the statement 
                        s.Parse(sTokens);
                        DoIfFalse.Add(s);
                    }
                    t = sTokens.Pop();//}
                    if (!(t is Parentheses))
                        throw new SyntaxErrorException("Expected } received: " + t, t);
                    if (((Parentheses)t).Name != '}')
                        throw new SyntaxErrorException("Expected } received: " + t, t);
                }
            }


        }

        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
