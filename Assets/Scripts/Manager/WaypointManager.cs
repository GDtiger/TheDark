using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] List<WaypointGroup> waypointGroups;
    [SerializeReference] public Dictionary<EnemyController, WaypointGroup> waypointDic;
    [SerializeField] float radius;
    [SerializeField] bool drawGizmo;


    #region instance
    static WaypointManager instance = null;

    public static WaypointManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WaypointManager>();
            }
            return instance;
        }
    }
    #endregion


    public void Initiate()
    {
        waypointDic = new Dictionary<EnemyController, WaypointGroup>();
        foreach (var item in waypointGroups)
        {
            waypointDic.Add(item.enemyController, item);
        }
    }

    private void OnDrawGizmos()
    {
        if (waypointGroups != null && drawGizmo)
        {
            for (int i = 0; i < waypointGroups.Count; i++)
            {
                if (waypointGroups[i].waypoints != null)
                {
                    Gizmos.color = waypointGroups[i].color;
                    for (int j = 0; j < waypointGroups[i].waypoints.Length; j++)
                    {
                        Gizmos.DrawWireSphere(waypointGroups[i].waypoints[j].position, radius);
                        if (waypointGroups[i].enemyController!= null)
                        {
                            Gizmos.DrawLine(waypointGroups[i].waypoints[j].position, waypointGroups[i].enemyController.transform.position);
                        }
                    }
                }

            }
        }
      
    }
}