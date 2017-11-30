using Common;
using Day12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class Runner : GenericParallelTaskRunnerBase<int, bool>
    {
        private const int NUM_THREADS = 8;

        public int CorrectValue { get; private set; }

        private string[] _instructions;
        private int a = 1;

        public Runner(string[] instructions) : base(NUM_THREADS)
        {
            _instructions = instructions;
        }

        protected override int CreateTaskParameter()
        {
            return a++;
        }

        protected override void OnTaskFinished(int taskParameter, bool taskReturnValue)
        {
            if (taskReturnValue)
            {
                CorrectValue = taskParameter;
                RunComplete = true;
            }
        }

        protected override bool Worker(int a)
        {
            var outputBuffer = new List<int>();
            var cpu = new BunnyPU(_instructions);

            cpu.OnOutput += x =>
            {
                outputBuffer.Add(x);
            };

            int pointer = 0;

            outputBuffer.Clear();
            pointer = 0;
            cpu.GetRegister("a").Value = a;

            while (outputBuffer.Count < 20)
            {
                pointer = cpu.RunSingleInstruction(pointer);
                if (pointer == -1)
                    break;
            }

            return BufferContainsRepeatingPattern(outputBuffer);
        }

        private bool BufferContainsRepeatingPattern(List<int> outputBuffer)
        {
            for (int i = 0; i < outputBuffer.Count; i+= 4)
            {
                if (outputBuffer[i + 0] != 0 ||
                   outputBuffer[i + 1] != 1 ||
                   outputBuffer[i + 2] != 0 ||
                   outputBuffer[i + 3] != 1)
                {
                    return false;
                }
            }

            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("assembunny.txt").Select(t => t.Trim()).ToArray();
            var runner = new Runner(instructions);
            runner.RunToEnd();

            Console.WriteLine($"The magic value is {runner.CorrectValue}");
        }
    }
}
