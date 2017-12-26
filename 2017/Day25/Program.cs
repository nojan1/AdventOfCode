using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class RuleDefinition
    {
        public bool Write { get; set; }
        public int MoveDirection { get; set; }
        public string ContinueWith { get; set; }
    }

    class StateRules
    {
        public Dictionary<bool, RuleDefinition> Rules { get; set; }
    }
   

    class Program
    {
        static Dictionary<string, StateRules> GetRules()
        {
            return new Dictionary<string, StateRules>
            {
                {"A", new StateRules
                {
                    Rules = new Dictionary<bool, RuleDefinition>
                    {
                        {false, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = +1,
                            ContinueWith = "B"
                        }},
                        {true, new RuleDefinition
                        {
                            Write = false,
                            MoveDirection = -1,
                            ContinueWith = "C"
                        } }
                    }
                } },
                {"B", new StateRules
                {
                    Rules = new Dictionary<bool, RuleDefinition>
                    {
                        {false, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = -1,
                            ContinueWith = "A"
                        }},
                        {true, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = +1,
                            ContinueWith = "C"
                        } }
                    }
                } },
                {"C", new StateRules
                {
                    Rules = new Dictionary<bool, RuleDefinition>
                    {
                        {false, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = +1,
                            ContinueWith = "A"
                        }},
                        {true, new RuleDefinition
                        {
                            Write = false,
                            MoveDirection = -1,
                            ContinueWith = "D"
                        } }
                    }
                } },
                {"D", new StateRules
                {
                    Rules = new Dictionary<bool, RuleDefinition>
                    {
                        {false, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = -1,
                            ContinueWith = "E"
                        }},
                        {true, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = -1,
                            ContinueWith = "C"
                        } }
                    }
                } },
                {"E", new StateRules
                {
                    Rules = new Dictionary<bool, RuleDefinition>
                    {
                        {false, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = +1,
                            ContinueWith = "F"
                        }},
                        {true, new RuleDefinition
                        {
                            Write = false,
                            MoveDirection = +1,
                            ContinueWith = "A"
                        } }
                    }
                } },
                {"F", new StateRules
                {
                    Rules = new Dictionary<bool, RuleDefinition>
                    {
                        {false, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = +1,
                            ContinueWith = "A"
                        }},
                        {true, new RuleDefinition
                        {
                            Write = true,
                            MoveDirection = +1,
                            ContinueWith = "E"
                        } }
                    }
                } }
            };
        }

        static void Main(string[] args)
        {
            var numSteps = 12261543;
            var state = "A";
            var rules = GetRules();

            var tape = new InfiniteCollection<bool>();
            int cursor = 0;

            for(int step = 0; step < numSteps; step++)
            {
                var rule = rules[state].Rules[tape[cursor]];

                tape[cursor] = rule.Write;
                cursor += rule.MoveDirection;
                state = rule.ContinueWith;
            }

            var numTrue = tape.Count(x => x.Value);
            Console.WriteLine($"There are {numTrue} positions on");
        }
    }
}
