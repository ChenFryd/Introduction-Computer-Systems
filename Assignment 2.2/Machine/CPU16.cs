using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms.VisualStyles;
using SimpleComponents;

namespace Machine
{
    public class CPU16 
    {
         public const int J0 = 0, J1 = 1, J2 = 2, D0 = 3, D1 = 4, D2 = 5, C0 = 6, C1 = 7, C2 = 8, C3 = 9, C4 = 10, A = 11, X0 = 12, X1 = 13, X2 = 14, Type = 15;

        public int Size { get; private set; }

        public WireSet Instruction { get; private set; }
        public WireSet MemoryInput { get; private set; }
        public Wire Reset { get; private set; }

        public WireSet MemoryOutput { get; private set; }
        public Wire MemoryWrite { get; private set; }
        public WireSet MemoryAddress { get; private set; }
        public WireSet InstructionAddress { get; private set; }

        private ALU m_gALU;
        private Counter m_rPC;
        private MultiBitRegister m_rA, m_rD;
        private BitwiseMux m_gAMux, m_gMAMux;


        public CPU16()
        {
            Size = 16;

            Instruction = new WireSet(Size);
            MemoryInput = new WireSet(Size);
            MemoryOutput = new WireSet(Size);
            MemoryAddress = new WireSet(Size);
            InstructionAddress = new WireSet(Size);
            MemoryWrite = new Wire();
            Reset = new Wire();

            m_gALU = new ALU(Size);
            m_rPC = new Counter(Size);
            m_rA = new MultiBitRegister(Size);
            m_rD = new MultiBitRegister(Size);

            m_gAMux = new BitwiseMux(Size);
            m_gMAMux = new BitwiseMux(Size);

            m_gAMux.ConnectInput1(Instruction);
            m_gAMux.ConnectInput2(m_gALU.Output);

            m_rA.ConnectInput(m_gAMux.Output);

            m_gMAMux.ConnectInput1(m_rA.Output);
            m_gMAMux.ConnectInput2(MemoryInput);
            m_gALU.InputY.ConnectInput(m_gMAMux.Output);

            m_gALU.InputX.ConnectInput(m_rD.Output);

            m_rD.ConnectInput(m_gALU.Output);

            MemoryOutput.ConnectInput(m_gALU.Output);
            MemoryAddress.ConnectInput(m_rA.Output);

            InstructionAddress.ConnectInput(m_rPC.Output);
            m_rPC.ConnectInput(m_rA.Output);
            m_rPC.ConnectReset(Reset);

            ConnectControls();
        }

        //Add gates for control implementation here
        private WireSet m_wireSet_ALU;
        private AndGate m_andGate_D_register;
        private OrGate m_orGate_A_register;
        private NotGate m_notGate_A_register_msb;
        private AndGate m_andGate_a_register;
        Wire wire0;
        Wire wire1;
        private AndGate andGateRW;
        private MuxGate muxGate0;
        private MuxGate muxGate1;
        private MuxGate muxGate2;
        private MuxGate muxGate3;
        private MuxGate muxGate4;
        private MuxGate muxGate5;
        private MuxGate muxGate6;

        private OrGate orGateBigger1;
        private NotGate notGateBigger2;
        private NotGate notGateBiggerEqual3;
        private OrGate orGateBiggerEqual4;
        private NotGate notGateLess5;
        private NotGate notGateNotEqual6;

