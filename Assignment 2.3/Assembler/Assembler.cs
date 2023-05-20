using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assembler
{
    public class Assembler
    {
        private const int WORD_SIZE = 16;

        private Dictionary<string, int[]> m_dControl, m_dJmp, m_dDest; //these dictionaries map command mnemonics to machine code - they are initialized at the bottom of the class
        private Dictionary<string, int> m_dSymbolMap;
        //more data structures here (symbol map, ...)

        public Assembler()
        {
            InitCommandDictionaries();
        }

        //this method is called from the outside to run the assembler translation
        public void TranslateAssemblyFile(string sInputAssemblyFile, string sOutputMachineCodeFile)
        {
            //read the raw input, including comments, errors, ...
            StreamReader sr = new StreamReader(sInputAssemblyFile);
            List<string> lLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lLines.Add(sr.ReadLine());
            }
            sr.Close();
            //translate to machine code
            List<string> lTranslated = TranslateAssemblyFile(lLines);
            //write the output to the machine code file
            StreamWriter sw = new StreamWriter(sOutputMachineCodeFile);
            foreach (string sLine in lTranslated)
                sw.WriteLine(sLine);
            sw.Close();
        }

        //translate assembly into machine code
        private List<string> TranslateAssemblyFile(List<string> lLines)
        {
            //implementation order:
            //first, implement "TranslateAssemblyToMachineCode", and check if the examples "Add", "MaxL" are translated correctly.
            //next, implement "CreateSymbolTable", and modify the method "TranslateAssemblyToMachineCode" so it will support symbols (translating symbols to numbers). check this on the examples that don't contain macros
            //the last thing you need to do, is to implement "ExpendMacro", and test it on the example: "SquareMacro.asm".
            //init data structures here 

            //expand the macros
            List<string> lAfterMacroExpansion = ExpendMacros(lLines);

            //first pass - create symbol table and remove lable lines
            CreateSymbolTable(lAfterMacroExpansion);

            //second pass - replace symbols with numbers, and translate to machine code
            List<string> lAfterTranslation = TranslateAssemblyToMachineCode(lAfterMacroExpansion);
            return lAfterTranslation;
        }

        
        //first pass - replace all macros with real assembly
        private List<string> ExpendMacros(List<string> lLines)
        {
            //You do not need to change this function, you only need to implement the "ExapndMacro" method (that gets a single line == string)
            List<string> lAfterExpansion = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                //remove all redudant characters
                string sLine = CleanWhiteSpacesAndComments(lLines[i]);
                if (sLine == "")
                    continue;
                //if the line contains a macro, expand it, otherwise the line remains the same
                List<string> lExpanded = ExapndMacro(sLine);
                //we may get multiple lines from a macro expansion
                foreach (string sExpanded in lExpanded)
                {
                    lAfterExpansion.Add(sExpanded);
                }
            }
            return lAfterExpansion;
        }

        //expand a single macro line
        private List<string> ExapndMacro(string sLine)
        {
            List<string> lExpanded = new List<string>();

            if (IsCCommand(sLine))
            {
                string sDest, sCompute, sJmp;
                GetCommandParts(sLine, out sDest, out sCompute, out sJmp);
                //your code here - check for indirect addessing and for jmp shortcuts
                //read the word file to see all the macros you need to support
                if (sJmp != "")
                {
                    if (sJmp.Contains(':'))
                    {
                        lExpanded.Add($"@{sJmp.Substring(sJmp.IndexOf(':') + 1)}");
                        lExpanded.Add($"{sCompute};{sJmp.Substring(0, sJmp.IndexOf(':'))}");
                    }
                    else
                        lExpanded.Add(sLine);
                    return lExpanded;
                }
                if (sCompute.Contains("++"))
                {
                    if (sDest == "D" || sDest == "A" || sDest == "M")
                    {
                        lExpanded.Add($"{sCompute}={sCompute}+1");
                    }
                    else
                    {
                        lExpanded.Add($"@{sCompute[0]}");
                        lExpanded.Add($"M=M+1");
                    }
                    return lExpanded;
                }
                if (sCompute.Contains("--"))
                {
                    if (sDest == "D" || sDest == "A" || sDest == "M" )
                    {
                        lExpanded.Add($"{sCompute}={sCompute}-1");
                    }
                    else
                    {
                        lExpanded.Add($"@{sCompute[0]}");
                        lExpanded.Add($"M=M-1");
                    }
                    return lExpanded;
                }
                if (!(sDest == "D" || sDest == "A" || sDest == "M"))
                { //label=
                    if (!(sCompute == "D" || sCompute == "A" || sCompute == "M"))
                    {
                        Regex r = new Regex("^[0-9]+$");
                        Regex c = new Regex("[a-zA-Z]+");
                        if (!c.IsMatch(sCompute)) //label = number
                        { //label =number
                            lExpanded.Add($"@{sCompute}");
                            lExpanded.Add($"D=A");
                            lExpanded.Add($"@{sDest}");
                            lExpanded.Add($"M=D");
                        }
                        else//label=label
                        {
                            lExpanded.Add($"@{sCompute}");
                            lExpanded.Add($"D=M");
                            lExpanded.Add($"@{sDest}");
                            lExpanded.Add($"M=D");
                        }
                    }
                    else
                    { //label=comp
                        if (sCompute == "A") { //label=A
                            lExpanded.Add("D=A");
                            lExpanded.Add($"@{sDest}");
                            lExpanded.Add("M=D");
                        }
                        else
                        {//label =D/M
                            lExpanded.Add($"@{sDest}");
                            lExpanded.Add($"M={sCompute}");
                        }
                        
                        
                        
                        return lExpanded;
                    }

                    
                }//A=p1
                else //Dest=A/D/M
                {
                    Regex r = new Regex("[0-9]+");
                    Regex c = new Regex("[a-zA-Z]+");
                    if (!(sCompute.Contains("D") || sCompute.Contains("A") || sCompute.Contains("M")))
                    { //A/D/M=label/Number
                        if (c.IsMatch(sCompute)) {//ADM=label
                            if (sDest == "A")
                                lExpanded.Add($"@{sCompute}");
                            if (sDest == "D")
                            {
                                lExpanded.Add($"@{sCompute}");
                                lExpanded.Add($"{sDest}=M");
                            }
                            if (sDest == "M") //M=label
                            {
                                lExpanded.Add("D=A");
                                lExpanded.Add($"@{sCompute}");
                                lExpanded.Add("A=D");
                                lExpanded.Add("M=M");
                            }
                        }
                        else {//ADM=number
                            if(sDest == "A") //A=number
                                lExpanded.Add($"@{sCompute}");
                            if (sDest == "D")//D=NUmber
                            {
                                lExpanded.Add($"@{sCompute}");
                                lExpanded.Add($"D=A");
                            }
                            if (sDest == "M") //M=Number
                            {
                                ; //not supported
                            }
                        }
                        
                    }
                    else { //ADM = ADM
                        lExpanded.Add(sLine);
                    }
                    return lExpanded;
                }

            }
            if (lExpanded.Count == 0)
                lExpanded.Add(sLine);
            return lExpanded;
        }
        
        //second pass - record all symbols - labels and variables
        private void CreateSymbolTable(List<string> lLines)
        {
            string sLine = "";
            m_dSymbolMap = new Dictionary<string, int>();
            int varCounter = 0, linePointerCounter=0;
           
            for (int i = 0; i < lLines.Count; i++)
            {
                sLine = lLines[i];
                if (IsLabelLine(sLine))
                {
                    //record label in symbol table
                    //do not add the label line to the result
                    string keyStringLine = sLine.Substring(1, sLine.Length - 2);
                    int intOutValue = 0;
                    if (m_dSymbolMap.TryGetValue(keyStringLine,out intOutValue))
                    {
                        if (intOutValue == -1)
                            m_dSymbolMap[keyStringLine] = lLines.IndexOf(sLine) - linePointerCounter;
                        else
                            throw new AssemblerException(i, lLines[i], "the line pointer already exists");
                    }
                    else
                    {
                        m_dSymbolMap.Add(sLine.Substring(1, sLine.Length - 2), lLines.IndexOf(sLine) - linePointerCounter);
                    }
                    linePointerCounter++;
                
                   
                }
                else if (IsACommand(sLine))
                {
                    //may contain a variable - if so, record it to the symbol table (if it doesn't exist there yet...)
                    Regex r = new Regex("^[0-9]+$");
                    Regex c = new Regex("[a-zA-Z]+");
                    if (!c.IsMatch(sLine.Substring(1))) //@number
                    {
                        if (sLine[1] == '-')//cant create a minus number
                            throw new AssemblerException(i, lLines[i], "Number can't be minus");
                    }
                    else if(r.IsMatch(sLine.Substring(1,1)))//cant create a label with a number at the start
                        throw new AssemblerException(i, lLines[i], "can't create that label");
                    else {
                        if (!m_dSymbolMap.ContainsKey(sLine.Substring(1)))
                        {
                            m_dSymbolMap.Add(sLine.Substring(1),-1);
                            
                        }
                    }
                        
                }
                else if (IsCCommand(sLine))
                {
                    //do nothing here
                    ;
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
            
            }
            foreach (string strKey in m_dSymbolMap.Keys.ToList())
            {
                if (m_dSymbolMap[strKey] == -1)
                {
                    m_dSymbolMap[strKey] = 16 + varCounter;
                    varCounter += 1;
                }
            }

        }
        
        //third pass - translate lines into machine code, replacing symbols with numbers
        private List<string> TranslateAssemblyToMachineCode(List<string> lLines)
        {
            string sLine = "";
            List<string> lAfterPass = new List<string>();
            CreateSymbolTable(lLines);
            for (int i = 0; i < lLines.Count; i++)
            {
                sLine = lLines[i];
                if (sLine[0] == '(' && sLine[sLine.Length - 1] == ')')
                    continue;
                if (IsACommand(sLine))
                {
                    //translate an A command into a sequence of bits
                    if (m_dSymbolMap.ContainsKey(sLine.Substring(1, sLine.Length - 1)))
                    {
                        string strKey = sLine.Substring(1, sLine.Length - 1);
                        int valueFromKey = m_dSymbolMap[strKey];
                        string stringBinary = ToBinary(valueFromKey);
                        lAfterPass.Add(stringBinary);
                    }
                    else
                    {
                        if (Int64.Parse(sLine.Substring(1)) > 65535)
                        {
                            throw new AssemblerException(i, sLine, "number is too high");
                        }
                        else
                        {
                            lAfterPass.Add(ToBinary(Int32.Parse(sLine.Substring(1))));
                        }
                    }
                }
                else if (IsCCommand(sLine))
                {
                    string sDest, sControl, sJmp;
                    GetCommandParts(sLine, out sDest, out sControl, out sJmp);
                    //translate an C command into a sequence of bits
                    //take a look at the dictionaries m_dControl, m_dJmp, and where they are initialized (InitCommandDictionaries), to understand how to you them here
                    if (!m_dDest.ContainsKey(sDest) || !m_dJmp.ContainsKey(sJmp) || !m_dControl.ContainsKey(sControl))
                        throw new AssemblerException(i, sLine, "the line is wrong");
                    string sLineOutput = "1000"+ ToString(m_dControl[sControl]) + ToString(m_dDest[sDest]) + ToString(m_dJmp[sJmp]);
                    lAfterPass.Add(sLineOutput);
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
            }
            return lAfterPass;
        }

        //helper functions for translating numbers or bits into strings
        private string ToString(int[] aBits)
        {
            string sBinary = "";
            for (int i = 0; i < aBits.Length; i++)
                sBinary = aBits[i]+ sBinary;
            return sBinary;
        }

        private string ToBinary(int x)
        {
            string sBinary = "";
            for (int i = 0; i < WORD_SIZE; i++)
            {
                sBinary = (x % 2) + sBinary;
                x = x / 2;
            }
            return sBinary;
        }


        //helper function for splitting the various fields of a C command
        private void GetCommandParts(string sLine, out string sDest, out string sControl, out string sJmp)
        {
            if (sLine.Contains('='))
            {
                int idx = sLine.IndexOf('=');
                sDest = sLine.Substring(0, idx);
                sLine = sLine.Substring(idx + 1);
            }
            else
                sDest = "";
            if (sLine.Contains(';'))
            {
                int idx = sLine.IndexOf(';');
                sControl = sLine.Substring(0, idx);
                sJmp = sLine.Substring(idx + 1);

            }
            else
            {
                sControl = sLine;
                sJmp = "";
            }
        }

        private bool IsCCommand(string sLine)
        {
            return !IsLabelLine(sLine) && sLine[0] != '@';
        }

        private bool IsACommand(string sLine)
        {
            return sLine[0] == '@';
        }

        private bool IsLabelLine(string sLine)
        {
            if (sLine.StartsWith("(") && sLine.EndsWith(")"))
                return true;
            return false;
        }

        private string CleanWhiteSpacesAndComments(string sDirty)
        {
            string sClean = "";
            for (int i = 0 ; i < sDirty.Length ; i++)
            {
                char c = sDirty[i];
                if (c == '/' && i < sDirty.Length - 1 && sDirty[i + 1] == '/') // this is a comment
                    return sClean;
                if (c > ' ' && c <= '~')//ignore white spaces
                    sClean += c;
            }
            return sClean;
        }

        public int[] GetArray(params int[] l)
        {
            int[] a = new int[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[l.Length - i - 1] = l[i];
            return a;
        }
        private void InitCommandDictionaries()
        {
            m_dControl = new Dictionary<string, int[]>();


            m_dControl["0"] = GetArray(0, 0, 0, 0, 0, 0);
            m_dControl["1"] = GetArray(0, 0, 0, 0, 0, 1);
            m_dControl["D"] = GetArray(0, 0, 0, 0, 1, 0);
            m_dControl["A"] = GetArray(0, 0, 0, 0, 1, 1);
            m_dControl["!D"] = GetArray(0, 0, 0, 1, 0, 0);
            m_dControl["!A"] = GetArray(0, 0, 0, 1, 0, 1);
            m_dControl["-D"] = GetArray(0, 0, 0, 1, 1, 0);
            m_dControl["-A"] = GetArray(0, 0, 0, 1, 1, 1);
            m_dControl["D+1"] = GetArray(0, 0, 1, 0, 0, 0);
            m_dControl["A+1"] = GetArray(0, 0, 1, 0, 0, 1);
            m_dControl["D-1"] = GetArray(0, 0, 1, 0, 1, 0);
            m_dControl["A-1"] = GetArray(0, 0, 1, 0, 1, 1);
            m_dControl["A+D"] = GetArray(0, 0, 1, 1, 0, 0);
            m_dControl["D+A"] = GetArray(0, 0, 1, 1, 0, 0);
            m_dControl["D-A"] = GetArray(0, 0, 1, 1, 0, 1);
            m_dControl["A-D"] = GetArray(0, 0, 1, 1, 1, 0);
            m_dControl["A^D"] = GetArray(0, 0, 1, 1, 1, 1);
            m_dControl["A&D"] = GetArray(0, 1, 0, 0, 0, 0);
            m_dControl["AvD"] = GetArray(0, 1, 0, 0, 0, 1);
            m_dControl["A|D"] = GetArray(0, 1, 0, 0, 1, 0);

            m_dControl["M"] = GetArray(1, 0, 0, 0, 1, 1);
            m_dControl["!M"] = GetArray(1, 0, 0, 1, 0, 1);
            m_dControl["-M"] = GetArray(1, 0, 0, 1, 1, 1);
            m_dControl["M+1"] = GetArray(1, 0, 1, 0, 0, 1);
            m_dControl["M-1"] = GetArray(1, 0, 1, 0, 1, 1);
            m_dControl["M+D"] = GetArray(1, 0, 1, 1, 0, 0);
            m_dControl["D+M"] = GetArray(1, 0, 1, 1, 0, 0);
            m_dControl["D-M"] = GetArray(1, 0, 1, 1, 0, 1);
            m_dControl["M-D"] = GetArray(1, 0, 1, 1, 1, 0);
            m_dControl["M^D"] = GetArray(1, 0, 1, 1, 1, 1);
            m_dControl["M&D"] = GetArray(1, 1, 0, 0, 0, 0);
            m_dControl["MvD"] = GetArray(1, 1, 0, 0, 0, 1);
            m_dControl["M|D"] = GetArray(1, 1, 0, 0, 1, 0);



            m_dDest = new Dictionary<string, int[]>();
            m_dDest[""] = GetArray(0, 0, 0);
            m_dDest["M"] = GetArray(0, 0, 1);
            m_dDest["D"] = GetArray(0, 1, 0);
            m_dDest["A"] = GetArray(1, 0, 0);
            m_dDest["DM"] = GetArray(0, 1, 1);
            m_dDest["AM"] = GetArray(1, 0, 1);
            m_dDest["AD"] = GetArray(1, 1, 0);
            m_dDest["ADM"] = GetArray(1, 1, 1);


            m_dJmp = new Dictionary<string, int[]>();

            m_dJmp[""] = GetArray(0, 0, 0);
            m_dJmp["JGT"] = GetArray(0, 0, 1);
            m_dJmp["JEQ"] = GetArray(0, 1, 0);
            m_dJmp["JGE"] = GetArray(0, 1, 1);
            m_dJmp["JLT"] = GetArray(1, 0, 0);
            m_dJmp["JNE"] = GetArray(1, 0, 1);
            m_dJmp["JLE"] = GetArray(1, 1, 0);
            m_dJmp["JMP"] = GetArray(1, 1, 1);
        }
    }
}
