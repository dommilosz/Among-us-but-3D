using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerScript : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey("w"))
        {
            animator.SetBool("isWalking", true);
        }
        if (!Input.GetKey("w"))
        {
            animator.SetBool("isWalking", false);
        }


        if (Input.GetKey("s"))
        {
            animator.SetBool("isWalkingBack", true);
        }
        if (!Input.GetKey("s"))
        {
            animator.SetBool("isWalkingBack", false);
        }

        if (Input.GetKey("d"))
        {
            animator.SetBool("isWalkingRight", true);
        }
        if (!Input.GetKey("d"))
        {
            animator.SetBool("isWalkingRight", false);
        }

        if (Input.GetKey("a"))
        {
            animator.SetBool("isWalkingLeft", true);
        }
        if (!Input.GetKey("a"))
        {
            animator.SetBool("isWalkingLeft", false);
        }
    }
}
