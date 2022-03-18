


using UnityEngine;

[System.Serializable]
public class EnemyUnitStatus
{
    public AIStateType aiStateType;
    public AIStateType originAiState;
    public DirEight originLookAt;
    public Node originNode;
    public EnemyBaseInformation enemyBaseInformation;

    public int hp;



    public void SetFullHP() {
        hp = enemyBaseInformation.maxHP;
    }


    
 
}
