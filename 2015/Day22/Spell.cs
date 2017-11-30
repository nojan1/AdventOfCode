using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    class MagicMissile : Spell
    {
        public MagicMissile(Player player) : base("Magic Missile", 53, false, player)
        {
        }

        protected override void CastInner(Boss boss)
        {
            boss.Hitpoints -= 4;
        }
    }

    class Drain : Spell
    {
        public Drain(Player player) : base("Drain", 73, false, player)
        {
        }

        protected override void CastInner(Boss boss)
        {
            boss.Hitpoints -= 2;
            _player.Hitpoints += 2;
        }
    }

    class Shield : Spell
    {
        public Shield(Player player) : base("Shield", 113, true, player)
        {
        }

        protected override void CastInner(Boss boss)
        {
            EffectActive = true;
            EffectCounter = 0;

            _player.Shield += 7;
        }

        public override void ApplyEffect(Boss boss)
        {
            base.ApplyEffect(boss);

            if(++EffectCounter == 6)
            {
                EffectActive = false;
                _player.Shield -= 7;
            }
        }
    }

    class Poison : Spell
    {
        public Poison(Player player) : base("Poison", 173, true, player)
        {
        }

        protected override void CastInner(Boss boss)
        {

            EffectActive = true;
            EffectCounter = 0;
        }

        public override void ApplyEffect(Boss boss)
        {
            base.ApplyEffect(boss);

            boss.Hitpoints -= 3;

            if (++EffectCounter == 6)
            {
                EffectActive = false;
            }
        }
    }

    class Recharge : Spell
    {
        public Recharge(Player player) : base("Recharge", 229, true, player)
        {
        }

        protected override void CastInner(Boss boss)
        {

            EffectActive = true;
            EffectCounter = 0;
        }

        public override void ApplyEffect(Boss boss)
        {
            base.ApplyEffect(boss);

            _player.Mana += 101;

            if (++EffectCounter == 5)
            {
                EffectActive = false;
            }
        }
    }

    abstract class Spell
    {
        public string Name { get; private set; }
        public int Cost { get; private set; }

        public bool CanCast { get => _player.Mana >= Cost && (!HasEffect || (HasEffect && !EffectActive)); }

        public bool HasEffect { get; protected set; }
        public bool EffectActive { get; protected set; } = false;
        public int EffectCounter { get; protected set; } = 0;

        protected Player _player;

        public Spell(string name, int cost, bool hasEffect, Player player)
        {
            Name = name;
            Cost = cost;
            HasEffect = hasEffect;

            _player = player;
        }

        public void Cast(Boss boss)
        {
            _player.Mana -= Cost;
            CastInner(boss);
        }
        
        public virtual void ApplyEffect(Boss boss) { }

        protected abstract void CastInner(Boss boss);
    }
}
