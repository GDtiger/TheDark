using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BridgeNodeCotroller : MonoBehaviour, IPointerClickHandler
{
    public Transform start;
    public Transform startNodeTrans;
    public Transform end;
    public Transform endNodeTrans;
    public Node startNode;
    public Node endNode;
    public bool inRange;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inRange)
        {

            Debug.Log($"{gameObject.name} in Range");

        }
        else
        {
            Debug.Log($"{gameObject.name} out Range");
        }
    }

    private void OnDrawGizmos()
    {
        if (start != null && end != null && startNode != null && endNode != null)
        {
            Gizmos.DrawWireSphere(start.position, 0.5f);
            Gizmos.DrawWireSphere(endNodeTrans.position, 0.5f);         
            Gizmos.DrawWireSphere(end.position, 0.5f);
            Gizmos.DrawWireSphere(startNodeTrans.position, 0.5f);
            Gizmos.DrawLine(startNodeTrans.position, start.position);
            Gizmos.DrawLine(start.position, end.position);
            Gizmos.DrawLine(end.position, endNodeTrans.position);
        }

    }
}
