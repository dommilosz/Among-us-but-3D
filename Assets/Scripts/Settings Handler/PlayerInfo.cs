using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerInfo : MonoBehaviour
{
    public bool isImpostor = false;
    public string color = Enums.Colors.RED;
    public VentScript VentStanding = null;
    public bool inVent = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float backup_ForwardSpeed;
    float backup_BackwardSpeed;
    float backup_StrafeSpeed;
    float backup_JumpForce;
    public void setCanMove(bool canMove)
    {
        var pm = this.GetComponentInParent<PlayerMovement>();
        if (pm.moveSpeed > 0 && pm.jumpForce > 0)
        {
            backup_ForwardSpeed = pm.moveSpeed;
            backup_JumpForce = pm.jumpForce;
        }
        if (!canMove)
        {
            pm.moveSpeed = 0;
            pm.jumpForce = 0;
        }
        else
        {
            pm.moveSpeed = backup_ForwardSpeed;
            pm.jumpForce = backup_JumpForce;
        }
        
    }
}
