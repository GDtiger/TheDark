




using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{

    public abstract void FSMEnter(EnemyController unitController);
    public abstract void FSMUpdate(EnemyController unitController);

    public abstract void FSMExit(EnemyController unitController);
}
