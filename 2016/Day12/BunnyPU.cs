using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class Register
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class BunnyPU
    {
        public List<Register> Registers { get; private set; }

        public BunnyPU()
        {
            Registers = new List<Register>();
        }

        public Register GetRegister(string name)
        {
            var register = Registers.FirstOrDefault(r => r.Name == name);
            if (register == null)
            {
                register = new Register { Name = name };
                Registers.Add(register);
            }

            return register;
        }

        public void ProcessInstructions(string[] instructions)
        {
            for (int instructionPointer = 0; instructionPointer < instructions.Count(); instructionPointer++)
            {
                var instructionParts = instructions[instructionPointer].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToLower()).ToArray();

                if (instructionParts[0].StartsWith("//"))
                    continue;

                switch (instructionParts[0])
                {
                    case "cpy":
                        GetRegister(instructionParts[2]).Value = GetRawOrRegisterValue(instructionParts[1]);

                        break;
                    case "inc":
                    case "dec":
                        GetRegister(instructionParts[1]).Value += instructionParts[0] == "inc" ? 1 : -1;

                        break;
                    case "jnz":
                        int conditionValue = GetRawOrRegisterValue(instructionParts[1]);
                        if (conditionValue != 0)
                        {
                            var pointerChange = GetRawOrRegisterValue(instructionParts[2]);
                            instructionPointer += pointerChange - 1;
                        }

                        break;
                    case "tgl":
                        int instructionToToggle = instructionPointer + GetRawOrRegisterValue(instructionParts[1]);
                        if (instructionToToggle > 0 && instructionToToggle < instructions.Count())
                        {
                            var toggleRow = instructions[instructionToToggle].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToLower()).ToArray();

                            if (toggleRow[0].StartsWith("//"))
                            {
                                toggleRow[0] = toggleRow[0].Substring(2);
                            }

                            if (toggleRow.Length == 3)
                            {
                                if (toggleRow[0] == "jnz")
                                {
                                    toggleRow[0] = "cpy";
                                }
                                else
                                {
                                    toggleRow[0] = "jnz";
                                }

                                int crap;
                                if (toggleRow[0] == "cpy" && int.TryParse(toggleRow[2], out crap))
                                {
                                    toggleRow[0] = "//" + toggleRow[0];
                                }
                            }
                            else
                            {
                                if (toggleRow[0] == "inc")
                                {
                                    toggleRow[0] = "dec";
                                }
                                else
                                {
                                    toggleRow[0] = "inc";
                                }

                                int crap;
                                if (int.TryParse(toggleRow[1], out crap))
                                {
                                    toggleRow[0] = "//" + toggleRow[0];
                                }
                            }

                            instructions[instructionToToggle] = string.Join(" ", toggleRow);
                        }
                        break;
                    case "nop":
                        //Troloolol
                        break;
                    default:
                        throw new Exception("Unsupported instruction: " + instructions[instructionPointer]);
                }
            }
        }

        private int GetRawOrRegisterValue(string x)
        {
            int value;
            if (!int.TryParse(x, out value))
            {
                value = GetRegister(x).Value;
            }

            return value;
        }
    }
}
