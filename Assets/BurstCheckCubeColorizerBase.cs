using Unity.Burst;
using Unity.Collections;
using UnityEngine;

public interface IRepeatExecuteProcessor
{
    void Execute(int iteration, int totalIterations);
}

public struct CheckBurstProcessor : IRepeatExecuteProcessor
{
    public NativeReference<bool> ranInBurst;

    public void Execute(int iteration, int totalIterations)
    {
        bool temp = true;
        NotInBurstCheck(ref temp);
        ranInBurst.Value = temp;
    }

    [BurstDiscard]
    void NotInBurstCheck(ref bool inBurst)
    {
        inBurst = false;
    }
}

public abstract class BurstCheckCubeColorizerBase : MonoBehaviour
{
    void Start()
    {
        var ranInBurst = new NativeReference<bool>(Allocator.TempJob);
        var processor  = new CheckBurstProcessor { ranInBurst = ranInBurst };
        RunProcessor(processor);

        var mr  = GetComponent<MeshRenderer>();
        var mat = mr.material;
        if (ranInBurst.Value)
        {
            mat.color = Color.green;
        }
        else
        {
            mat.color = Color.red;
        }

        ranInBurst.Dispose();
    }

    protected abstract void RunProcessor(CheckBurstProcessor processor);
}

