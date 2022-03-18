using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Hero Info", menuName = "Data/Hero", order = 1)]
public class HeroInformation : InformationOrigin
{
    [SerializeField]
    private int dmg = 0;
    [SerializeField]
    private int hp = 0;
    [SerializeField]
    private int level = 0;


    [SerializeField] Sprite heroAvatar = null;
    [SerializeField] Sprite heroThumb = null;
    [SerializeField] Sprite heroPortrait = null;
    [SerializeField] Sprite heroSideIcon = null;


 
    public int Dmg
    {
        get
        {
            return dmg;
        }
    }
    public int Hp
    {
        get
        {
            return hp;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
    }

    public Sprite HeroAvartar
    {
        get
        {
            return heroAvatar;
        }
    }

}
