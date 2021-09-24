using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnLoad : MonoBehaviour
{
    public bool DestroyOn0Scene = true;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (DestroyOn0Scene)
        {
            gameObject.DontDestroyUntil0();
        }
    }
}
