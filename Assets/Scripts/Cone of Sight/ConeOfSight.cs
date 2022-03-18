using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Mesh mesh;
    [SerializeField] float fov = 90;
    [SerializeField] float viewDistance = 50;
    [SerializeField] Transform origin;
    [SerializeField] Transform drawMesh;
    [SerializeField] Vector3 offSet;

    [SerializeField] float startingAngle;
    [SerializeField] int rayCount = 50;

    private void Start()
    {
        mesh = new Mesh();
        drawMesh.GetComponent<MeshFilter>().mesh = mesh;
       // fov = 90f;
        //origin = Vector3.zero;
    }

    private void LateUpdate()
    {
        //transform.position = Vector3.zero;
        //transform.rotation = Quaternion.Euler(Vector3.zero);
        if (rayCount > 1)
        {
            float angle = startingAngle - ( fov / 2) + transform.rotation.y ;

            //Debug.Log($"{ transform.rotation.y}");

            float angleIncrease = fov / (rayCount - 1);

            Vector3[] vertices = new Vector3[rayCount + 1 + 1];
            Vector2[] uv = new Vector2[vertices.Length];
            int[] triangles = new int[rayCount * 3];

            vertices[0] = offSet;
            Vector3 pivot = origin.position + offSet;
            int vertexIndex = 1;
            int triangleIndex = 0;
            int maxTriangleIndex = rayCount - 1;
            for (int i = 0; i < rayCount; i++)
            {
                Vector3 vertex;
                RaycastHit raycastHit2D;

                if (Physics.Raycast(origin.position, UtilClass.GetVectorFromAngle(angle ), out raycastHit2D, viewDistance, layerMask))
                {
                    // Hit object
                    vertex = raycastHit2D.point - origin.position + offSet;
                }
                else
                {
                    // No hit
                    vertex = offSet + UtilClass.GetVectorFromAngle(angle ) * viewDistance;
                }
                Debug.DrawLine(offSet + origin.position, vertex);
                vertices[vertexIndex] = vertex;

                if (i < maxTriangleIndex)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 2] = vertexIndex;
                    triangles[triangleIndex + 1] = vertexIndex + 1;

                    triangleIndex += 3;
                }

                vertexIndex++;
                angle -= angleIncrease;
            }


            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
        
        //mesh.bounds = new Bounds(origin.position, Vector3.one * 1000f);
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.gameObject.name);
        Debug.Log($"Enter {other.gameObject.name}");

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exit {other.gameObject.name}");
    }

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = UtilClass.GetAngleFromVector(aimDirection) + fov / 2f;
    }

    public void SetFoV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }
}
