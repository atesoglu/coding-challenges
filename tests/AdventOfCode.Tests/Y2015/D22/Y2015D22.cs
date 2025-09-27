using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D22;

[ChallengeName("Wizard Simulator 20XX")]
public class Y2015D22
{
    private readonly State _initialState;

    public Y2015D22()
    {
        var lines = File.ReadAllLines(@"Y2015\D22\Y2015D22-input.txt", Encoding.UTF8);
        _initialState = new State
        {
            PlayerHp = 50,
            PlayerMana = 500,
            BossHp = int.Parse(lines[0].Split(": ")[1]),
            BossDamage = int.Parse(lines[1].Split(": ")[1])
        };
    }

    [Fact]
    public void PartOne()
    {
        var output = FindMinimumManaToWin(_initialState, hardMode: false);
        output.Should().Be(1269);
    }

    [Fact]
    public void PartTwo()
    {
        var output = FindMinimumManaToWin(_initialState, hardMode: true);
        output.Should().Be(1309);
    }

    private static int FindMinimumManaToWin(State state, bool hardMode)
    {
        int lo = 0, hi = 1;

        // Find upper bound
        while (!CanDefeatBossWithinMana(state.CloneWithManaLimit(hi), hardMode))
            hi *= 2;

        // Binary search for minimal mana
        while (hi - lo > 1)
        {
            var mid = (lo + hi) / 2;
            if (CanDefeatBossWithinMana(state.CloneWithManaLimit(mid), hardMode))
                hi = mid;
            else
                lo = mid;
        }

        return hi;
    }

    private static bool CanDefeatBossWithinMana(State state, bool hardMode)
    {
        if (hardMode) state = state.TakeDamage(1);
        state = state.ApplyActiveEffects();

        if (state.BossHp <= 0) return true;
        if (state.PlayerHp <= 0) return false;

        foreach (var next in state.PlayerTurns())
        {
            var afterEffects = next.ApplyActiveEffects();
            var afterBoss = afterEffects.BossTurn();

            if (afterBoss.BossHp <= 0 ||
                (afterBoss.PlayerHp > 0 && CanDefeatBossWithinMana(afterBoss, hardMode)))
                return true;
        }

        return false;
    }

    private class State
    {
        public int PlayerHp, PlayerMana, BossHp, BossDamage;
        public int Shield, Poison, Recharge, UsedMana, PlayerArmor, ManaLimit;

        public State Clone() => (State)MemberwiseClone();

        public State CloneWithManaLimit(int limit)
        {
            var s = Clone();
            s.ManaLimit = limit;
            return s;
        }

        public State ApplyActiveEffects()
        {
            var s = Clone();
            if (s.Poison > 0)
            {
                s.BossHp -= 3;
                s.Poison--;
            }

            if (s.Recharge > 0)
            {
                s.PlayerMana += 101;
                s.Recharge--;
            }

            s.PlayerArmor = (s.Shield > 0) ? 7 : 0;
            if (s.Shield > 0) s.Shield--;
            return s;
        }

        public State TakeDamage(int dmg)
        {
            var s = Clone();
            s.PlayerHp -= dmg;
            return s;
        }

        public State BossTurn()
        {
            var s = Clone();
            s.PlayerHp -= Math.Max(1, s.BossDamage - s.PlayerArmor);
            return s;
        }

        public IEnumerable<State> PlayerTurns()
        {
            foreach (var spell in Spell.All)
            {
                if (spell.IsCastable(this))
                {
                    var s = Clone();
                    s.PlayerMana -= spell.Cost;
                    s.UsedMana += spell.Cost;
                    spell.Cast(s);
                    yield return s;
                }
            }
        }
    }

    private class Spell
    {
        public string Name { get; }
        public int Cost { get; }
        private readonly Action<State> _cast;
        private readonly Func<State, bool> _canCast;

        private Spell(string name, int cost, Action<State> cast, Func<State, bool>? canCast = null)
        {
            Name = name;
            Cost = cost;
            _cast = cast;
            _canCast = canCast ?? (_ => true);
        }

        public void Cast(State s) => _cast(s);
        public bool IsCastable(State s) => s.PlayerMana >= Cost && s.UsedMana + Cost <= s.ManaLimit && _canCast(s);

        public static readonly Spell[] All =
        [
            new("MagicMissile", 53, s => s.BossHp -= 4),
            new("Drain", 73, s =>
            {
                s.BossHp -= 2;
                s.PlayerHp += 2;
            }),
            new("Shield", 113, s => s.Shield = 6, s => s.Shield == 0),
            new("Poison", 173, s => s.Poison = 6, s => s.Poison == 0),
            new("Recharge", 229, s => s.Recharge = 5, s => s.Recharge == 0)
        ];
    }
}