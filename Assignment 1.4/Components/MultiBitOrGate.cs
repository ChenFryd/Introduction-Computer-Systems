using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)

    class MultiBitOrGate : MultiBitGate
    {
        private OrGate[] OrGates;
        private int size;
        public MultiBitOrGate(int iInputCount): base(iInputCount)
        {
            size = iInputCount;
            OrGates = new OrGate[iInputCount];
            OrGates[0] = new OrGate(); // create the first or gate
            OrGates[0].ConnectInput1(m_wsInput[0]);//connect the first two inputs to the first or gate
            OrGates[0].ConnectInput2(m_wsInput[1]);
            if (iInputCount >= 2)
            {
                for (int i = 1; i < iInputCount -1; i++)
                {

                    OrGates[i] = new OrGate();
                    Wire output_var = OrGates[i - 1].Output;
                    OrGates[i].ConnectInput1(output_var);
                    OrGates[i].ConnectInput2(m_wsInput[i + 1]);
                }
            }
            Output = OrGates[iInputCount -2].Output;

        }

        public override bool TestGate()
        {
            for (int i = 0; i < size; i++)
            {
                m_wsInput[i].Value = 1;
            }
            if (Output.Value != 1)
            { return false; }
            m_wsInput[1].Value = 0;
            if (Output.Value != 1)
            { return false; }

            return true;
        }
    }
}
