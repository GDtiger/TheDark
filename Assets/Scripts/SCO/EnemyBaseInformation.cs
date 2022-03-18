using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Hero Info", menuName = "Data/Enemy", order = 1)]
public class EnemyBaseInformation : InformationOrigin
{

    public int damage;
    public int maxHP;
    [Header("최대 이동 거리")]
    public int maxMoveRange;

    [Header("감지 거리")]
    public int detectionRange;

    [Header("근처 아군 호출 거리")]
    public int callRange;


    public int attackRange;
    public float criticalChance;

    public Sprite heroAvatar = null;
    public Sprite heroThumb = null;
    public Sprite heroPortrait = null;
    public Sprite heroSideIcon = null;


}
