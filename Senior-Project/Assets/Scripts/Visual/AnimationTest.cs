using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("IsDrill", true);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetBool("IsDrill", false);
        }
    }
}
