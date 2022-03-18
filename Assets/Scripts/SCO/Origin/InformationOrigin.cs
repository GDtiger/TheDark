using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationOrigin : ScriptableObject
{
    [SerializeField]
    protected string _name;
    [SerializeField]
    protected int id;


    public virtual int GetCategory1() {
        return -1;
    }

    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public string Name {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
}
