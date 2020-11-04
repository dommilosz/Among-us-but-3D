
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class PlayerMove : MonoBehaviour 
{ 
    public CharacterController controller; 
    public Transform groundCheck; 
    public float groundDistanse = 0.4f; 
    public LayerMask groundMask; 
    public float speed = 12f; 
    public float graviti = -9.81f; 
    
    Vector3 velocity; 
    bool isGrounded; 
    
    void Update() 
    { 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistanse, groundMask); 
        if(isGrounded && velocity.y < 0)
        { 
            velocity.y = -2f; 
        } 
        
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        
        Vector3 move = transform.right * x + transform.forward * z; 
        controller.Move(move * speed * Time.deltaTime); 
        velocity.y += graviti * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime); 
     } 
}  