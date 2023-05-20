using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseDemux : Gate
    {
        public int Size { get; private set; }
        public WireSet Output1 { get; private set; }
        public WireSet Output2 { get; private set; }
        public WireSet Input { get; private set; }
        public Wire Control { get; private set; }

        private Demux[] demuxGatesOutput;
        public BitwiseDemux(int iSize)
        {
            Size = iSize;
            Control = new Wire();
            Input = new WireSet(Size);
            demuxGatesOutput = new Demux[iSize];
            Output1 = new WireSet(iSize);
            Output2 = new WireSet(iSize);
            for (int i = 0; i < iSize; i++)
            {
                demuxGatesOutput[i] = new Demux();
                demuxGatesOutput[i].ConnectControl(Control);
                demuxGatesOutput[i].ConnectInput(Input[i]);
                Output1[i].ConnectInput(demuxGatesOutput[i].Output1);
                Output2[i].ConnectInput(demuxGatesOutput[i].Output2);

            }
        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        public override bool TestGate()
        {
            Control.Value = 0;
            //inp1=0, inp2=0
            for (int i = 0; i < Size; i++)
            {
                Input[i].Value = 0;
                if (Output1[i].Value != 0)
                    return false;
                if (Output2[i].Value != 0)
                    return false;
            }
            //c=0
            for (int i = 0; i < Size; i++)
            {
                Input[i].Value = 1;
                if (Output1[i].Value != 1)
                    return false;
                if (Output2[i].Value != 0)
                    return false;
            }
            Control.Value = 1;
            //c=1
            for (int i = 0; i < Size; i++)
            {
                Input[i].Value = 0;
                if (Output1[i].Value != 0)
                    return false;
                if (Output2[i].Value != 0)
                    return false;
            }
            //c=1
            for (int i = 0; i < Size; i++)
            {
                Input[i].Value = 1;
                if (Output1[i].Value != 0)
                    return false;
                if (Output2[i].Value != 1)
                    return false;
            }
            return true;
        }
    }
}
