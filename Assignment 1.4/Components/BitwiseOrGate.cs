using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseOrGate : BitwiseTwoInputGate
    {
        //your code here
        private OrGate[] orGatesOutput;
        private int SizeM;

        public BitwiseOrGate(int iSize)
            : base(iSize)
        {
            SizeM = iSize;
            orGatesOutput = new OrGate[iSize];
            for (int i = 0; i < iSize; i++)
            {
                orGatesOutput[i] = new OrGate();
                orGatesOutput[i].ConnectInput1(Input1[i]);
                orGatesOutput[i].ConnectInput2(Input2[i]);

                Output[i].ConnectInput(orGatesOutput[i].Output);
            }
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(or)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Or " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            //inp1=0, inp2=0
            for (int i = 0; i < SizeM; i++)
            {
                Input1[i].Value = 0;

                Input2[i].Value = 0;
                int temp_output = Output[i].Value;
                if (temp_output != 0)
                { return false; }
            }

            //inp1=0, inp2=1
            for (int i = 0; i < SizeM; i++)
            {
                Input1[i].Value = 0;
                Input2[i].Value = 1;
                int temp_output = Output[i].Value;
                if (temp_output != 1)
                { return false; }
            }
            //inp1=1, inp2=0
            for (int i = 0; i < SizeM; i++)
            {
                Input1[i].Value = 1;
                Input2[i].Value = 0;
                int temp_output = Output[i].Value;
                if (temp_output != 1)
                {
                    return false;
                }
            }
            //inp1=1, inp2=1
            for (int i = 0; i < SizeM; i++)
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
