using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a demux with k outputs, each output with n wires. The input also has n wires.

    class BitwiseMultiwayDemux : Gate
    {
        //Word size - number of bits in each output
        public int Size { get; private set; }

        //The number of control bits needed for k outputs
        public int ControlBits { get; private set; }

        public WireSet Input { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Outputs { get; private set; }
        public int bitWiseArraySize;
        public BitwiseDemux[] bitWiseArray;
        public int controlCounter=0;
        public int cControl; //for the tests
        //your code here

        public BitwiseMultiwayDemux(int iSize, int cControlBits)
        {
            cControl = cControlBits;
            Size = iSize;
            Input = new WireSet(Size);
            Control = new WireSet(cControlBits);
            bitWiseArraySize = (int)Math.Pow(2, cControlBits)-1;
            bitWiseArray = new BitwiseDemux[bitWiseArraySize]; //full tree is 2^k-1 items
            Outputs = new WireSet[bitWiseArraySize+1];

            for (int i = 0; i < bitWiseArraySize + 1; i++)
            {
                Outputs[i] = new WireSet(Size); //init the wireset
            }

            for (int i = 0; i < bitWiseArray.Length; i++) //init the array Demux
            {
                bitWiseArray[i] = new BitwiseDemux(Size);
            }
            for (int i = 1; i < bitWiseArraySize+1; i++)
            {
                
                if (i == 1)
                    bitWiseArray[i - 1].ConnectInput(Input);
                else
                {
                    if (i % 2 == 0)
                        bitWiseArray[i - 1].ConnectInput(bitWiseArray[(i / 2)-1].Output1);
                    else //i%2==1
                        bitWiseArray[i - 1].ConnectInput(bitWiseArray[((i - 1) / 2)-1].Output2);
                }
            }
            for (int i = (bitWiseArraySize-(int)Math.Pow(2,cControlBits-1)); i < bitWiseArraySize; i++)//init the outputs, start at the leafs 
            {
                Outputs[controlCounter].ConnectInput(bitWiseArray[i].Output1);
                controlCounter++;
                Outputs[controlCounter].ConnectInput(bitWiseArray[i].Output2);
                controlCounter++;
            }
            controlCounter = 0; //restart the couner to reconnect the conrolinputs to gates
            for (int k = 0; k < cControlBits; k++)
            {
                for (int i = 0; i < (int)Math.Pow(2, k); i++)
                {
                    if (controlCounter <= (bitWiseArraySize))
                    {
                        bitWiseArray[controlCounter].ConnectControl(Control[(cControlBits - 1) - k]); //connect the control to all the mux gates
                        controlCounter++;
                    }
                }
            }

        }


        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }


        public override bool TestGate()
        {
            WireSet wireSetOutput = new WireSet(cControl);
            Control.ConnectInput(wireSetOutput);
            for (int i = 0; i < (int)Math.Pow(2,cControl); i++)
            {
                Input.SetValue(i);
                wireSetOutput.SetValue(i);
                if (Outputs[i].GetValue() != i)
                    return false;
            }

            return true;
        }
    }
}
