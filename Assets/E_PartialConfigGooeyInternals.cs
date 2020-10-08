using Unity.Burst;
using Unity.Jobs;

namespace PartialConfig
{
    public partial struct Config<T> where T : struct, IRepeatExecuteProcessor
    {
        internal static class GooeyInternals
        {
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
}

