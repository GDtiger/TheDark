using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarSceipt : MonoBehaviour
{
    public GameObject fogOfwarPlane;
    public Transform player;
    public LayerMask fogLayer;
    public float radius = 5f;
    private float radiusSqr { get { return radius * radius; } }

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    void Start()
    {
        Initialized();
    }

    
    void Update()
    {
        Ray ray = new Ray(transform.position, player.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, fogLayer, QueryTriggerInteraction.Collide))
        {
            for (int i = 0; i < vertices.Length; ++i)
            {
                Vector3 v = fogOfwarPlane.transform.TransformPoint(vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < radiusSqr)
                {
                    float alpha = Mathf.Min(colors[i].a, dist / radiusSqr);
                    colors[i].a = alpha;

                }
            }
            UpdateColor();
        }
    }

    private void Initialized()//메쉬 정점에 대한 참조
    {
        mesh = fogOfwarPlane.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length]; //매쉬 색상 배열
        for (int i = 0; i < colors.Length; ++i)
        {
            colors[i] = Color.black;
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        mesh.colors = colors;
        
    }

}
