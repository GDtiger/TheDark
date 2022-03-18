using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{

    public abstract void FSMEnter(PlayerController unitController);
    public abstract void FSMUpdate(PlayerController unitController);
    public abstract void FSMExit(PlayerController unitController);
}
