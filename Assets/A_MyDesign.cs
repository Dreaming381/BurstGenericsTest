using Unity.Burst;
using Unity.Jobs;

namespace MyDesign
{
    public class A_MyDesign : BurstCheckCubeColorizerBase
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
    }

    public static class Api
    {
        public static Config<T> RepeatExecute<T>(int repeatCount, T processor) where T : struct, IRepeatExecuteProcessor
        {
            return new Config<T> { processor = processor, repeatCount = repeatCount };
        }

        public static void Run<T>(this Config<T> config) where T : struct, IRepeatExecuteProcessor
        {
            new JobDirectory.RepeatJob<T> { repeatCount = config.repeatCount, processor = config.processor }.Run();
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

