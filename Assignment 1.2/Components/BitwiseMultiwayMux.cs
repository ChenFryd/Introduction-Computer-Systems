using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a mux with k input, each input with n wires. The output also has n wires.

    class BitwiseMultiwayMux : Gate
    {
        //Word size - number of bits in each output
        public int Size { get; private set; }

        //The number of control bits needed for k outputs
        public int ControlBits { get; private set; }

        public WireSet Output { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Inputs { get; private set; }

        public BitwiseMux[] bitWiseArray;

        public int bitWiseArraySize;

        public int inputCounter; // counts the times we put a value from Inputs to bitWiseArray
        public int controlCounter = 0;
        public int cControl;
        //your code here

        public BitwiseMultiwayMux(int iSize, int cControlBits)
        {
            inputCounter = 0;
            cControl = cControlBits;
            Size = iSize; //n size
            Output = new WireSet(Size); //n size
            Control = new WireSet(cControlBits); //k size
            bitWiseArraySize = (int)Math.Pow(2, cControlBits) -1;//size of 2^k-1
            Inputs = new WireSet[(int)Math.Pow(2, cControlBits)]; //2^k size
            bitWiseArray = new BitwiseMux[bitWiseArraySize];//to save the bitwise
            
            for (int i = 0; i < bitWiseArraySize; i++)
            {
                bitWiseArray[i] = new BitwiseMux(Size); //init the array
                
            }
            for (int i = 0; i <= bitWiseArraySize; i++)
            {
                Inputs[i] = new WireSet(Size);
            }
            for (int k = 0; k < cControlBits; k++)
            {
                for (int i = 0; i < (int)Math.Pow(2,k); i++)
                {
                    if (controlCounter <= (bitWiseArraySize))
                    {
                        bitWiseArray[controlCounter].ConnectControl(Control[(cControlBits - 1)-k]); //connect the control to all the mux gates
                        controlCounter++;
                    }
                }
            }
            for (int i = 1 ; i < (bitWiseArraySize+1); i++)
            {
                //i'm initianting a heap
                Inputs[i] = new WireSet(Size);


                if ((2 * (i)-1) < (bitWiseArraySize))
                    bitWiseArray[i-1].ConnectInput1(bitWiseArray[2 * (i)-1].Output); //right son
                else
                {
                    bitWiseArray[i-1].ConnectInput1(Inputs[inputCounter]); //connect the input wireset to the leafs
                    inputCounter++; 
                }

                if ((2 * (i)) < (bitWiseArraySize))
                {
                    bitWiseArray[i-1].ConnectInput2(bitWiseArray[2 * (i)].Output); //left son
                }
                else
                {
                    bitWiseArray[i-1].ConnectInput2(Inputs[inputCounter]);//connect the input wireset to the leafs
                    inputCounter++;
                }
            }
            Output.ConnectInput(bitWiseArray[0].Output); //connect the output of the head of the tree
           

        }


        public void ConnectInput(int i, WireSet wsInput)
        {
            Inputs[i].ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }



        public override bool TestGate()
        {
            //init the control bits, k size
            for (int i = 0; i < Control.Size; i++)
            {
                Control[i].Value = 0;
            }
            //test 1, c=0,even=0,odd=0
            //init the inputs, n size for 2^k wireset
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 0)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 1 failed");
                    return false;
                }

            }


            //test 2, c=0,even=1,odd=0,o=1
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 1
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 1)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 2 failed");
                    return false;
                }
            }
            //test 3, c=0,even=0,odd=1,o=0
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 1
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 0)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 3 failed");
                    return false;
                }
            }
            //test 4, c=0,even=1,odd=1,o=1
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 0
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 1
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 1)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 4 failed");
                    return false;
                }
            }

            //init the control bits, k size
            for (int i = 0; i < Control.Size; i++)
            {
                Control[i].Value = 1;
            }

            //test 5, c=1,even=0,odd=0,o=0
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 0)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 5 failed");
                    return false;
                }
            }
            //test 6, c=1,even=1,odd=0,o=0
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 0
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 0)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 6 failed");
                    return false;
                }
            }
            //test 7, c=1,even=0,odd=1,o=1
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 0
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 1
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 1)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 7 failed");
                    return false;
                }
            }
            //test 8, c=1,even=1,odd=1,o=1
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 1
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 1; //init all the wires to 1
                }
            }
            for (int k = 0; k < Output.Size; k++)
            {
                if (Output[k].Value != 1)
                {
                    Console.WriteLine("bitwiseMultiwayMux test 8 failed");
                    return false;
                }
            }
            WireSet outputWireSet = new WireSet(cControl);
            outputWireSet.SetValue(1);
            Control.ConnectInput(outputWireSet);
            for (int i = 0; i < bitWiseArraySize + 1; i += 2) //even
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 1
                }
            }
            for (int i = 1; i < bitWiseArraySize + 1; i += 2) //odd
            {
                for (int j = 0; j < Size; j++)
                {
                    Inputs[i][j].Value = 0; //init all the wires to 1
                }
            }



            return true;

        }
    }
}
