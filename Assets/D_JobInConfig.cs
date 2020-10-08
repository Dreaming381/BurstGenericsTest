using Unity.Burst;
using Unity.Jobs;
namespace JobInConfig
{
    public class D_JobInConfig : BurstCheckCubeColorizerBase
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
            new RepeatJob { repeatCount = repeatCount, processor = processor }.Run();
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct RepeatJob : IJob
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

    public static class Api
    {
        public static Config<T> RepeatExecute<T>(int repeatCount, T processor) where T : struct, IRepeatExecuteProcessor
        {
            return new Config<T> { processor = processor, repeatCount = repeatCount };
        }
    }
}

