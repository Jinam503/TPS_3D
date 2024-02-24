using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    // ReSharper disable Unity.PerformanceAnalysis
    public virtual State Tick(ZombieManager zombieManager)
    {
        Debug.Log("running");
        return this;
    }
}
