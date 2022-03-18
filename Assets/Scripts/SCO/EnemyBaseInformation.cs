using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Hero Info", menuName = "Data/Enemy", order = 1)]
public class EnemyBaseInformation : InformationOrigin
{

    public int damage;
    public int maxHP;
    [Header("�ִ� �̵� �Ÿ�")]
    public int maxMoveRange;

    [Header("���� �Ÿ�")]
    public int detectionRange;

    [Header("��ó �Ʊ� ȣ�� �Ÿ�")]
    public int callRange;


    public int attackRange;
    public float criticalChance;

    public Sprite heroAvatar = null;
    public Sprite heroThumb = null;
    public Sprite heroPortrait = null;
    public Sprite heroSideIcon = null;


}
