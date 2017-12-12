using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    static class ProgramListExtensions
    {
        public static Program GetOrCreateById(this List<Program> programs, string id)
        {
            var program = programs.FirstOrDefault(x => x.ID == id);
            if (program == null)
            {
                program = new Program { ID = id };
                programs.Add(program);
            }

            return program;
        }
    }

    class Program
    {
        public string ID { get; set; }
        public List<Program> PipeLinks { get; set; } = new List<Program>();
    }

    class Application
    {
        static List<Program> GetGroup(Program program, List<Program> linkedPrograms = null)
        {
            if (linkedPrograms == null)
                linkedPrograms = new List<Program>();

            if (!linkedPrograms.Contains(program))
            {
                linkedPrograms.Add(program);

                foreach (var pipeLink in program.PipeLinks)
                {
                    GetGroup(pipeLink, linkedPrograms);
                }
            }

            return linkedPrograms;
        }

        static void Main(string[] args)
        {
            var programs = new List<Program>();

            File.ReadAllLines("input.txt").ToList().ForEach(l =>
            {
                var lineParts = l.Split(new string[] { "<->" }, StringSplitOptions.RemoveEmptyEntries);

                var currentProgram = programs.GetOrCreateById(lineParts[0].Trim());
                if (lineParts.Length > 1)
                {
                    foreach (var linkedId in lineParts[1].Split(','))
                    {
                        var linkedProgram = programs.GetOrCreateById(linkedId.Trim());
                        currentProgram.PipeLinks.Add(linkedProgram);

                        if (!linkedProgram.PipeLinks.Contains(currentProgram))
                            linkedProgram.PipeLinks.Add(currentProgram);
                    }
                }
            });

            var program0 = programs.FirstOrDefault(p => p.ID == "0");
            var pipeLinks = GetGroup(program0);

            Console.WriteLine($"There are {pipeLinks.Count} programs in group 0");

            var groups = new List<List<Program>>();

            foreach (var program in programs)
            {
                var group = GetGroup(program);

                if (!groups.Any(x => x.Any(z => group.Contains(z))))
                {
                    groups.Add(group);
                }
            }

            Console.WriteLine($"Try.. {groups.Count} groups?");
        }
    }
}
