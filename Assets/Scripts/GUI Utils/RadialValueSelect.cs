using UnityEngine;

public class RadialValueSelect : MonoBehaviour
{
    public GameObject Wheel; //the thing you're trying to rotate
    Vector2 dir;
    float dist;

    public float value;
    [Range(0, 360)]
    public float rangeMin = 0;
    [Range(0, 360)]
    public float rangeMax = 360;

    public bool isRotating;
    public float angle;

    public float minDist;
    public float maxDist;
    [Range(0, 360)]
    public float shift;

    public bool VisualizeInPlayMode;
    public CircleOverImage minVisualizer;
    public CircleOverImage maxVisualizer;

    void Update()
    {
        //is rotating is set true if mouse is down on the handle
        if (isRotating)
        {
            //Vector from center to mouse pos
            dir = (Input.mousePosition - Wheel.transform.position);
            //Distance between mouse and the center
            dist = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
            //if mouse is not outside nor too inside the wheel
            if (dist < maxDist && dist > minDist)
            {
                angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg; //alien technology
                angle = (angle > 0) ? angle : angle + 360; //0 to 360 instead of -180 to 180

                value = angle + shift;
                value = value % 360;
                if (value > rangeMax) { value = rangeMax; angle = value - shift; }
                if (value < rangeMin) { value = rangeMin; angle = value - shift; }
                if (angle < 0) angle = 360 - angle;
                Wheel.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.back);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            BeginDrag();
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void Start()
    {
        if (VisualizeInPlayMode)
            DrawVisualizers();
    }

    public void BeginDrag()
    {
        isRotating = true;
    }
    public void EndDrag()
    {
        isRotating = false;
    }

    public void DrawVisualizers()
    {
        minVisualizer.radius = minDist;
        maxVisualizer.radius = maxDist;
    }

    private void OnValidate()
    {
        DrawVisualizers();
    }
}
