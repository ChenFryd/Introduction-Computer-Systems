using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a HalfAdder, taking as input 2 bits - 2 numbers and computing the result in the output, and the carry out.

    class HalfAdder : TwoInputGate
    {
        public Wire CarryOutput { get; private set; }
        private AndGate AndGate1;
        private XorGate XorGate1;
        //your code here


        public HalfAdder()
        {
            XorGate1 = new XorGate(); //init the xor gate
            XorGate1.ConnectInput1(Input1);
            XorGate1.ConnectInput2(Input2);
            Output = XorGate1.Output;

            AndGate1 = new AndGate(); // init the And Gate
            AndGate1.ConnectInput1(Input1);
            AndGate1.ConnectInput2(Input2);
            CarryOutput = AndGate1.Output;
        }


        public override string ToString()
        {
            return "HA " + Input1.Value + "," + Input2.Value + " -> " + Output.Value + " (C" + CarryOutput + ")";
        }

        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if(Output.Value != 0)
                return false;
            if (CarryOutput.Value != 0)
                return false;

            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            if (CarryOutput.Value != 0)
                return false;

            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
                return false;
            if (CarryOutput.Value != 0)
                return false;

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0)
                return false;
            if (CarryOutput.Value != 1)
                return false;

            return true;
        }
    }
}
