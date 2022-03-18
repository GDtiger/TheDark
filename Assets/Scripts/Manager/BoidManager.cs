using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public Dictionary<EnemyController,bool> allEnemy;

    public List<BoidGroup> boidGroups;


    public Vector3 gizmoLineOffset;
    public bool cycle;

    #region instance
    static BoidManager instance = null;
    public static BoidManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BoidManager>();
            }
            return instance;
        }
    }
    #endregion

    public void Start()
    {
        allEnemy = new Dictionary<EnemyController, bool>();
        foreach (var boid in boidGroups)
        {
            foreach (var enemy in boid.allEnemyInBoid)
            {
                if (!allEnemy.ContainsKey(enemy))
                {
                    allEnemy.Add(enemy,true);
                }
                foreach (var otherEnemy in boid.allEnemyInBoid)
                {
                    if (otherEnemy != enemy)
                    {
                        enemy.friendEnemy.Add(otherEnemy);
                    }
                    
                }

            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var boid in boidGroups)
        {
            var count = boid.allEnemyInBoid.Count - 1;
            Gizmos.color = boid.color;
            for (int i = 0; i < count; i++)
            {
                Gizmos.DrawLine(boid.allEnemyInBoid[i].transform.position + gizmoLineOffset, boid.allEnemyInBoid[i + 1].transform.position + gizmoLineOffset);
            }
            if (cycle)
            {
                Gizmos.DrawLine(boid.allEnemyInBoid[0].transform.position + gizmoLineOffset, boid.allEnemyInBoid[boid.allEnemyInBoid.Count - 1].transform.position + gizmoLineOffset);
            }

        }
    }
}


[System.Serializable]
public class BoidGroup
{
    public List<EnemyController> allEnemyInBoid;
    public Color color = Color.white;
}
