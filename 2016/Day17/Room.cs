using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public class Room
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool[] Doors { get; private set; }

        public Room(int x, int y, string path, string passcode)
        {
            X = x;
            Y = y;

            var hash = MD5Magic.Hash(passcode + path);

            Doors = new bool[4];
            Doors[0] = y > 0 && IsOpen(0, hash); //UP
            Doors[1] = x < 3 && IsOpen(3, hash); //RIGHT
            Doors[2] = y < 3 && IsOpen(1, hash); //DOWN
            Doors[3] = x > 0 && IsOpen(2, hash); //LEFT
        }

        private bool IsOpen(int index, string hash)
        {
            return new string[] { "b", "c", "d", "e", "f" }.Contains(hash.Substring(index, 1));
        }
    }
}