        private AndGate andGateACommend;
        private void ConnectControls()
        {
            //1. connect control of mux 1 (selects entrance to register A)
            m_gAMux.ConnectControl(Instruction[15]);

            //2. connect control to mux 2 (selects A or M entrance to the ALU)
            m_gMAMux.ConnectControl(Instruction[11]);

            //3. consider all instruction bits only if C type instruction (MSB of instruction is 1)
            

            //4. connect ALU control bits
            m_wireSet_ALU = new WireSet(5); //initate the wireset to connect to the control of the ALU
            for (int i = 0; i < 5; i++)
            {
                m_wireSet_ALU[0+i].ConnectInput(Instruction[6 + i]);
            }
            m_gALU.Control.ConnectInput(m_wireSet_ALU);

            //5. connect control to register D (very simple)
            m_andGate_D_register = new AndGate();
            m_andGate_D_register.ConnectInput1(Instruction[15]); //connect the msb
            m_andGate_D_register.ConnectInput2(Instruction[4]) ; //not sure
            m_rD.Load.ConnectInput(m_andGate_D_register.Output);

            //6. connect control to register A (a bit more complicated)
            m_orGate_A_register = new OrGate();
            m_andGate_a_register = new AndGate();

            //if the msb=1 and and Destination is A
            m_andGate_a_register.ConnectInput1(Instruction[5]); //5 is the d=A
            m_andGate_a_register.ConnectInput2(Instruction[15]); //connect the msb
            

            //msb=0
            m_notGate_A_register_msb = new NotGate();
            m_notGate_A_register_msb.ConnectInput(Instruction[15]); //msb

            //the or gate between the two
            m_orGate_A_register.ConnectInput1(m_andGate_a_register.Output);
            m_orGate_A_register.ConnectInput2(m_notGate_A_register_msb.Output);
            m_rA.Load.ConnectInput(m_orGate_A_register.Output);

            //7. connect control to MemoryWrite
            wire0 = new Wire();
            wire0.Value = 0;
            wire1 = new Wire();
            wire1.Value = 1;
            
            andGateRW = new AndGate();
            andGateRW.ConnectInput1(Instruction[15]); //msb
            andGateRW.ConnectInput2(Instruction[3]); //d0
            MemoryWrite.ConnectInput(andGateRW.Output);

            //8. create inputs for jump mux
            muxGate0 = new MuxGate();
            muxGate1 = new MuxGate();
            muxGate2 = new MuxGate();
            muxGate3 = new MuxGate();
            muxGate4 = new MuxGate();
            muxGate5 = new MuxGate();
            muxGate6 = new MuxGate();

            orGateBigger1 = new OrGate();
            notGateBigger2 = new NotGate();
            notGateBiggerEqual3 = new NotGate();
            orGateBiggerEqual4 = new OrGate();
            notGateLess5 = new NotGate();
            notGateNotEqual6 = new NotGate();
            

            //9. connect jump mux (this is the most complicated part)
            muxGate0.ConnectInput1(muxGate1.Output);
            muxGate0.ConnectInput2(muxGate2.Output);
            muxGate0.ConnectControl(Instruction[2]);
            
            //second layer
            muxGate1.ConnectInput1(muxGate3.Output);
            muxGate1.ConnectInput2(muxGate4.Output);
            muxGate1.ConnectControl(Instruction[1]);

            muxGate2.ConnectInput1(muxGate5.Output);
            muxGate2.ConnectInput2(muxGate6.Output);
            muxGate2.ConnectControl(Instruction[1]);

            //third layer
            muxGate3.ConnectInput1(wire0);
            orGateBigger1.ConnectInput1(m_gALU.Negative);
            orGateBigger1.ConnectInput2(m_gALU.Zero);
            notGateBigger2.ConnectInput(orGateBigger1.Output);
            muxGate3.ConnectInput2(notGateBigger2.Output);
            muxGate3.ConnectControl(Instruction[0]);

            muxGate4.ConnectInput1(m_gALU.Zero);
            orGateBiggerEqual4.ConnectInput1(m_gALU.Zero);
            notGateBiggerEqual3.ConnectInput(m_gALU.Negative);
            orGateBiggerEqual4.ConnectInput2(notGateBiggerEqual3.Output);
            muxGate4.ConnectInput2(orGateBiggerEqual4.Output);
            muxGate4.ConnectControl(Instruction[0]);

            notGateLess5.ConnectInput(orGateBiggerEqual4.Output);
            muxGate5.ConnectInput1(notGateLess5.Output);
            notGateNotEqual6.ConnectInput(m_gALU.Zero);
            muxGate5.ConnectInput2(notGateNotEqual6.Output);
            muxGate5.ConnectControl(Instruction[0]);

            muxGate6.ConnectInput1(orGateBigger1.Output);
            muxGate6.ConnectInput2(wire1);
            muxGate6.ConnectControl(Instruction[0]);

            andGateACommend = new AndGate();
            andGateACommend.ConnectInput1(Instruction[15]);
            andGateACommend.ConnectInput2(muxGate0.Output);

            //10. connect PC load control
            m_rPC.Load.ConnectInput(andGateACommend.Output); 
        }
        

        public override string ToString()
        {
            return "A=" + m_rA + ", D=" + m_rD + ", PC=" + m_rPC + ",Ins=" + Instruction;
        }

        private string GetInstructionString()
        {
            if (Instruction[Type].Value == 0)
                return "@" + Instruction.GetValue();
            return Instruction[Type].Value + "XXX " +
               "a" + Instruction[A] + " " +
               "c" + Instruction[C4] + Instruction[C3] + Instruction[C2] + Instruction[C1] + Instruction[C0] + " " +
               "d" + Instruction[D2] + Instruction[D1] + Instruction[D0] + " " +
               "j" + Instruction[J2] + Instruction[J1] + Instruction[J0];
        }

        public void PrintState()
        {
            Console.WriteLine("CPU state:");
            Console.WriteLine("PC=" + m_rPC + "=" + m_rPC.Output.GetValue());
            Console.WriteLine("A=" + m_rA + "=" + m_rA.Output.GetValue());
            Console.WriteLine("D=" + m_rD + "=" + m_rD.Output.GetValue());
            Console.WriteLine("Ins=" + GetInstructionString());
            Console.WriteLine("ALU=" + m_gALU);
            Console.WriteLine("inM=" + MemoryInput);
            Console.WriteLine("outM=" + MemoryOutput);
            Console.WriteLine("addM=" + MemoryAddress);
        }
    }
}
