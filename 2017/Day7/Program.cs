using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day7
{
    class InputData
    {
        public string Name { get; private set; }
        public int Weight { get; private set; }
        public IEnumerable<string> ChildrenNames { get; private set; }

        public InputData(string line)
        {
            var matches = Regex.Match(line, @"^(\w+) \((\d+)\)(| -> (.*?))$");

            Name = matches.Groups[1].Value;
            Weight = Convert.ToInt32(matches.Groups[2].Value);

            ChildrenNames = matches.Groups[4].Success 
                ? matches.Groups[4].Value.Split(',').Select(x => x.Trim()).ToArray()
                : new string[0];

        }
    }

    class Program
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public List<Program> Children { get; set; } = new List<Program>();
        public Program Parent { get; set; }

        public int GetTotalWeight()
        {
            if (!Children.Any())
                return Weight;

            return Children.Sum(c => c.GetTotalWeight()) +  Weight;
        }
    }

    class Application
    {
        static void FindAllChildNames(IEnumerable<Program> programs, List<string> childNames)
        {
            foreach(var program in programs.Where(p => p.Children.Any()))
            {
                childNames.AddRange(program.Children.Select(x => x.Name));
                FindAllChildNames(program.Children, childNames);
            }
        }

        static Program AssembleTree(InputData[] inputs)
        {
            var programs = inputs.Select(x => new Program { Name = x.Name, Weight = x.Weight }).ToList(); ;

            foreach(var input in inputs.Where(x => x.ChildrenNames.Any()))
            {
                var program = programs.First(x => x.Name == input.Name);

                foreach(var childName in input.ChildrenNames)
                {
                    var childProgram = programs.First(x => x.Name == childName);
                    childProgram.Parent = program;

                    program.Children.Add(childProgram);
                }
            }

            var childNames = new List<string>();
            FindAllChildNames(programs, childNames);

            return programs.Single(p => !childNames.Contains(p.Name));
        }

        static Program FindUnbalancedProgram(Program tree)
        {
            if (!tree.Children.Any())
                return tree.Parent;

            var weightGroupings = tree.Children.GroupBy(c => c.GetTotalWeight());
            if(weightGroupings.Count() == 1)
            {
                //All weights are the same so program is balanced
                return tree;
            }

            var orderedWeightGroupings = weightGroupings.OrderBy(x => x.Count());
            if (orderedWeightGroupings.First().Count() != 1)
                throw new NotImplementedException("OOPS!");

            return FindUnbalancedProgram(orderedWeightGroupings.First().First());
        }

        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("input.txt").Select(l => new InputData(l)).ToArray();

            var tree = AssembleTree(inputs);
            Console.WriteLine($"The bottom program is {tree.Name}");

            var unbalancedProgram = FindUnbalancedProgram(tree);
            var expectedWeight = unbalancedProgram.Parent.Children.GroupBy(c => c.GetTotalWeight()).OrderByDescending(x => x.Count()).Select(x => x.Key).First();

            var correctWeight = unbalancedProgram.Weight - (unbalancedProgram.GetTotalWeight() - expectedWeight);
            Console.WriteLine($"The correct weight is: {correctWeight}");
        }
    }
}
