

using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class AIController : MonoBehaviour
{
    public Vector2 target;
    
    public float speed;
    public bool IsStale { get; private set; }

    public void Update()
    {
           
    }

}
