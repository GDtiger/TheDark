using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class WaypointGroup
{
    public EnemyController enemyController;
    public Transform[] waypoints;
    public Color color = Color.white;

}