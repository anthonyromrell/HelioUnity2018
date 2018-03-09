using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;


public abstract class GameActionHandler : MonoBehaviour
{
    public GameAction Action;
    
    private void Awake()
    {
        Action.Call += Respond;
    }

    protected abstract void Respond(object obj);

}