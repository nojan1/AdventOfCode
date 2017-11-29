using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    enum State
    {
        Flying, 
        Resting
    }

    class Reindeer
    {
        public string Name { get; private set; }
        public int Distance { get; private set; }
        public int Score { get; private set; }
        public State State { get; private set; }

        private int _speed;
        private int _stamina;
        private int _restingTime;

        private int _durationTimer;

        public Reindeer(string name, int speed, int stamina, int restingTime)
        {
            Name = name;

            _speed = speed;
            _stamina = stamina;
            _restingTime = restingTime;

            State = State.Flying;
        }

        public void Tick()
        {
            _durationTimer++;

            if(State == State.Flying)
            {
                Distance += _speed;

                if(_durationTimer == _stamina)
                {
                    _durationTimer = 0;
                    State = State.Resting;
                }
            }
            else if(_durationTimer == _restingTime)
            {
                State = State.Flying;
                _durationTimer = 0;
            }
        }

        public void AwardScore(int amount = 1)
        {
            Score += amount;
        }

        public override string ToString()
        {
            return $"{Name} {Distance}km {Score}pts";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var deers = new List<Reindeer>();

            foreach (var line in File.ReadAllLines("stats.txt"))
            {
                var parts = line.Split(' ');

                deers.Add(new Reindeer(parts[0], 
                                       Convert.ToInt32(parts[3]),
                                       Convert.ToInt32(parts[6]),
                                       Convert.ToInt32(parts[13])));
            }

            for (int sec = 1; sec <= 2503; sec++)
            {
                deers.ForEach(d => d.Tick());

                var maxDistance = deers.Max(d => d.Distance);
                deers.Where(d => d.Distance == maxDistance).ToList().ForEach(d => d.AwardScore());
            }

            var winner = deers.Single(d => d.Score == deers.Max(d2 => d2.Score));
            Console.WriteLine($"The winner is {winner.Name} with {winner.Distance} km traveled and a score of {winner.Score}");
        }
    }
}
