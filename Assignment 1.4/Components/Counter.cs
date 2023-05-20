using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Counter : Gate
    {
        private int m_iValue;
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public Wire Set { get; private set; }
        public int Size { get; private set; }

        //The counter contains a register, and supports two possible operations:
        //1. Set = 0: Incrementing the current register value by 1 (++)
        //2. Set = 1: Setting the register to a new value

        public Counter(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Set = new Wire();

            //BitWiseMux
            BitwiseMux muxSet = new BitwiseMux(Size);
            muxSet.ConnectInput2(Input);
            muxSet.ConnectControl(Set);

            //n bit reg
            MultiBitRegister nBitRegister = new MultiBitRegister(Size);
            nBitRegister.ConnectInput(muxSet.Output);
            nBitRegister.Load.Value = 1;

            //adder
            MultiBitAdder multiAdder = new MultiBitAdder(Size);
            multiAdder.ConnectInput1(nBitRegister.Output);

            WireSet wireSetOne = new WireSet(Size);
            wireSetOne[0].Value = 1;
            multiAdder.ConnectInput2(wireSetOne);
            muxSet.ConnectInput1(multiAdder.Output);

            Output.ConnectInput(nBitRegister.Output);

        }

        public void ConnectInput(WireSet ws)
        {
            Input.ConnectInput(ws);
        }
        
        public void ConnectReset(Wire w)
        {
            Set.ConnectInput(w);
        }

        public override string ToString()
        {
            return Output.ToString();
        }

        

        public override bool TestGate()
        {
            Input.SetValue(5);
            Set.Value = 1;

            Clock.ClockDown();
            Clock.ClockUp();

            if (Output.GetValue() != 5)
                return false;

            Set.Value = 0;

            Clock.ClockDown();
            Clock.ClockUp();

            if (Output.GetValue() != 6)
                return false;

            return true;
        }
    }
}
