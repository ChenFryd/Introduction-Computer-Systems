using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the xor operation. To implement it, follow the example in the And gate.
    class XorGate : TwoInputGate
    {
        //your code here
        private NAndGate m_gNand;
        private OrGate m_gOr;
        private AndGate m_And;
        public XorGate()
        {
            m_gNand = new NAndGate();
            m_gOr = new OrGate();
            m_And = new AndGate();

            //connect two wires into the NAND gate
            m_gNand.ConnectInput1(Input1);
            m_gNand.ConnectInput2(Input2);

            //connect two wires into the or gate
            m_gOr.ConnectInput1(Input1);
            m_gOr.ConnectInput2(Input2);

            //connect the NAND and OR to AND
            m_And.ConnectInput1(m_gNand.Output);
            m_And.ConnectInput2(m_gOr.Output);
            Output = m_And.Output;
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(xor)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Xor " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }


        //this method is used to test the gate. 
        //we simply check whether the truth table is properly implemented.
        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0)
                return false;
            return true;
        }
    }
}
