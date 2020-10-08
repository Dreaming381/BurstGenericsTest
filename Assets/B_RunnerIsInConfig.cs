using Unity.Burst;
using Unity.Jobs;

namespace RunnerIsInConfig
{
    public class B_RunnerIsInConfig : BurstCheckCubeColorizerBase
    {
        protected override void RunProcessor(CheckBurstProcessor processor)
        {
            Api.RepeatExecute(1, processor).Run();
        }
    }

    public struct Config<T> where T : struct, IRepeatExecuteProcessor
    {
        public T   processor;
        public int repeatCount;

        public void Run()
        {
            new JobDirectory.RepeatJob<T> { repeatCount = repeatCount, processor = processor }.Run();
        }
    }

    public static class Api
    {
        public static Config<T> RepeatExecute<T>(int repeatCount, T processor) where T : struct, IRepeatExecuteProcessor
        {
            return new Config<T> { processor = processor, repeatCount = repeatCount };
        }
    }

    internal static class JobDirectory
    {
        [BurstCompile(CompileSynchronously = true)]
        public struct RepeatJob<T> : IJob where T : struct, IRepeatExecuteProcessor
        {
            public int repeatCount;
            public T   processor;

            public void Execute()
            {
                for (int i = 0; i < repeatCount; i++)
                {
                    processor.Execute(i, repeatCount);
                }
            }
        }
    }
}

