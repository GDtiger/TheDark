using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{


    public override void Dead()
    {
        gm.BossDied();
        Debug.Log($"{gameObject.name}Àº Á×À½");

    }
}
