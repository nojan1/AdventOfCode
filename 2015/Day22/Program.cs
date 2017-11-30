using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    
    class Player
    {
        public int Hitpoints { get; set; }
        public int Mana { get; set; }
        public int Shield { get; set; }

        public ICollection<Spell> Spells { get; set; }

        public Player()
        {
            Reset();
        }

        public void ApplyEffects(Boss boss)
        {
            foreach (var spell in Spells.Where(s => s.EffectActive))
                spell.ApplyEffect(boss);
        }

        public void TakeDamage(int damage)
        {
            Hitpoints -= Math.Max(1, damage - Shield);
        }

        public void Reset()
        {
            Mana = 500;
            Hitpoints = 50;
            Shield = 0;

            Spells = new Spell[]
            {
                new MagicMissile(this),
                new Drain(this),
                new Shield(this),
                new Poison(this),
                new Recharge(this)
            };
        }
    }

    class Boss
    {
        public int Hitpoints { get; set; }
        public int Damage { get; private set; }

        private int _startingHitpoints;

        public Boss(int hitpoints, int damage)
        {
            _startingHitpoints = hitpoints;
            Damage = damage;

            Reset();
        }

        public void Reset()
        {
            Hitpoints = _startingHitpoints;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var boss = new Boss(55, 8);
            var player = new Player();

            var rnd = new Random();
            int lowestManaUsage = int.MaxValue;

            while (true)
            {
                int manaUsed = 0;
                var spellsUsed = new List<string>();

                player.Reset();
                boss.Reset();

                while (boss.Hitpoints > 0 && player.Hitpoints > 0)
                {
                    //Players turn
                    player.Hitpoints -= 1;
                    if (player.Hitpoints <= 0)
                        break;

                    player.ApplyEffects(boss);

                    if (player.Mana < player.Spells.Min(s => s.Cost))
                    {
                        player.Hitpoints = 0;
                        break;
                    }

                    var castableSpells = player.Spells.Where(s => s.CanCast).ToArray();
                    var spellToCast = castableSpells[rnd.Next(0, castableSpells.Length)];

                    spellsUsed.Add(spellToCast.Name);
                    manaUsed += spellToCast.Cost;
                    spellToCast.Cast(boss);

                    //Boss turn
                    player.ApplyEffects(boss);
                    player.TakeDamage(boss.Damage);
                }

                if(boss.Hitpoints <= 0 && manaUsed < lowestManaUsage)
                {
                    lowestManaUsage = manaUsed;

                    Console.Clear();
                    Console.WriteLine($"Mana used: {lowestManaUsage}");
                    Console.WriteLine(string.Join(" -> ", spellsUsed));
                }
            }
        }
    }
}
