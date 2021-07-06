using System;
using UnityEngine;

public class AnimationControllerScript : MonoBehaviour
{
    public float minimumMoveToTrigger = 5;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        pos = transform.position;
    }

    Vector3 pos;
    // Update is called once per frame
    void Update()
    {
        Vector3 currpos = transform.position;
        bool moved = ((Time.deltaTime * (Math.Abs(currpos.x - pos.x)) > minimumMoveToTrigger) || ((Time.deltaTime * (Math.Abs(currpos.z - pos.z)) > minimumMoveToTrigger)));
        if (PlayerInfo.isMine(gameObject.transform.parent.parent.gameObject))
        {
            if (Input.GetKey("w") && moved)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }


            if (Input.GetKey("s") && moved)
            {
                animator.SetBool("isWalkingBack", true);
            }
            else
            {
                animator.SetBool("isWalkingBack", false);
            }

            if (Input.GetKey("d") && moved)
            {
                animator.SetBool("isWalkingRight", true);
            }
            else
            {
                animator.SetBool("isWalkingRight", false);
            }

            if (Input.GetKey("a") && moved)
            {
                animator.SetBool("isWalkingLeft", true);
            }
            else
            {
                animator.SetBool("isWalkingLeft", false);
            }
            pos = transform.position;
        }

    }
}
