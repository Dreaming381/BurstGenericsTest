using Unity.Burst;
using Unity.Jobs;

namespace PartialConfig
{
    public class E_PartialConfig : BurstCheckCubeColorizerBase
    {
        protected override void RunProcessor(CheckBurstProcessor processor)
        {
            Api.RepeatExecute(1, processor).Run();
        }
    }

    public partial struct Config<T> where T : struct, IRepeatExecuteProcessor
    {
        public T   processor;
        public int repeatCount;

        public void Run()
        {
            new GooeyInternals.RepeatJob { repeatCount = repeatCount, processor = processor }.Run();
        }
    }

    public static class Api
    {
        public static Config<T> RepeatExecute<T>(int repeatCount, T processor) where T : struct, IRepeatExecuteProcessor
        {
            return new Config<T> { processor = processor, repeatCount = repeatCount };
        }
    }
}

