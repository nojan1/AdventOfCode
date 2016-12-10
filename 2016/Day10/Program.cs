using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    public static class DatacollectionExtensions
    {
        public static Bot GetOrCreate(this Dictionary<int, Bot> bots, int number)
        {
            if (!bots.ContainsKey(number))
            {
                bots.Add(number, new Bot { Number = number });
            }

            return bots[number];
        }

        public static List<int> GetOrCreate(this Dictionary<int, List<int>> outputs, int number)
        {
            if (!outputs.ContainsKey(number))
            {
                outputs.Add(number, new List<int>());
            }

            return outputs[number];
        }
    }

    public enum ChipType
    {
        Low,
        High
    }

    public enum TransferType
    {
        Bot,
        Output
    }

    public class TransferInstruction
    {
        public ChipType ChipType { get; set; }
        public int To { get; set; }
        public TransferType TransferType { get; set; }
    }

    public class Bot
    {
        public int Number { get; set; }
        public List<int> Chips { get; set; } = new List<int>();
        public List<TransferInstruction> TransferInstructions { get; set; } = new List<TransferInstruction>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bots = new Dictionary<int, Bot>();
            var outputs = new Dictionary<int, List<int>>();

            foreach (var line in File.ReadAllLines("input.txt").Select(t => t.Trim()))
            {
                var words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (words[0] == "value")
                {
                    var bot = bots.GetOrCreate(Convert.ToInt32(words[5]));
                    bot.Chips.Add(Convert.ToInt32(words[1]));
                }
                else if (words[0] == "bot")
                {
                    var bot = bots.GetOrCreate(Convert.ToInt32(words[1]));

                    var numberLow = Convert.ToInt32(words[6]);
                    var numberHigh = Convert.ToInt32(words[11]);

                    var typeLow = (TransferType)Enum.Parse(typeof(TransferType), words[5], true);
                    var typeHigh = (TransferType)Enum.Parse(typeof(TransferType), words[10], true);

                    bot.TransferInstructions.Add(new TransferInstruction { ChipType = ChipType.Low, To = numberLow, TransferType = typeLow });
                    bot.TransferInstructions.Add(new TransferInstruction { ChipType = ChipType.High, To = numberHigh, TransferType = typeHigh });
                }
                else
                {
                    throw new Exception("I don't know this line: " + line);
                }
            }

            while (true)
            {
                var botsThatCanTransfer = bots.Values.Where(b => b.Chips.Count == 2).ToList();
                if (botsThatCanTransfer.Count == 0)
                    break;

                foreach(var bot in botsThatCanTransfer)
                {
                    if(bot.Chips.Contains(17) && bot.Chips.Contains(61))
                    {
                        Console.WriteLine($"The bot in question is {bot.Number}");
                    }

                    var instructions = bot.TransferInstructions.ToList();
                    foreach(var instruction in instructions)
                    {
                        var toTransfer = instruction.ChipType == ChipType.High ? bot.Chips.Max() : bot.Chips.Min();

                        if (instruction.TransferType == TransferType.Bot)
                        {
                            var botToTransferTo = bots.GetOrCreate(instruction.To);
                            botToTransferTo.Chips.Add(toTransfer);
                        }else
                        {
                            var output = outputs.GetOrCreate(instruction.To);
                            output.Add(toTransfer);
                        }

                        bot.Chips.Remove(toTransfer);
                        bot.TransferInstructions.Remove(instruction);
                    }
                }
            }

            var chipProduct = outputs[0].First() * outputs[1].First() * outputs[2].First();
            Console.WriteLine($"Hmmm try: {chipProduct}");
        }
    }
}
