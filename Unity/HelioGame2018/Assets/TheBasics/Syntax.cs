using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.UI;

public class Syntax : MonoBehaviour
{

	//Value Types
	public string PlayerName;
	public int Score = 100; 
	public float Health = 1.0f;
	public bool On = true;

	//Reference Types
	public Players GamePlayers;

	public Text InpuText;
	
 	void Start ()
 	{
		 if (Score >= 0)
		 {
			 print(Score);
		 }

		 if (On)
		 {
			 print("On");
		 }
		 else
		 {
			 print("Off");
		 }
		 
		 
 		Loops();
 		
 	}

	void Loops()
	{
	
		foreach (var player in GamePlayers.PlayerNames)
		{
			print(player + " is playing.");
		}
		
	}

	
	public void AddPlayer()
	{
		GamePlayers.AddPlayers(InpuText.text);
	}
 }