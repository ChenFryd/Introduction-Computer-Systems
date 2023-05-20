using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class represents a set of n wires (a cable)
    class WireSet
    {
        //Word size - number of bits in the register
        public int Size { get; private set; }
        
        public bool InputConected { get; private set; }

        //An indexer providing access to a single wire in the wireset
        public Wire this[int i]
        {
            get
            {
                return m_aWires[i];
            }
        }
        private Wire[] m_aWires;
        
        public WireSet(int iSize)
        {
            Size = iSize;
            InputConected = false;
            m_aWires = new Wire[iSize];
            for (int i = 0; i < m_aWires.Length; i++)
                m_aWires[i] = new Wire();
        }
        public override string ToString()
        {
            string s = "[";
            for (int i = m_aWires.Length - 1; i >= 0; i--)
                s += m_aWires[i].Value;
            s += "]";
            return s;
        }

        //Transform a positive integer value into binary and set the wires accordingly, with 0 being the LSB
        public void SetValue(int iValue)
        {
            for (int i = 0; i < Size; i++)
            {
                m_aWires[i].Value = (int)(iValue % 2);
                iValue = iValue / 2;
            }
        }

        //Transform the binary code into a positive integer
        public int GetValue()
        {
            int bin_output = 0;
            for (int i = 0; i < Size; i++)
            {
                if (this[i].Value == 1)
                    bin_output += (int)Math.Pow(2,i);
            }
            return bin_output;
        }

        //Transform an integer value into binary using 2`s complement and set the wires accordingly, with 0 being the LSB
        public void Set2sComplement(int iValue)
        {
            if (iValue >=0)
                SetValue(iValue);
            else //ivalue <0
            {
                iValue = (-1)*iValue; //to swap the negative number to positive
                this.SetValue(iValue); //to get a positive num to neg we need to swap all and add 1 bit
                int carry = 1, i=0;
                for (int j = 0; j < Size; j++)
                    m_aWires[j].Value = 1- m_aWires[j].Value;

                while (carry ==1)
                {
                    if(this[i].Value == 1)
                    {
                        
                        this[i].Value = 0;
                        i++;
                    }
                    else //this.value = 0
                    { 
                        this[i].Value =1;
                        carry = 0;
                    }
                }
            }
        }


        //Transform the binary code in 2`s complement into an integer
        public int Get2sComplement()
        {
            if (m_aWires[Size -1].Value == 0)
                return this.GetValue();
            else //MSB is 1
            { 
                int i=0, carryIN=0;
                do
                {
                    if(m_aWires[i].Value == 1) //we can minus the last digit
                    { 
                        carryIN = 0;
                        m_aWires[i].Value = 0;
                    }
                    else
                    {
                        m_aWires[i].Value = 1; //else take 1 from one digit forward
                        carryIN = 1;
                        i++;
                    }
                } while (carryIN == 1);
                for (int j = 0; j < Size; j++)
                {
                    m_aWires[j].Value = 1 - m_aWires[j].Value; // swap the values
                }
                int output = (this.GetValue() * -1);
                Set2sComplement(output);
                return output;
            }
        }

        public void ConnectInput(WireSet wIn)
        {
            if (InputConected)
                throw new InvalidOperationException("Cannot connect a wire to more than one inputs");
            if(wIn.Size != Size)
                throw new InvalidOperationException("Cannot connect two wiresets of different sizes.");
            for (int i = 0; i < m_aWires.Length; i++)
                m_aWires[i].ConnectInput(wIn[i]);

            InputConected = true;
            wIn.InputConected = true; //might need to delete this
            
        }

    }
}
