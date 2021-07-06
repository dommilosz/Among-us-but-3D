using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform player;
    public float x_Offset, y_Offset, z_Offset;

    void Update()
    {
        transform.position = player.transform.position + new Vector3(x_Offset, y_Offset, z_Offset);
    }
}
