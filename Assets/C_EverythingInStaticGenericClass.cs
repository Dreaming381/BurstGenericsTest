using Unity.Burst;
using Unity.Jobs;

namespace EverythingInStaticGenericClass
{
    public class C_EverythingInStaticGenericClass : BurstCheckCubeColorizerBase
    {
        protected override void RunProcessor(CheckBurstProcessor processor)
        {
            //The invocation here is complicated, as now I have to repeat the type of the processor at the beginning of the chain.
            //From an API design POV, I don't like this.
            Api<CheckBurstProcessor>.RepeatExecute(1, processor).Run();
        }
    }

    public static class Api<T> where T : struct, IRepeatExecuteProcessor
    {
        public static Config RepeatExecute(int repeatCount, T processor)
        {
            return new Config { processor = processor, repeatCount = repeatCount };
        }

        public struct Config
        {
            public T   processor;
            public int repeatCount;

            public void Run()
            {
                new RepeatJob { repeatCount = repeatCount, processor = processor }.Run();
            }
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
}

