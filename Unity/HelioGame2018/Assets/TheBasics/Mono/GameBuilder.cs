using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBuilder : MonoBehaviour
{
	public Game MyGame;
	private GameObject _myInstance;
	
	void Start ()
	{
		_myInstance  = Instantiate(MyGame.Ammo) as GameObject;
		_myInstance.transform.position = MyGame.Position;
	}

	private void Update ()
	{
		MyGame.Position = _myInstance.transform.position;
		SaveData();
	}
	
	void SaveData()
	{
		PlayerPrefs.SetString("GameData", JsonUtility.ToJson(MyGame));	
		
		Debug.Log(PlayerPrefs.GetString("GameData"));
	}
}