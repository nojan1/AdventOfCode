using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    class Equpiment
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
    }

    class Fighter
    {
        public int Hitpoints { get; private set; }
        public List<Equpiment> Equipment { get; private set; } = new List<Equpiment>();

        public int Damage { get => _baseDamage + Equipment.Sum(x => x.Damage); }
        public int Armor { get => _baseArmor + Equipment.Sum(x => x.Armor); }

        private int _startingHitpoints;
        private int _baseDamage;
        private int _baseArmor;

        public Fighter(int hitpoints, int basedamage, int basearmor)
        {
            Hitpoints = hitpoints;

            _startingHitpoints = Hitpoints;
            _baseDamage = basedamage;
            _baseArmor = basearmor;
        }

        public bool TakeDamage(int damage)
        {
            Hitpoints -= Math.Max(1, damage - Armor);

            return Hitpoints > 0;
        }

        public void Reset()
        {
            Equipment.Clear();
            Hitpoints = _startingHitpoints;
        }
    }

    class Program
    {
        static List<Equpiment> GetWeapons()
        {
            return new List<Equpiment>
            {
                new Equpiment { Name = "Dagger", Cost = 8, Damage = 4, Armor = 0 },
                new Equpiment { Name = "Shortsword", Cost = 10, Damage = 5, Armor = 0 },
                new Equpiment { Name = "Warhammer", Cost = 25, Damage = 6, Armor = 0 },
                new Equpiment { Name = "Longsword", Cost = 40, Damage = 7, Armor = 0 },
                new Equpiment { Name = "Greataxe", Cost = 74, Damage = 8, Armor = 0 }
            };
        }

        static List<Equpiment> GetArmors()
        {
            return new List<Equpiment>
            {
                new Equpiment { Name = "Leather", Cost = 13, Damage = 0, Armor = 1 },
                new Equpiment { Name = "Chainmail", Cost = 31, Damage = 0, Armor = 2 },
                new Equpiment { Name = "Splintmail", Cost = 53, Damage = 0, Armor = 3 },
                new Equpiment { Name = "Bandedmail", Cost = 75, Damage = 0, Armor = 4 },
                new Equpiment { Name = "Platemail", Cost = 102, Damage = 0, Armor = 5 }
            };
        }

        static List<Equpiment> GetRings()
        {
            return new List<Equpiment>
            {
                new Equpiment { Name = "Damage +1", Cost = 25, Damage = 1, Armor = 0 },
                new Equpiment { Name = "Damage +2", Cost = 50, Damage = 2, Armor = 0 },
                new Equpiment { Name = "Damage +3", Cost = 100, Damage = 3, Armor = 0 },
                new Equpiment { Name = "Defense +1", Cost = 20, Damage = 0, Armor = 1 },
                new Equpiment { Name = "Defense +2", Cost = 40, Damage = 0, Armor = 2 },
                new Equpiment { Name = "Defense +3", Cost = 80, Damage = 0, Armor = 3 }
            };
        }

        static void Main(string[] args)
        {
            List<(int, bool)> results = new List<(int, bool)>();

            var boss = new Fighter(100, 8, 2);
            var me = new Fighter(100, 0, 0);

            var weapons = GetWeapons();
            var armors = GetArmors();
            armors.Add(null);

            var rings = new Combinations<Equpiment>(GetRings(), 2).ToList();
            rings.AddRange(GetRings().Select(x => new List<Equpiment> { x }));
            rings.Add(null);

            foreach (var weapon in weapons)
            {
                foreach (var armor in armors)
                {
                    foreach (var ring in rings)
                    {
                        int cost = 0;
                        boss.Reset();
                        me.Reset();

                        me.Equipment.Add(weapon);
                        cost += weapon.Cost;

                        if (armor != null)
                        {
                            me.Equipment.Add(armor);
                            cost += armor.Cost;
                        }

                        if (ring != null)
                        {
                            me.Equipment.AddRange(ring);
                            cost += ring.Sum(x => x.Cost);
                        }

                        while (boss.Hitpoints > 0 && me.Hitpoints > 0)
                        {
                            if (!boss.TakeDamage(me.Damage))
                                break;

                            me.TakeDamage(boss.Damage);
                        }

                        results.Add((cost, boss.Hitpoints <= 0));
                    }
                }
            }

            Console.WriteLine($"Minimum cost is {results.Where(x => x.Item2).Min(x => x.Item1)}");
            Console.WriteLine($"Maxiumem fail cost is {results.Where(x => !x.Item2).Max(x => x.Item1)}");
        }
    }
}
