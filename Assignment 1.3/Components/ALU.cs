using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class is used to implement the ALU
    class ALU : Gate
    {
        //The word size = number of bit in the input and output
        public int Size { get; private set; }

        //Input and output n bit numbers
        //inputs
        public WireSet InputX { get; private set; }
        public WireSet InputY { get; private set; }
        public WireSet Control { get; private set; }

        //outputs
        public WireSet Output { get; private set; }
        public Wire Zero { get; private set; }
        public Wire Negative { get; private set; }
        public BitwiseMultiwayMux Main_ALU;

        //your code here

        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            Control = new WireSet(6);
            Zero = new Wire();
            Negative = new Wire();
            Output = new WireSet(Size);

            WireSet[] Inputs = new WireSet[Size];
            BitwiseMultiwayMux Main_ALU = new BitwiseMultiwayMux(Size,6);
            Main_ALU.ConnectControl(Control);

            //00000 - 0
            WireSet wireSetZeroInput = new WireSet(Size);
            wireSetZeroInput.SetValue(0);
            Main_ALU.ConnectInput(0, wireSetZeroInput);

            //00001 - 1
            WireSet wireSetOneInput = new WireSet(Size);
            wireSetOneInput.SetValue(1);
            Main_ALU.ConnectInput(1,wireSetOneInput);

            //00010 - 2 - x
            Main_ALU.ConnectInput(2,InputX);

            //00011 - 3 - x
            Main_ALU.ConnectInput(3,InputY);

            //00100 - 4 - !x and 00101 - 5 - !y

            BitwiseMux bitwiseMuxNotXY = new BitwiseMux(Size);
            bitwiseMuxNotXY.ConnectInput1(InputX);
            bitwiseMuxNotXY.ConnectInput2(InputY);
            bitwiseMuxNotXY.ConnectControl(Control[0]);


            BitwiseNotGate bitwiseNotGate = new BitwiseNotGate(Size);
            bitwiseNotGate.ConnectInput(bitwiseMuxNotXY.Output);
            Main_ALU.ConnectInput(4, bitwiseNotGate.Output);
            Main_ALU.ConnectInput(5, bitwiseNotGate.Output);


            //00110 - 6 - (-x) and 00110 - y - (-y)

            


            WireSet wireSetMinusXMinusY = new WireSet(Size);
            MultiBitAdder multiBitAdderMinuxXY = new MultiBitAdder(Size);

            multiBitAdderMinuxXY.ConnectInput1(bitwiseNotGate.Output);
            multiBitAdderMinuxXY.ConnectInput2(wireSetOneInput);
            wireSetMinusXMinusY.ConnectInput(multiBitAdderMinuxXY.Output);

            
            Main_ALU.ConnectInput(6, wireSetMinusXMinusY);
            Main_ALU.ConnectInput(7, wireSetMinusXMinusY);

            ////01000  to 01110
            ////     see the map for the index of gates

            //bitwiseMuxXY_0, gate 0
            //input1:X input2:Y,Control = C[0], output: bitwiseMuxInput1MultiBitAdder_5
            BitwiseMux bitwiseMuxXY_0 = new BitwiseMux(Size);
            bitwiseMuxXY_0.ConnectInput1(InputX);
            bitwiseMuxXY_0.ConnectInput2(InputY);
            bitwiseMuxXY_0.ConnectControl(Control[0]);

            //bitwiseMuxOneMinusOne_1, gate 1
            //input1: 1 inpus2: -1, control = c[1],output: bitwiseMuxInput2MultiBitAdder_6
            BitwiseMux bitwiseMuxOneMinusOne_1 = new BitwiseMux(Size);
            WireSet wireSetMinusOne = new WireSet(Size);
            wireSetMinusOne.Set2sComplement(-1);
            bitwiseMuxOneMinusOne_1.ConnectInput1(wireSetOneInput);
            bitwiseMuxOneMinusOne_1.ConnectInput2(wireSetMinusOne);
            bitwiseMuxOneMinusOne_1.ConnectControl(Control[1]);

            //bitwiseMuxXY_2, gate 2
            //input1: x input2:y, control: c[1], output: bitwiseMuxInput1MultiBitAdder_5
            BitwiseMux bitwiseMuxXY_2 = new BitwiseMux(Size);
            bitwiseMuxXY_2.ConnectInput1(InputX);
            bitwiseMuxXY_2.ConnectInput2(InputY);
            bitwiseMuxXY_2.ConnectControl(Control[1]);

            //bitwiseMuxYMinusY_3, gate 3
            //input1: y, input2: -y, control: c[0], output: bitwiseMuxMinusYMinusX_4
            BitwiseMux bitwiseMuxYMinusY_3 = new BitwiseMux(Size);
            bitwiseMuxYMinusY_3.ConnectInput1(InputY);
            bitwiseMuxYMinusY_3.ConnectInput2(wireSetMinusXMinusY);
            bitwiseMuxYMinusY_3.ConnectControl(Control[0]);

            //bitwiseMuxMinusYMinusX_4, gate 4
            //input1: bitwiseMuxYMinusY_3, input2: -x, control: c[1], output: bitwiseMuxInput1MultiBitAdder_5
            BitwiseMux bitwiseMuxMinusYMinusX_4 = new BitwiseMux(Size);
            bitwiseMuxMinusYMinusX_4.ConnectInput1(bitwiseMuxYMinusY_3.Output);
            bitwiseMuxMinusYMinusX_4.ConnectInput2(wireSetMinusXMinusY);
            bitwiseMuxMinusYMinusX_4.ConnectControl(Control[1]);

            //bitwiseMuxInput1MultiBitAdder_5, gate 5
            //input1: bitwiseMuxXY_0 , input2: bitwiseMuxXY_2, control: c[2] ,output: MultiBitAdder
            BitwiseMux bitwiseMuxInput1MultiBitAdder_5 = new BitwiseMux(Size);
            bitwiseMuxInput1MultiBitAdder_5.ConnectInput1(bitwiseMuxXY_0.Output);
            bitwiseMuxInput1MultiBitAdder_5.ConnectInput2(bitwiseMuxXY_2.Output);
            bitwiseMuxInput1MultiBitAdder_5.ConnectControl(Control[2]);

            //bitwiseMuxInput2MultiBitAdder_6, gate 6
            //input1: bitwiseMuxOneMinusOne_1 , input2: bitwiseMuxMinusYMinusX_4 ,control: c[2], output: MultiBitAdder
            BitwiseMux bitwiseMuxInput2MultiBitAdder_6 = new BitwiseMux(Size);
            bitwiseMuxInput2MultiBitAdder_6.ConnectInput1(bitwiseMuxOneMinusOne_1.Output);
            bitwiseMuxInput2MultiBitAdder_6.ConnectInput2(bitwiseMuxMinusYMinusX_4.Output);
            bitwiseMuxInput2MultiBitAdder_6.ConnectControl(Control[2]);


            //MultiBitAdder
            //input1: bitwiseMuxInput1MultiBitAdder_5, input2: bitwiseMuxInput2MultiBitAdder_6,output: bitwiseMultiwayDemuxXY
            MultiBitAdder multiBitAdder = new MultiBitAdder(Size);
            multiBitAdder.ConnectInput1(bitwiseMuxInput1MultiBitAdder_5.Output);
            multiBitAdder.ConnectInput2(bitwiseMuxInput2MultiBitAdder_6.Output);

            for (int i = 0; i < 7; i++)
            { Main_ALU.ConnectInput(8 + i,multiBitAdder.Output); } //connect all the 01000 to 01110


            //01111 15 x&y
            BitwiseAndGate bitwiseAndGateXY = new BitwiseAndGate(Size);
            bitwiseAndGateXY.ConnectInput1(InputX);
            bitwiseAndGateXY.ConnectInput2(InputY);
            Main_ALU.ConnectInput(15, bitwiseAndGateXY.Output);

            //10000 and 10010 16 and 18
            MultiBitOrGate multiBitOrGate_1 = new MultiBitOrGate(Size);
            multiBitOrGate_1.ConnectInput(InputX);
            MultiBitOrGate multiBitOrGate_2 = new MultiBitOrGate(Size);
            multiBitOrGate_2.ConnectInput(InputY);
            AndGate andGateForMultibitOr = new AndGate();
            OrGate orGateForMultibitOr = new OrGate();
            andGateForMultibitOr.ConnectInput1(multiBitOrGate_1.Output);
            andGateForMultibitOr.ConnectInput2(multiBitOrGate_2.Output);
            orGateForMultibitOr.ConnectInput1(multiBitOrGate_1.Output);
            orGateForMultibitOr.ConnectInput2(multiBitOrGate_2.Output);

            WireSet outputAndGateWireSet = new WireSet(Size);
            outputAndGateWireSet[0].ConnectInput(andGateForMultibitOr.Output);

            Main_ALU.ConnectInput(16, outputAndGateWireSet);



            WireSet outputOrGate = new WireSet(Size);
            outputOrGate[0].ConnectInput(orGateForMultibitOr.Output);
            Main_ALU.ConnectInput(18, outputOrGate);

            //10001
            BitwiseOrGate bitwiseOrGateXY = new BitwiseOrGate(Size);
            bitwiseOrGateXY.ConnectInput1(InputX);
            bitwiseOrGateXY.ConnectInput2(InputY);
            Main_ALU.ConnectInput(17, bitwiseOrGateXY.Output);

            Output = Main_ALU.Output;

            //apply logicalnor (not on logical or) to get if the output is zero
            //MultiBitOrGate multiBitOrGateZeroChecker = new MultiBitOrGate(Size);
            //multiBitOrGateZeroChecker.ConnectInput(Output);
            NotGate notGateZeroChecker = new NotGate();
            notGateZeroChecker.ConnectInput(orGateForMultibitOr.Output);
            Zero.ConnectInput(notGateZeroChecker.Output);

            //zg=1 if the MSB is 1
            Negative.ConnectInput(Output[Size-1]);


        }

        public override bool TestGate()
        {
            //00000
            Control.SetValue(0);
            if (Output.GetValue() != 0)
                return false;
            
            //00001
            Control.SetValue(1);
            if (Output.GetValue() != 1)
                return false;

            //00010
            InputX.SetValue(5);
            Control.SetValue(2);
            if (Output.GetValue() != 5)
                return false;

            //00011
            InputY.SetValue(6);
            Control.SetValue(3);
            if (Output.GetValue() != 6)
                return false;

            //00100
            Control.SetValue(4);
            InputX.SetValue(7);
            for (int i = 0; i < 3; i++)
            {
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 4; i < 6; i++)
            {
                if (Output[i].Value != 1)
                    return false;
            }

            //00101 y!
            Control.SetValue(5);
            InputY.SetValue(3);
            for (int i = 0; i < 2; i++)
            {
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 3; i < 6; i++)
            {
                if (Output[i].Value != 1)
                    return false;
            }


            //00110 -x
            Control.SetValue(6);
            InputX.SetValue(7);
            if (Output[0].Value != 1)
                return false;
            for (int i = 1; i < 3; i++)
            {
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 4; i < 6; i++)
            {
                if (Output[i].Value != 1)
                    return false;
            }

            //00111 -y
            Control.SetValue(7);
            InputY.SetValue(7);
            if (Output[0].Value != 1)
                return false;
            for (int i = 1; i < 3; i++)
            {
                if (Output[i].Value != 0)
                    return false;
            }
            for (int i = 4; i < 6; i++)
            {
                if (Output[i].Value != 1)
                    return false;
            }

            //01000 x+1
            Control.SetValue(8);
            InputX.SetValue(23);
            if (Output.GetValue() != 24)
                return false;

            //01001 y+1
            Control.SetValue(9);
            InputY.SetValue(52);
            if (Output.GetValue() != 53)
                return false;

            //01010 x-1
            Control.SetValue(10);
            InputX.SetValue(33);
            if (Output.GetValue() != 32)
                return false;

            //01011 y-1
            Control.SetValue(11);
            InputY.SetValue(63);
            if (Output.GetValue() != 62)
                return false;

            //01100 x+y
            Control.SetValue(12);
            InputY.SetValue(33);
            InputX.SetValue(10);
            if (Output.GetValue() != 43)
                return false;

            //01101 x-y
            Control.SetValue(13);
            InputY.SetValue(37);
            InputX.SetValue(40);
            if (Output.GetValue() != 3)
                return false;

            //01110 y-x
            Control.SetValue(14);
            InputY.SetValue(37);
            InputX.SetValue(15);
            if (Output.GetValue() != 22)
                return false;

            //01111 x&y 15
            Control.SetValue(15);
            InputY.SetValue(3);
            InputX.SetValue(7);
            if (Output.GetValue() != 3)
                return false;

            //10001 x|y 17
            Control.SetValue(17);
            InputY.SetValue(8);
            InputX.SetValue(7);
            if (Output.GetValue() != 15)
                return false;
            InputX.SetValue(15);
            if (Output.GetValue() != 15)
                return false;

            //10000 x logicaland y 16
            Control.SetValue(16);
            InputX.SetValue(9);
            InputY.SetValue(0);
            if (Output.GetValue() != 0)
                return false;
            InputY.SetValue(33);
            if (Output.GetValue() != 1)
                return false;
            InputX.SetValue(0);
            InputY.SetValue(0);
            if (Output.GetValue() != 0)
                return false;

            //10010 x logicalor y 18
            Control.SetValue(18);
            InputX.SetValue(9);
            InputY.SetValue(0);
            if (Output.GetValue() != 1)
                return false;
            InputY.SetValue(33);
            if (Output.GetValue() != 1)
                return false;
            InputX.SetValue(0);
            InputY.SetValue(0);
            if (Output.GetValue() != 0)
                return false;
            return true;
        }
    }
}
