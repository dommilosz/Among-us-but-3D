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
        var rfpsc = this.GetComponentInParent<RigidbodyFirstPersonController>();
        if (rfpsc.movementSettings.ForwardSpeed > 0 && rfpsc.movementSettings.BackwardSpeed > 0 && rfpsc.movementSettings.StrafeSpeed > 0&& rfpsc.movementSettings.JumpForce>0)
        {
            backup_ForwardSpeed = rfpsc.movementSettings.ForwardSpeed;
            backup_BackwardSpeed = rfpsc.movementSettings.BackwardSpeed;
            backup_StrafeSpeed = rfpsc.movementSettings.StrafeSpeed;
            backup_JumpForce = rfpsc.movementSettings.JumpForce;
        }
        if (!canMove)
        {
            rfpsc.movementSettings.ForwardSpeed = 0;
            rfpsc.movementSettings.BackwardSpeed = 0;
            rfpsc.movementSettings.StrafeSpeed = 0;
            rfpsc.movementSettings.JumpForce = 0;
        }
        else
        {
            rfpsc.movementSettings.ForwardSpeed = backup_ForwardSpeed;
            rfpsc.movementSettings.BackwardSpeed = backup_BackwardSpeed;
            rfpsc.movementSettings.StrafeSpeed = backup_StrafeSpeed;
            rfpsc.movementSettings.JumpForce = backup_JumpForce;
        }
        
    }
}
