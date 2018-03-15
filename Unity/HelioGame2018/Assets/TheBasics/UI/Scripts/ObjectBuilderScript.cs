using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made By Anthony Romrell
public class ObjectBuilderScript : MonoBehaviour
{
    public GameObject obj;
    public Vector3 spawnPoint;


    public void BuildObject()
    {
        Instantiate(obj, spawnPoint, Quaternion.identity);
    }
}