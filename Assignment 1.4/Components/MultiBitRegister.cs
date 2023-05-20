using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class represents an n bit register that can maintain an n bit number
    class MultiBitRegister : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        //A bit setting the register operation to read or write
        public Wire Load { get; private set; }

        //Word size - number of bits in the register
        public int Size { get; private set; }
        

        public MultiBitRegister(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Load = new Wire();
            SingleBitRegister[] array1BR = new SingleBitRegister[Size];
            //init the size 1br
            for (int i = 0; i < Size; i++)
            {
                array1BR[i] = new SingleBitRegister();
                array1BR[i].ConnectLoad(Load);
                array1BR[i].ConnectInput(Input[i]);
                Output[i].ConnectInput(array1BR[i].Output);
            }

        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        
        public override string ToString()
        {
            return Output.ToString();
        }


        public override bool TestGate()
        {
            Input.SetValue(5);
            Load.Value = 1;
            Clock.ClockDown();
            Clock.ClockUp();
            if (this.Output.GetValue() != 5)
                return false;

            Input.SetValue(0);
            Load.Value = 1;
            Clock.ClockDown();
            Clock.ClockUp();

            if (this.Output.GetValue() != 0)
                return false;

            Load.Value = 0;
            Clock.ClockDown();
            Clock.ClockUp();
            if (this.Output.GetValue() != 0)
                return false;

            Input.SetValue(2);
            Load.Value = 1;
            Clock.ClockDown();
            Clock.ClockUp();
            if (this.Output.GetValue() != 2)
                return false;

            Load.Value = 0;
            Clock.ClockDown();
            Clock.ClockUp();
            if (this.Output.GetValue() != 2)
                return false;

            return true;
        }
    }
}
