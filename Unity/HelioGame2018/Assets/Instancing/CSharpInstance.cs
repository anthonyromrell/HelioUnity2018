using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CSharpInstance : ScriptableObject
{
    public int Health = 100;
    public string PlayerName = "Bob";

    public void InstanceWork()
    {
        Debug.Log("Run this");
    }
}
