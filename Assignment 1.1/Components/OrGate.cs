﻿using System;
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
            //create new gates
            m_gAnd = new AndGate();
            m_gNot_x = new NotGate();
            m_gNot_y = new NotGate();
            m_gNot_after = new NotGate();
            //connect the two inputs into the not gates
            m_gNot_x.ConnectInput(Input1);
            m_gNot_y.ConnectInput(Input2);
            //connect the two nots into the and gate
            m_gAnd.ConnectInput1(m_gNot_x.Output);
            m_gAnd.ConnectInput2(m_gNot_y.Output);
            //connect the and gate into not
            m_gNot_after.ConnectInput(m_gAnd.Output);
            //output
            Output= m_gNot_after.Output;
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
