using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D18;

[ChallengeName("Duet")]
public class Y2017D18
{
    private readonly string[] _programLines = File.ReadAllLines(@"Y2017\D18\Y2017D18-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = new SingleProgramMachine()
            .Execute(_programLines)
            .First(value => value.HasValue).Value;

        output.Should().Be(9423);
    }

    [Fact]
    public void PartTwo()
    {
        var queue0 = new Queue<long>();
        var queue1 = new Queue<long>();

        int program1Sends = new DualProgramMachine(0, queue0, queue1)
            .Execute(_programLines)
            .Zip(new DualProgramMachine(1, queue1, queue0).Execute(_programLines),
                (p0, p1) => (p0: p0, p1: p1))
            .First(pair => !pair.p0.Running && !pair.p1.Running)
            .p1.SentCount;

        program1Sends.Should().Be(7620);
    }

    private abstract class DuetMachine<TState>
    {
        private readonly Dictionary<string, long> _registers = new();
        protected int InstructionPointer = 0;
        protected bool Running;

        protected long GetValue(string token)
            => long.TryParse(token, out var n) ? n : _registers.GetValueOrDefault(token);

        protected void SetRegister(string register, long value)
        {
            if (!long.TryParse(register, out _)) _registers[register] = value;
        }

        protected long this[string reg]
        {
            get => GetValue(reg);
            set => SetRegister(reg, value);
        }

        public IEnumerable<TState> Execute(string[] lines)
        {
            while (InstructionPointer >= 0 && InstructionPointer < lines.Length)
            {
                Running = true;
                var parts = lines[InstructionPointer].Split(' ');

                switch (parts[0])
                {
                    case "snd": Snd(parts[1]); break;
                    case "rcv": Rcv(parts[1]); break;
                    case "set": Set(parts[1], parts[2]); break;
                    case "add": Add(parts[1], parts[2]); break;
                    case "mul": Mul(parts[1], parts[2]); break;
                    case "mod": Mod(parts[1], parts[2]); break;
                    case "jgz": Jgz(parts[1], parts[2]); break;
                    default: throw new InvalidOperationException($"Cannot parse {lines[InstructionPointer]}");
                }

                yield return GetState();
            }

            Running = false;
            yield return GetState();
        }

        protected abstract TState GetState();
        protected abstract void Snd(string reg);
        protected abstract void Rcv(string reg);

        private void Set(string x, string y)
        {
            this[x] = GetValue(y);
            InstructionPointer++;
        }

        private void Add(string x, string y)
        {
            this[x] += GetValue(y);
            InstructionPointer++;
        }

        private void Mul(string x, string y)
        {
            this[x] *= GetValue(y);
            InstructionPointer++;
        }

        private void Mod(string x, string y)
        {
            this[x] %= GetValue(y);
            InstructionPointer++;
        }

        private void Jgz(string x, string y)
        {
            InstructionPointer += GetValue(x) > 0 ? (int)GetValue(y) : 1;
        }
    }

    private class SingleProgramMachine : DuetMachine<long?>
    {
        private long? _lastSound = null;
        private long? _recovered = null;

        protected override long? GetState() => _recovered;

        protected override void Snd(string reg)
        {
            _lastSound = this[reg];
            InstructionPointer++;
        }

        protected override void Rcv(string reg)
        {
            if (this[reg] != 0) _recovered = _lastSound;
            InstructionPointer++;
        }
    }

    private class DualProgramMachine : DuetMachine<(bool Running, int SentCount)>
    {
        private readonly Queue<long> _inputQueue;
        private readonly Queue<long> _outputQueue;
        private int _sentCount;

        public int SentCount => _sentCount;

        public DualProgramMachine(long programId, Queue<long> inputQueue, Queue<long> outputQueue)
        {
            this["p"] = programId;
            _inputQueue = inputQueue;
            _outputQueue = outputQueue;
        }

        protected override (bool Running, int SentCount) GetState() => (Running, _sentCount);

        protected override void Snd(string reg)
        {
            _outputQueue.Enqueue(this[reg]);
            _sentCount++;
            InstructionPointer++;
        }

        protected override void Rcv(string reg)
        {
            if (_inputQueue.Any())
            {
                this[reg] = _inputQueue.Dequeue();
                InstructionPointer++;
            }
            else
            {
                Running = false; // wait
            }
        }
    }
}