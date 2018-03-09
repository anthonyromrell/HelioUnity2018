using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Calculator
{
    [CreateAssetMenu]
    public class Number : ScriptableObject
    {

        public string Value;

        public UnityEvent<string> Event;

        public void SendNumber(string v)
        {
            Event.Invoke(v);
        }

      
    }
}