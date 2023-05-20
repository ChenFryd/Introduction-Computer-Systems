using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a FullAdder, taking as input 3 bits - 2 numbers and a carry, and computing the result in the output, and the carry out.
    class FullAdder : TwoInputGate
    {
        public Wire CarryInput; //{ get; private set; }
        public Wire CarryOutput; //{ get; private set; }

        //your code here
        public HalfAdder halfAdder1;
        private XorGate xorGate2;
        private AndGate andGate2;
        private OrGate orGate2;

        public FullAdder()
        {
            CarryInput = new Wire();
            halfAdder1 = new HalfAdder();

            halfAdder1.ConnectInput1(Input1); // init halfAdder1
            halfAdder1.ConnectInput2(Input2);

            xorGate2 = new XorGate();
            xorGate2.ConnectInput1(CarryInput);
            xorGate2.ConnectInput2(halfAdder1.Output);

            andGate2 = new AndGate();
            andGate2 = new AndGate();
            andGate2.ConnectInput1(CarryInput);
            andGate2.ConnectInput2(halfAdder1.Output);

            orGate2 = new OrGate();
            orGate2.ConnectInput1(andGate2.Output);
            orGate2.ConnectInput2(halfAdder1.CarryOutput);

            Output = xorGate2.Output;
            CarryOutput = orGate2.Output;

        }


        public override string ToString()
        {
            return Input1.Value + "+" + Input2.Value + " (C" + CarryInput.Value + ") = " + Output.Value + " (C" + CarryOutput.Value + ")";
        }

        public override bool TestGate()
        {
            CarryInput.Value = 0;
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0 && CarryOutput.Value != 0)
                return false;

            CarryInput.Value = 1;
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 1 && CarryOutput.Value != 0)
                return false;

            CarryInput.Value = 0;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1 && CarryOutput.Value != 0)
                return false;

            CarryInput.Value = 0;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1 && CarryOutput.Value != 0)
                return false;

            CarryInput.Value = 1;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 0 && CarryOutput.Value != 1)
                return false;

            CarryInput.Value = 1;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 0 && CarryOutput.Value != 1)
                return false;

            CarryInput.Value = 0;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0 && CarryOutput.Value != 1)
                return false;

            CarryInput.Value = 1;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1 && CarryOutput.Value != 1)
                return false;

            return true;
        }
    }
}
