using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class CircleOverImage : MonoBehaviour
{
    public float radius;
    public int segments;
    public int thickness;
    LineRenderer lineRenderer;
    public Color color;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        Draw();
    }

    private void OnValidate()
    {
        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }

    void Draw()
    {
        if (material == null) return;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.sharedMaterial = new Material(material);
        lineRenderer.sharedMaterial.SetColor("_Color", color);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
        CreatePoints();
    }
}
