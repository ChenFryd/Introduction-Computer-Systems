using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseMux : BitwiseTwoInputGate
    {
        public Wire ControlInput { get; private set; }
        private MuxGate[] muxGatesOutput;
        public int input_length;

        public BitwiseMux(int iSize)
            : base(iSize)
        {
            input_length = iSize;
            muxGatesOutput = new MuxGate[input_length];
            ControlInput = new Wire();
            for (int i = 0; i < iSize; i++)
            {
                //create the next multiplexer gate
                muxGatesOutput[i] = new MuxGate();
                muxGatesOutput[i].ConnectInput1(Input1[i]);
                muxGatesOutput[i].ConnectInput2(Input2[i]);
                muxGatesOutput[i].ConnectControl(ControlInput);
                Output[i].ConnectInput(muxGatesOutput[i].Output);
            }
        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }

        public override string ToString()
        {
            return "Mux " + Input1 + "," + Input2 + ",C" + ControlInput.Value + " -> " + Output;
        }




        public override bool TestGate()
        {
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 0; //init all the first wireset to 0
                Input2[i].Value = 0; //init all the second wireset to 0
                ControlInput.Value = 0; //init all the control wireset to 0
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 1; //init all the first wireset to 1
                Input2[i].Value = 0; //init all the second wireset to 0
                ControlInput.Value = 0; //init all the control wireset to 0
                if (Output[i].Value != 1)
                    return false;
            }
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 0; //init all the first wireset to 1
                Input2[i].Value = 1; //init all the second wireset to 0
                ControlInput.Value = 0; //init all the control wireset to 0
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 1; //init all the first wireset to 1
                Input2[i].Value = 1; //init all the second wireset to 1
                ControlInput.Value = 0; //init all the control wireset to 0
                if (Output[i].Value != 1)
                    return false;
            }
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 0; //init all the first wireset to 0
                Input2[i].Value = 0; //init all the second wireset to 0
                ControlInput.Value = 1; //init all the control wireset to 1
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 0; //init all the first wireset to 0
                Input2[i].Value = 1; //init all the second wireset to 1
                ControlInput.Value = 1; //init all the control wireset to 1
                if (Output[i].Value != 1)
                    return false;
            }
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 1; //init all the first wireset to 1
                Input2[i].Value = 0; //init all the second wireset to 0
                ControlInput.Value = 1; //init all the control wireset to 1
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 0; i < input_length; i++)
            {
                Input1[i].Value = 1; //init all the first wireset to 1
                Input2[i].Value = 1; //init all the second wireset to 1
                ControlInput.Value = 1; //init all the control wireset to 1
                if (Output[i].Value != 1)
                    return false;
            }

            return true;
        }
    }
}
