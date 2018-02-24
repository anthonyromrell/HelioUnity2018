using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSomethingAnything : MonoBehaviour {
	
	private void Awake()
	{
		FunctionsAndEvents.DelagateEvent += MyVeryCoolFunction;
	}

	private void MyVeryCoolFunction()
	{
		print("Hey this is very cool");
	}
}
