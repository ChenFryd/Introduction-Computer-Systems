using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is an example of a testing code that you should run for all the gates that you create

            //Create a gate
            AndGate and = new AndGate();
            Console.WriteLine(and + "");
            //Test that the unit testing works properly
            if (!and.TestGate())
                Console.WriteLine("bugbug");

            //Now we ruin the nand gates that are used in all other gates. The gate should not work properly after this.
            NAndGate.Corrupt = true;
            if (and.TestGate())
                Console.WriteLine("bugbug");



            BitwiseMultiwayMux m = new BitwiseMultiwayMux(7, 3);


            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
