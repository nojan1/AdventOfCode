using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Register
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    class BunnyPU
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

                switch (instructionParts[0])
                {
                    case "cpy":
                        int value;
                        if (int.TryParse(instructionParts[1], out value))
                        {
                            GetRegister(instructionParts[2]).Value = value;
                        }
                        else
                        {
                            GetRegister(instructionParts[2]).Value = GetRegister(instructionParts[1]).Value;
                        }

                        break;
                    case "inc":
                    case "dec":
                        GetRegister(instructionParts[1]).Value += instructionParts[0] == "inc" ? 1 : -1;

                        break;
                    case "jnz":
                        int conditionValue = 0;
                        if ((int.TryParse(instructionParts[1], out conditionValue) && conditionValue != 0) || GetRegister(instructionParts[1]).Value != 0)
                        {
                            var pointerChange = Convert.ToInt32(instructionParts[2]);
                            instructionPointer += pointerChange - 1;
                        }

                        break;
                    default:
                        throw new Exception("Unsupported instruction: " + instructions[instructionPointer]);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cpu = new BunnyPU();
            cpu.GetRegister("c").Value = 1;
            cpu.ProcessInstructions(File.ReadAllLines("input.txt").Select(t => t.Trim()).ToArray());

            Console.Write(string.Join(Environment.NewLine, cpu.Registers.Select(r => r.Name + ": " + r.Value.ToString())));
        }
    }
}
