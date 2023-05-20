using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A demux has 2 outputs. There is a single input and a control bit, selecting whether the input should be directed to the first or second output.
    class Demux : Gate
    {
        public Wire Output1 { get; private set; }
        public Wire Output2 { get; private set; }
        public Wire Input { get; private set; }
        public Wire Control { get; private set; }

        //your code here
        private AndGate m_gAND1;
        private NotGate m_gNOT;
        private AndGate m_gAND2;
        private Wire controlWireTest;
        private Wire inputWireTest;
        public Demux()
        {
            Input = new Wire();
            Control = new Wire();
            m_gAND1 = new AndGate();
            m_gAND2 = new AndGate();
            m_gNOT = new NotGate();

            //the first AND gate
            m_gNOT.ConnectInput(Control);
            m_gAND1.ConnectInput1(m_gNOT.Output);
            m_gAND1.ConnectInput2(Input);
            Output1 = m_gAND1.Output;

            //the 2nd AND gate 
            m_gAND2.ConnectInput1(Control);
            m_gAND2.ConnectInput2(Input);
            Output2 = m_gAND2.Output;
        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }



        public override bool TestGate()
        {
            controlWireTest = new Wire();
            //c=0
            controlWireTest.Value = 0;
            if (!Control.InputConected)
                ConnectControl(controlWireTest);


            //x=0
            //Input.Value = 0;
            inputWireTest = new Wire();
            inputWireTest.Value = 0;
            if (!Input.InputConected)
                ConnectInput(inputWireTest);

            //control =0(x), output should be x
            if (Output1.Value != 0)
                return false;
            if (Output2.Value != 0)
                return false;

            //c=0,x=1
            inputWireTest.Value = 1;
            if (Output1.Value != 1)
                return false;
            if (Output2.Value != 0)
                return false;

            controlWireTest.Value = 1;
            inputWireTest.Value = 0;
            //c=1,x=0
            if (Output1.Value != 0)
                return false;
            if (Output2.Value != 0)
                return false;

            inputWireTest.Value = 1;
            //c=1,x=1
            if (Output1.Value != 0)
                return false;
            if (Output2.Value != 1)
                return false;
            return true;
        }
    }
}
