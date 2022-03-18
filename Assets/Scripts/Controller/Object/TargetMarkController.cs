using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetMarkController : MonoBehaviour
{
    public List<EnemyController> enemyWhoFollowThisMarkForSee;
    public List<EnemyController> enemyWhoFollowThisMarkForSound;


    public void Initiate() { 
        
    }



    public void AddEnemy(EnemyController enemyController) {
        enemyWhoFollowThisMarkForSee.Add(enemyController);
        enemyController.targetMarkController = this;
    }

    public void RemoveEnemy(EnemyController enemyController) {
        if (enemyWhoFollowThisMarkForSee.Contains(enemyController))
        {
            enemyWhoFollowThisMarkForSee.Remove(enemyController);
        }
    }

    public void GobackToNormal() {
        foreach (var enemy in enemyWhoFollowThisMarkForSee)
        {
            enemy.ReturnToNormalAllLinkTargetMark();
        }
        Destroy(gameObject);
    }

    public void FindTargetAndTrackTarget(PlayerController playerController)
    {
        foreach (var enemy in enemyWhoFollowThisMarkForSee)
        {
            enemy.FoundPlayerAndSet(playerController);
        }
        Destroy(gameObject);
    }
}
