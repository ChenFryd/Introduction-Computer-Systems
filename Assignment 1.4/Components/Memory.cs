using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a memory unit, containing k registers, each of size n bits.
    class Memory : SequentialGate
    {
        //The address size determines the number of registers
        public int AddressSize { get; private set; }
        //The word size determines the number of bits in each register
        public int WordSize { get; private set; }

        //Data input and output - a number with n bits
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        //The address of the active register
        public WireSet Address { get; private set; }
        //A bit setting the memory operation to read or write
        public Wire Load { get; private set; }

        private WireSet loadws;
        public Memory(int iAddressSize, int iWordSize)
        {
            AddressSize = iAddressSize;
            WordSize = iWordSize;

            Input = new WireSet(WordSize);
            Output = new WireSet(WordSize);
            Address = new WireSet(AddressSize);
            Load = new Wire();

            //wire load
            WireSet wireSetRW = new WireSet(1);
            wireSetRW[0].ConnectInput(Load);

            //demux at the start
            BitwiseMultiwayDemux demuxAddress = new BitwiseMultiwayDemux(1,AddressSize);
            demuxAddress.ConnectInput(wireSetRW);
            demuxAddress.ConnectControl(Address);

            //mux at the end
            BitwiseMultiwayMux muxAddress = new BitwiseMultiwayMux(WordSize,AddressSize);
            muxAddress.ConnectControl(Address);
            Output.ConnectInput(muxAddress.Output);

            //multiBitRegister array
            int MBRsize = (int)Math.Pow(2, AddressSize);
            MultiBitRegister[] arrayMultiBitRegister = new MultiBitRegister[MBRsize];
            for (int i = 0; i < MBRsize; i++)
            {
                arrayMultiBitRegister[i] = new MultiBitRegister(WordSize);
                arrayMultiBitRegister[i].ConnectInput(Input);
                arrayMultiBitRegister[i].Load.ConnectInput(demuxAddress.Outputs[i][0]);
                muxAddress.Inputs[i].ConnectInput(arrayMultiBitRegister[i].Output);
            }

        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectAddress(WireSet wsAddress)
        {
            Address.ConnectInput(wsAddress);
        }


        public override void OnClockUp()
        {
        }

        public override void OnClockDown()
        {
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override bool TestGate()
        {
            Input.SetValue(6);
            Load.Value = 1;
            Address.SetValue(0);

            Clock.ClockDown();
            Clock.ClockUp();

            if (Output.GetValue() != 6)
                return false;

            Input.SetValue(1);

            Clock.ClockDown();
            Clock.ClockUp();

            if (Output.GetValue() != 1)
                return false;
            return true;
        }
    }
}
