using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public class Register
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class CPU
    {
        private string[] instructions;

        public List<Register> Registers { get; private set; }

        public event Action<int> OnOutput = delegate { };

        public CPU(string[] instructions)
        {
            this.instructions = instructions;
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

        public int RunSingleInstruction(int instructionPointer)
        {
            if (instructionPointer > instructions.Count() - 1)
            {
                return -1;
            }

            var instructionParts = instructions[instructionPointer].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim(',').ToLower()).ToArray();

            if (instructionParts[0].StartsWith("//"))
                return instructionPointer;

            switch (instructionParts[0])
            {
                case "hlf":
                    GetRegister(instructionParts[1]).Value = GetRawOrRegisterValue(instructionParts[1]) / 2;

                    break;
                case "tpl":
                    GetRegister(instructionParts[1]).Value = GetRawOrRegisterValue(instructionParts[1]) * 3;

                    break;
                case "inc":
                case "dec":
                    GetRegister(instructionParts[1]).Value += instructionParts[0] == "inc" ? 1 : -1;

                    break;
                case "jmp":
                    int jumpChange = GetRawOrRegisterValue(instructionParts[1]);
                    instructionPointer += jumpChange - 1;

                    break;
                case "jie":
                    int registryEvenValue = GetRawOrRegisterValue(instructionParts[1]);
                    if (registryEvenValue % 2 == 0)
                    {
                        var pointerChange = GetRawOrRegisterValue(instructionParts[2]);
                        instructionPointer += pointerChange - 1;
                    }

                    break;
                case "jio":
                    int conditionValue = GetRawOrRegisterValue(instructionParts[1]);
                    if (conditionValue == 1)
                    {
                        var pointerChange = GetRawOrRegisterValue(instructionParts[2]);
                        instructionPointer += pointerChange - 1;
                    }

                    break;
                case "nop":
                    //Troloolol
                    break;
                default:
                    throw new Exception("Unsupported instruction: " + instructions[instructionPointer]);
            }

            return instructionPointer + 1;
        }

        public void RunToEnd()
        {
            for (int instructionPointer = 0; instructionPointer < instructions.Count(); instructionPointer++)
            {
                instructionPointer = RunSingleInstruction(instructionPointer) - 1;
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
