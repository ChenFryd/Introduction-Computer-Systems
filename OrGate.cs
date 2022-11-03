using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the or operation. To implement it, follow the example in the And gate.
    class OrGate : TwoInputGate
    {
        //your code here 
        private AndGate m_gAnd;
        private NotGate m_gNot_x;
        private NotGate m_gNot_y;
        private NotGate m_gNot_after;

        public OrGate()
        {
            /* m_gAnd = new AndGate();
            m_gNot_x = new NotGate();
            m_gNot_y = new NotGate();
            m_gNot_after = new NotGate();

            Input1 = m_gNot_x.input1;
            Input2 = m_gNot_y.input2;

            m_gAnd.ConnectInput1(Input1);
            m_gAnd.ConnectInput2(Input2);
            Output = m_gNot_after.ConnectInput(m_gAnd.output); */
        }


        public override string ToString()
        {
            return "Or " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }

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
            if (Output.Value != 1)
                return false;

            return true;
        }
    }

}
