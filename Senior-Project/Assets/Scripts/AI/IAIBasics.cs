using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIBasics
{
    abstract Vector2 Target { get; set; }

    abstract void ExecutePath();
    abstract bool SetDestination(Vector2 target);
    abstract bool SetDestination(Transform target);

}

