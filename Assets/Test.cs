using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float angle;
    public Transform target;
    public bool useY;
    public Vector3 vector3;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * Time.deltaTime;
        }

        Vector3 dir = target.position - transform.position;
        if (useY)
        {
            dir.y = 0;
        }
        angle = UtilClass.GetAngleFromVector(dir);
        vector3 = UtilClass.GetVectorFromAngle(angle);
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
}
