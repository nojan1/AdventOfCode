using Common;
using System.Collections.Generic;
using System.Linq;

namespace Day9
{
    class Parser
    {
        private string _stream;

        public Parser(string stream)
        {
            _stream = stream;
        }

        public Group GetRootGroup(out int garbageCount)
        {
            var groups = new List<Group>();
            var groupStarts = new Stack<int>();
            var insideGarbage = false;
            garbageCount = 0;

            for (int i = 0; i < _stream.Length; i++)
            {
                var c = _stream[i];
                if (c == '!') { 
                    i++;
                    continue;
                }

                if (insideGarbage)
                {
                    if (c == '>')
                    {
                        insideGarbage = false;
                    }
                    else
                    {
                        garbageCount++;
                    }
                }
                else
                {
                    switch (c)
                    {
                        case '<':
                            insideGarbage = true;
                            break;
                        case '{':
                            groupStarts.Push(i);
                            break;
                        case '}':
                            groups.Add(new Group
                            {
                                Start = groupStarts.Pop(),
                                Stop = i
                            });
                            break;
                    }
                }
            }

            var sortedGroups = groups.OrderBy(g => g.Start).ToList();
            var rootGroup = sortedGroups.First();

            foreach (var group in sortedGroups.Skip(1))
            {
                var placed = PlaceGroup(rootGroup, group);
                if (!placed)
                    throw new WtfException("So now what?");
            }

            return rootGroup;
        }

        private bool PlaceGroup(Group current, Group toPlace)
        {
            foreach (var child in current.Children)
            {
                var placed = PlaceGroup(child, toPlace);
                if (placed)
                    return true;
            }

            if (toPlace.Start > current.Start && toPlace.Stop < current.Stop)
            {
                current.Children.Add(toPlace);
                return true;
            }

            return false;
        }
    }
}
