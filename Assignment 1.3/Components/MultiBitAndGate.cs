using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)
    class MultiBitAndGate : MultiBitGate
    {
        private AndGate[] AndGates;
        private int size;
        public MultiBitAndGate(int iInputCount)
            : base(iInputCount)
        {
            size = iInputCount;
            AndGates = new AndGate[iInputCount]; //new array of and gates
            AndGates[0] = new AndGate();
            AndGates[0].ConnectInput1(m_wsInput[0]); //create the first 1
            AndGates[0].ConnectInput2(m_wsInput[1]);
            for (int i = 1; i < iInputCount-1; i++)
            {
                AndGates[i] = new AndGate();
                Wire Output_var = AndGates[i - 1].Output;
                AndGates[i].ConnectInput1(Output_var); //connect the last gate
                AndGates[i].ConnectInput2(m_wsInput[i+1]); 
            }
            
            Output = AndGates[iInputCount -2 ].Output;//-2 because the first two go to the first and gate and another because of the array
        }

        public override bool TestGate()
        {
            //a test for Multibitand gate
            for (int i = 0; i < size; i++)
            {
                m_wsInput[i].Value = 1;
            }
            if (Output.Value != 1)
            { return false; }

            m_wsInput[1].Value = 0;
            if (Output.Value != 0)
            { return false; }

            return true;

        }
    }
}
