using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {

             //Create a and gate
             AndGate and = new AndGate();
            if (!and.TestGate())
                Console.WriteLine("bug at Andgate");
            else { Console.WriteLine("And gate is clear"); }

            //Create a or gate
            AndGate or = new AndGate();
            if (!or.TestGate())
                Console.WriteLine("bug at Orgate");
            else { Console.WriteLine("Or gate is clear"); }

            XorGate xor = new XorGate();
            if (!xor.TestGate())
                Console.WriteLine("bug at Xorgate");
            else { Console.WriteLine("Xor gate is clear"); }

            MuxGate mux = new MuxGate();
            if (!mux.TestGate())
                Console.WriteLine("bug at muxgate");
            else { Console.WriteLine("Mux gate is clear"); }

            Demux demux = new Demux();
            if (!demux.TestGate())
                Console.WriteLine("bug at Demux");
            else { Console.WriteLine("Demux gate is clear"); }

            MultiBitAndGate multiand = new MultiBitAndGate(3);
            if (!multiand.TestGate())
                Console.WriteLine("bug at multiand");
            else { Console.WriteLine("multiand gate is clear"); }

            MultiBitOrGate multior = new MultiBitOrGate(3);
            if (!multior.TestGate())
                Console.WriteLine("bug at multior");
            else { Console.WriteLine("multior gate is clear"); }

            BitwiseAndGate bitwiseand = new BitwiseAndGate(3);
            if (!bitwiseand.TestGate())
                Console.WriteLine("bug at BitwiseAndGate");
            else { Console.WriteLine("BitwiseAndGate gate is clear"); }

            BitwiseNotGate bitwisenot = new BitwiseNotGate(3);
            if (!bitwisenot.TestGate())
                Console.WriteLine("bug at BitwiseNotGate");
            else { Console.WriteLine("BitwiseNotGate gate is clear"); }

            BitwiseOrGate bitwiseor = new BitwiseOrGate(3);
            if (!bitwiseor.TestGate())
                Console.WriteLine("bug at bitwiseor");
            else { Console.WriteLine("bitwiseor gate is clear"); }


            BitwiseMux bitwisemux = new BitwiseMux(3);
            if (!bitwisemux.TestGate())
                Console.WriteLine("bug at BitwiseMux");
            else { Console.WriteLine("BitwiseMux gate is clear"); }

            BitwiseDemux bitwisedemux = new BitwiseDemux(2);
            if (!bitwisedemux.TestGate())
                Console.WriteLine("bug at BitwiseDemux");
            else { Console.WriteLine("BitwiseDemux gate is clear"); }
            
            BitwiseMultiwayMux bitwiseMultiwayMux = new BitwiseMultiwayMux(7, 4);
            if (!bitwiseMultiwayMux.TestGate())
                Console.WriteLine("bug at bitwiseMultiwayMux");
            else
            { Console.WriteLine("bitwiseMultiwayMux gate is clear"); }

            BitwiseMultiwayDemux BitwiseMultiwayDemux = new BitwiseMultiwayDemux(7, 4);
            if (!BitwiseMultiwayDemux.TestGate())
                Console.WriteLine("bug at BitwiseMultiwayDemux");
            else
            { Console.WriteLine("BitwiseMultiwayDemux gate is clear"); }

            HalfAdder halfAdder = new HalfAdder();
            if (!halfAdder.TestGate())
                Console.WriteLine("bug at HalfAdder");
            else
            { Console.WriteLine("HalfAdder gate is clear"); }

            FullAdder fullAdder = new FullAdder();
            if (!fullAdder.TestGate())
                Console.WriteLine("bug at fullAdder");
            else
            { Console.WriteLine("fullAdder gate is clear"); }

            MultiBitAdder multiBitAdder = new MultiBitAdder(4);
            if (!multiBitAdder.TestGate())
                Console.WriteLine("bug at multiBitAdder");
            else
            { Console.WriteLine("multiBitAddergate is clear"); }

            WireSet wireSet = new WireSet(4);
            wireSet.Set2sComplement(-3);
            if (wireSet[0].Value != 1)
                Console.WriteLine("wireSet has wrong 1 bit");
            else if (wireSet[1].Value != 0)
                Console.WriteLine("wireSet has wrong 2 bit");
            else if (wireSet[2].Value != 1)
                Console.WriteLine("wireSet has wrong 3 bit");
            else if (wireSet[3].Value != 1)
                Console.WriteLine("wireSet has wrong 4 bit");
            else
                Console.WriteLine("wireset has 2s complement is clear");
            if(wireSet.Get2sComplement() != -3)
                Console.WriteLine("FALSE");
            WireSet wireSet2 = new WireSet(8);
            wireSet2.Set2sComplement(-120);
            Console.WriteLine(wireSet2.Get2sComplement());
            wireSet2.Set2sComplement(56);
            Console.WriteLine(wireSet2.Get2sComplement());
            wireSet2.Set2sComplement(89);
            Console.WriteLine(wireSet2.Get2sComplement());

            ALU aLU = new ALU(6);
            if (!aLU.TestGate())
                Console.WriteLine("bug at ALU");
            else
            { Console.WriteLine("alu is clear"); }

            //Now we ruin the nand gates that are used in all other gates. The gate should not work properly after this.
            NAndGate.Corrupt = true;
            Console.WriteLine("\n nand is now corrupt");
            if (and.TestGate())
                 Console.WriteLine("corrupt bug at and gate");
            if (or.TestGate())
                Console.WriteLine("corrupt bug at or gate");
            if (xor.TestGate())
                Console.WriteLine("corrupt bug at xor gate");
            if (mux.TestGate())
                Console.WriteLine("corrupt bug at mux gate");
            if (demux.TestGate())
                Console.WriteLine("corrupt bug at demux gate");
            if (multiand.TestGate())
                Console.WriteLine("corrupt bug at multiand gate");
            if (multior.TestGate())
                Console.WriteLine("corrupt bug at multior gate");
            if (bitwiseand.TestGate())
                Console.WriteLine("corrupt bug at bitwiseand gate");
            if (bitwiseor.TestGate())
                Console.WriteLine("corrupt bug at bitwiseor gate");
            if (bitwisemux.TestGate())
                Console.WriteLine("corrupt bug at bitwisemux gate");
            if (bitwisedemux.TestGate())
                Console.WriteLine("corrupt bug at bitwisedemux gate");
            //if (bitwiseMultiwayMux.TestGate())
            //    Console.WriteLine("corrupt bug at bitwiseMultiwayMux gate");
            //if (BitwiseMultiwayDemux.TestGate())
            //    Console.WriteLine("corrupt bug at bitwiseMultiwayMux gate");
            if (halfAdder.TestGate())
                Console.WriteLine("corrupt bug at halfAdder gate");
            if (halfAdder.TestGate())
                Console.WriteLine("corrupt bug at fullAdder gate");
            if (multiBitAdder.TestGate())
                Console.WriteLine("corrupt bug at multiBitAdder gate");
            
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
