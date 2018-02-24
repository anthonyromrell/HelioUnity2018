using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class FunctionsAndEvents : MonoBehaviour
{
    public static UnityAction DelagateEvent;

    private void Start()
    {
        DelagateEvent();
    }
}
