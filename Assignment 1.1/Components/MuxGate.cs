using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A mux has 2 inputs. There is a single output and a control bit, selecting which of the 2 inpust should be directed to the output.
    
    class MuxGate : TwoInputGate
    {
        public Wire ControlInput { get; private set; }
        //your code here
        private AndGate m_gAnd1;
        private AndGate m_gAnd2;
        private OrGate m_gOr;
        private NotGate m_not;
        private Wire controlWire;

        public MuxGate()
        {
            ControlInput = new Wire();
            m_not = new NotGate();
            m_gAnd1 = new AndGate();
            m_gOr = new OrGate();
            m_gAnd2 = new AndGate();

            //connect the control wire
            //ConnectControl(ControlInput);
            //connect the x and the not to the "top" AND gate
            m_not.ConnectInput(ControlInput);
            ControlInput.ConnectOutput(m_not);
            m_gAnd1.ConnectInput1(m_not.Output);
            m_gAnd1.ConnectInput2(Input1);

            //connect the y and the control to the "botton" AND gate
            m_gAnd2.ConnectInput1(ControlInput);
            m_gAnd2.ConnectInput2(Input2);
            
            //connect the two outputs of the and gates
            m_gOr.ConnectInput1(m_gAnd1.Output);
            m_gOr.ConnectInput2(m_gAnd2.Output);
            Output = m_gOr.Output;
        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }


        public override string ToString()
        {
            return "Mux " + Input1.Value + "," + Input2.Value + ",C" + ControlInput.Value + " -> " + Output.Value;
        }

        public override bool TestGate()
        {
            controlWire = new Wire();
            controlWire.Value = 0;
            if(!ControlInput.InputConected)
                ConnectControl(controlWire);

            
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;

            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 0)
                return false;

            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1) //error here
                return false;

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            //
            controlWire.Value = 1;
            //

            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;

            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1) //this line here
                return false;

            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;

            return true;
        }
    }
}
