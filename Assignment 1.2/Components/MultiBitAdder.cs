using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements an adder, receving as input two n bit numbers, and outputing the sum of the two numbers
    class MultiBitAdder : Gate
    {
        //Word size - number of bits in each input
        public int Size { get; private set; }

        public WireSet Input1 { get; private set; }
        public WireSet Input2 { get; private set; }
        public WireSet Output { get; private set; }
        //An overflow bit for the summation computation
        public Wire Overflow { get; private set; }
        private Wire zeroWire;
        private FullAdder[] fullAdderArray;
        public MultiBitAdder(int iSize)
        {
            Size = iSize;
            Input1 = new WireSet(Size);
            Input2 = new WireSet(Size);
            Output = new WireSet(Size);
            fullAdderArray = new FullAdder[Size-1];
            Overflow = new Wire();
            HalfAdder firstHalfAdder = new HalfAdder();

            for (int i = 0; i < Size; i++)
            {
                if (i == 0) //first adder is half adder
                {
                    firstHalfAdder.ConnectInput1(Input1[i]);
                    firstHalfAdder.ConnectInput2(Input2[i]);
                }
                else //not a first adder, so full adder
                {
                    fullAdderArray[i-1] = new FullAdder();
                    fullAdderArray[i-1].ConnectInput1(Input1[i]);
                    fullAdderArray[i-1].ConnectInput2(Input2[i]);
                }

                if (i == 1) // connect the second adder to the first adder
                    fullAdderArray[i-1].CarryInput.ConnectInput(firstHalfAdder.CarryOutput);
                else if (i > 1) //connect all the adders to the previous adder
                    fullAdderArray[i-1].CarryInput.ConnectInput(fullAdderArray[i - 2].CarryOutput);
            }

            Output[0].ConnectInput(firstHalfAdder.Output);
            for (int i = 1; i < (fullAdderArray.Length + 1); i++)
                Output[i].ConnectInput(fullAdderArray[i - 1].Output);
            //Console.WriteLine(fullAdderArray[(fullAdderArray.Length - 1)].CarryOutput.Value);
            Overflow.ConnectInput(fullAdderArray[2].CarryOutput);

        }

        public override string ToString()
        {
            return Input1 + "(" + Input1.Get2sComplement() + ")" + " + " + Input2 + "(" + Input2.Get2sComplement() + ")" + " = " + Output + "(" + Output.Get2sComplement() + ")";
        }

        public void ConnectInput1(WireSet wInput)
        {
            Input1.ConnectInput(wInput);
        }
        public void ConnectInput2(WireSet wInput)
        {
            Input2.ConnectInput(wInput);
        }


        public override bool TestGate()
        {
            for (int i = 0; i < Input1.Size; i++)
            {
                Input1[i].Value = 0;
            }
            for (int i = 0; i < Input2.Size; i++)
            {
                Input2[i].Value = 0;
            }
            for (int i = 0; i < Output.Size; i++)
            {
                if (Output[i].Value != 0 && Overflow.Value != 0 )
                    return false;
            }
            ///
            for (int i = 0; i < Input1.Size; i++)
            {
                Input1[i].Value = 1;
            }
            for (int i = 0; i < Input2.Size; i++)
            {
                Input2[i].Value = 0;
            }
            for (int i = 0; i < Output.Size; i++)
            {
                if (Output[i].Value != 1 && Overflow.Value != 0)
                    return false;
            }

            ///
            for (int i = 0; i < Input1.Size; i++)
            {
                Input1[i].Value = 0;
            }
            for (int i = 0; i < Input2.Size; i++)
            {
                Input2[i].Value = 1;
            }
            for (int i = 0; i < Output.Size; i++)
            {
                if (Output[i].Value != 1 && Overflow.Value != 0)
                    return false;
            }

            //

            for (int i = 0; i < Input1.Size; i++)
            {
                Input1[i].Value = 1;
            }
            for (int i = 0; i < Input2.Size; i++)
            {
                Input2[i].Value = 1;
            }
            for (int i = 0; i < Output.Size; i++)
            {
                if (Output[i].Value != 0 && Overflow.Value != 1)
                    return false;
            }
            //
            for (int i = 0; i < Input1.Size; i++)
            {
                Input1[i].Value = 0;
            }
            for (int i = 0; i < Input2.Size; i++)
            {
                Input2[i].Value = 0;
            }
            Input1[0].Value = 1;
            Input2[0].Value = 1;
            for (int i = 0; i < Output.Size; i++)
            {
                if (i==1)
                { 
                    if (Output[i].Value != 1)
                        return false;
                }
                else if (Output[i].Value != 0 && Overflow.Value != 0)
                    return false;
            }



            return true;
        }
    }
}
