using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Made By Anthony Romrell
public class MySceneManger : MonoBehaviour 
{
	public Image MyImage;
	public Color MyColor;

	private void Start()
	{
		MyColor = MyImage.color;
		SceneManager.sceneLoaded += StartFade;
	}

	public void LoadScene(string myScene)
	{
		SceneManager.LoadSceneAsync(myScene, LoadSceneMode.Additive);
		var newScene = SceneManager.CreateScene("New");
		SceneManager.SetActiveScene(newScene);
	}

	void StartFade(Scene arg0, LoadSceneMode loadSceneMode)
	{
		StartCoroutine(FadeOut());
	}

	
	public IEnumerator FadeOut()
	{
		
		while (MyImage.color.a > 0)
		{
			MyColor.a -= 0.01f; 
			MyImage.color = MyColor;
			yield return new WaitForEndOfFrame();
		}

		SceneManager.UnloadSceneAsync("StartScreen");
	}
}