using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseAndGate : BitwiseTwoInputGate
    {
        //your code here

        private AndGate[] andGatesOutput;
        public BitwiseAndGate(int iSize) : base(iSize)
        {

            andGatesOutput = new AndGate[iSize];
            for (int i = 0; i<iSize; i++)
            {

                andGatesOutput[i] = new AndGate();
                andGatesOutput[i].ConnectInput1(Input1[i]);
                andGatesOutput[i].ConnectInput2(Input2[i]);

                Output[i].ConnectInput(andGatesOutput[i].Output);
            }

        }
        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(and)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "And " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            //inp1=0, inp2=0
            for (int i = 0; i < Size; i++)
            {
                Input1[i].Value = 0;

                Input2[i].Value = 0;
                int temp_output = Output[i].Value;
                if (temp_output != 0)
                { return false; }
            }

            //inp1=0, inp2=1
            for (int i = 0; i < Size; i++)
            {
                Input1[i].Value = 0;
                Input2[i].Value = 1;
                int temp_output = Output[i].Value;
                if (temp_output != 0)
                { return false; }
            }
            //inp1=1, inp2=0
            for (int i = 0; i < Size; i++)
            {
                Input1[i].Value = 1;
                Input2[i].Value = 0;
                int temp_output = Output[i].Value;
                if (temp_output != 0)
                {
                    return false;
                }
            }
            //inp1=1, inp2=1
            for (int i = 0; i < Size; i++)
            {
                Input1[i].Value = 1;
                Input2[i].Value = 1;
                int temp_output = Output[i].Value;
                if (temp_output != 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
