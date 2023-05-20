using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a register that can maintain 1 bit.
    class SingleBitRegister : Gate
    {
        public Wire Input { get; private set; }
        public Wire Output { get; private set; }
        //A bit setting the register operation to read or write
        public Wire Load { get; private set; }

        public SingleBitRegister()
        {
            
            Input = new Wire();
            Load = new Wire();
            Output = new Wire();
            DFlipFlopGate DFF = new DFlipFlopGate();
            MuxGate demuxDFF = new MuxGate();
            demuxDFF.ConnectInput1(DFF.Output);
            demuxDFF.ConnectInput2(Input);
            demuxDFF.ConnectControl(Load);
            DFF.ConnectInput(demuxDFF.Output);
            Output.ConnectInput(DFF.Output);

        }

        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }

      

        public void ConnectLoad(Wire wLoad)
        {
            Load.ConnectInput(wLoad);
        }


        public override bool TestGate()
        {
            Input.Value = 0;
            Load.Value = 0;
            if (Output.Value != 0)
                return false;

            Clock.ClockDown();
            Clock.ClockUp();

            Input.Value = 1;
            if (Output.Value != 0)
                return false;

            Clock.ClockDown();
            Clock.ClockUp();

            Load.Value = 1;
            Input.Value = 0;
            if (Output.Value != 0)
                return false;

            Clock.ClockDown();
            Clock.ClockUp();

            Load.Value = 1;
            Input.Value = 1;
            if (Output.Value != 0)
                return false;

            Clock.ClockDown();
            Clock.ClockUp();

            if (Output.Value != 1)
                return false;

            Clock.ClockDown();
            Clock.ClockUp();

            Load.Value = 0;
            Input.Value = 0;
            if (Output.Value != 1)
                return false;

            return true;
        }
    }
}
