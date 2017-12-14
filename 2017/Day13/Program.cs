using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    interface IScanner
    {
        int Length { get; }
        int CurrentPosition { get; }

        void Advance();
        bool IsCaught(Packet packet);
    }

    class Scanner : IScanner
    {
        public int Length { get; private set; }
        public int CurrentPosition { get; private set; }

        private int _direction;

        public Scanner(int length)
        {
            Length = length;
            CurrentPosition = 0;
            _direction = +1;
        }

        public void Advance()
        {
            CurrentPosition += _direction;

            if (CurrentPosition == Length - 1 || CurrentPosition == 0)
            {
                _direction *= -1;
            }
        }

        public bool IsCaught(Packet packet)
        {
            return CurrentPosition == 0;
        }
    }

    class DummyScanner : IScanner
    {
        public int Length => 0;

        public int CurrentPosition => 0;

        public void Advance()
        {
            //Nope
        }

        public bool IsCaught(Packet packet)
        {
            return false;
        }
    }

    class Firewall
    {
        public List<IScanner> Scanners { get; private set; }

        public Firewall(IDictionary<int, int> scannerDefinitions)
        {
            Scanners = new List<IScanner>();
            for (int i = 0; i <= scannerDefinitions.Keys.Max(); i++)
            {
                if (scannerDefinitions.ContainsKey(i))
                {
                    Scanners.Add(new Scanner(scannerDefinitions[i]));
                }
                else
                {
                    Scanners.Add(new DummyScanner());
                }
            }
        }

        public void UpdateScanners()
        {
            Scanners.ForEach(s => s.Advance());
        }
    }

    class Packet
    {
        public int BornAt { get; private set; }
        public List<int> CaughtAt { get; private set; } = new List<int>();
        public int Position { get; private set; }
        public bool HasPassedFirewall { get; private set; }

        private Firewall _firewall;

        public Packet(Firewall firewall, int bornAt)
        {
            _firewall = firewall;
            BornAt = bornAt;
        }

        public void CheckCaught()
        {
            if (HasPassedFirewall)
                return;

            if (_firewall.Scanners[Position].IsCaught(this))
            {
                CaughtAt.Add(Position);
            }
        }

        public void Update()
        {
            if (HasPassedFirewall)
                return;

            Position++;

            if (Position > _firewall.Scanners.Count - 1)
            {
                HasPassedFirewall = true;
                return;
            }

            CheckCaught();
        }
    }

    class Program
    {
        static int A(IDictionary<int, int> scannerDefinitions)
        {
            var firewall = new Firewall(scannerDefinitions);
            var packet = new Packet(firewall, 0);

            packet.CheckCaught();

            while (!packet.HasPassedFirewall)
            {
                firewall.UpdateScanners();
                packet.Update();
            }

            return packet.CaughtAt.Aggregate((tot, i) => tot + (i * scannerDefinitions[i]));
        }

        static int B(IDictionary<int, int> scannerDefinitions)
        {
            int timeCounter = 0;

            var packets = new List<Packet>();
            var firewall = new Firewall(scannerDefinitions);

            while (!packets.Any(p => p.HasPassedFirewall && !p.CaughtAt.Any()))
            {
                packets.Add(new Packet(firewall, timeCounter++));

                firewall.UpdateScanners();
                packets.ForEach(p => p.Update());
            }

            return packets.First(p => p.HasPassedFirewall && !p.CaughtAt.Any()).BornAt;
        }

        static void Main(string[] args)
        {
            var scannerDefinitions = File.ReadAllLines("input.txt").Select(l => l.Split(':')).ToDictionary(x => Convert.ToInt32(x[0].Trim()), x => Convert.ToInt32(x[1].Trim()));

            var severity = A(scannerDefinitions);
            Console.WriteLine($"Severity is {severity}");

            var lowestDelay = B(scannerDefinitions);
            Console.WriteLine($"Lowest possible delay is {lowestDelay}");
        }
    }
}
