using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EnumFastToStringGenerated;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchy>();
        }
    }

    [MemoryDiagnoser]
    public class Benchy
    {
        [Benchmark]
        public string NativeToString()
        {
            HumanStates state = HumanStates.Sleeping;
            return state.ToString();
        }

        [Benchmark]
        public string SwitchStatementToString()
        {
            return SwitchStatementToString(HumanStates.Sleeping);
        }

        [Benchmark]
        public string SwitchExpressionToString()
        {
            return SwitchExpressionToString(HumanStates.Sleeping);
        }

        [Benchmark]
        public string EnumFastToStringDotNetToString()
        {
            HumanStates state = HumanStates.Sleeping;
            return state.FastToString();
        }

        public string SwitchStatementToString(HumanStates state)
        {
            switch (state)
            {
                case HumanStates.Idle:
                    return nameof(HumanStates.Idle);
                case HumanStates.Working:
                    return nameof(HumanStates.Working);
                case HumanStates.Sleeping:
                    return nameof(HumanStates.Sleeping);
                case HumanStates.Eating:
                    return nameof(HumanStates.Eating);
                case HumanStates.Dead:
                    return nameof(HumanStates.Dead);
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public string SwitchExpressionToString(HumanStates state)
        {
            return state switch
            {
                HumanStates.Idle => nameof(HumanStates.Idle),
                HumanStates.Working => nameof(HumanStates.Working),
                HumanStates.Sleeping => nameof(HumanStates.Sleeping),
                HumanStates.Eating => nameof(HumanStates.Eating),
                HumanStates.Dead => nameof(HumanStates.Dead),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }

    [FastToString]
    public enum HumanStates
    {
        Idle,
        Working,
        Sleeping,
        Eating,
        Dead
    }
}
