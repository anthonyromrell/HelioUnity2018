using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Object")]
public class SendAndGetObject : ScriptableObject
{
    public object Object { get; private set; }

    public UnityEvent SendObject;

    private void OnEnable()
    {
        Object = null;
    }

    private void SendObjectWork(object obj)
    {
        Object = obj;
        SendObject.Invoke();
    }

    //Overloads
    public void GetObject(Transform obj)
    {
        SendObjectWork(obj);
    }

    public void GetObject(GameObject obj)
    {
        SendObjectWork(obj);
    }

    public void GetObject(ScriptableObject obj)
    {
        SendObjectWork(obj);
    }

    public void GetObject(string obj)
    {
        SendObjectWork(obj);
    }
}