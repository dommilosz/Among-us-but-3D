using UnityEngine;
using UnityEngine.EventSystems;

public class IsMouseOn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isMouseOn = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOn = false;
    }
}
