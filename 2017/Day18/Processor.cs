using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    public class Register
    {
        public string Name { get; set; }
        public long Value { get; set; }
    }

    public class Processor
    {
        private DayPart _dayPart;
        private string[] _instructions;

        public List<Register> Registers { get; private set; } = new List<Register>();
        public Queue<long> InputQueue { get; private set; } = new Queue<long>();

        public event Action<long> OnRecieve = delegate { };
        public event Action<long> OnSend = delegate { };

        public Dictionary<string, int> InstructionUsageCount { get; set; } = new Dictionary<string, int>();

        public Processor(string[] instructions, DayPart dayPart)
        {
            _instructions = instructions;
            _dayPart = dayPart;
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
            if (instructionPointer > _instructions.Count() - 1)
            {
                return -1;
            }

            var instructionParts = _instructions[instructionPointer].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToLower()).ToArray();

            if (instructionParts[0].StartsWith("//"))
                return instructionPointer;

            switch (instructionParts[0])
            {
                case "set":
                    GetRegister(instructionParts[1]).Value = GetRawOrRegisterValue(instructionParts[2]);

                    break;
                case "add":
                    GetRegister(instructionParts[1]).Value += GetRawOrRegisterValue(instructionParts[2]);

                    break;
                case "sub":
                    GetRegister(instructionParts[1]).Value -= GetRawOrRegisterValue(instructionParts[2]);

                    break;
                case "jgz":
                    var conditionValue = GetRawOrRegisterValue(instructionParts[1]);
                    if (conditionValue > 0)
                    {
                        var pointerChange = GetRawOrRegisterValue(instructionParts[2]);
                        instructionPointer += (int)pointerChange - 1;
                    }

                    break;
                case "jnz":
                    var conditionJumpValue = GetRawOrRegisterValue(instructionParts[1]);
                    if (conditionJumpValue != 0)
                    {
                        var pointerChange = GetRawOrRegisterValue(instructionParts[2]);
                        instructionPointer += (int)pointerChange - 1;
                    }

                    break;
                case "mul":
                    GetRegister(instructionParts[1]).Value = GetRawOrRegisterValue(instructionParts[1]) * GetRawOrRegisterValue(instructionParts[2]);

                    break;
                case "mod":
                    GetRegister(instructionParts[1]).Value = GetRawOrRegisterValue(instructionParts[1]) % GetRawOrRegisterValue(instructionParts[2]);

                    break;
                case "rcv":
                    if (_dayPart == DayPart.One)
                    {
                        var value = GetRawOrRegisterValue(instructionParts[1]);
                        if (value != 0)
                        {
                            OnRecieve(value);
                        }
                    }
                    else
                    {
                        if (!InputQueue.Any())
                            return instructionPointer;

                        GetRegister(instructionParts[1]).Value = InputQueue.Dequeue();
                    }

                    break;
                case "snd":
                    OnSend(GetRawOrRegisterValue(instructionParts[1]));

                    break;
                case "nop":
                    //Troloolol
                    break;
                default:
                    throw new Exception("Unsupported instruction: " + _instructions[instructionPointer]);
            }

            if (InstructionUsageCount.ContainsKey(instructionParts[0]))
            {
                InstructionUsageCount[instructionParts[0]]++;
            }
            else
            {
                InstructionUsageCount[instructionParts[0]] = 1;
            }

            return instructionPointer + 1;
        }

        public void RunToEnd()
        {
            for (int instructionPointer = 0; instructionPointer < _instructions.Count(); instructionPointer++)
            {
                instructionPointer = RunSingleInstruction(instructionPointer) - 1;
            }
        }

        private long GetRawOrRegisterValue(string x)
        {
            long value;
            if (!long.TryParse(x, out value))
            {
                value = GetRegister(x).Value;
            }

            return value;
        }
    }
}
