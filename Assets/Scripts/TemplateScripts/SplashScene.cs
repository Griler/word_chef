using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SplashScene : MonoBehaviour {
	
	int appStartedNumber;
	AsyncOperation progress = null;
	Image progressBar;
	float myProgress=0;
	string sceneToLoad;
	
	void Start ()
	{


			sceneToLoad = "MainScene";



		
		progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
		if(PlayerPrefs.HasKey("appStartedNumber"))
		{
			appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
		}
		else
		{
			appStartedNumber = 0;
		}
		appStartedNumber++;
		PlayerPrefs.SetInt("appStartedNumber",appStartedNumber);
		StartCoroutine(LoadScene());
	}
	
	
	
	
	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds(5.05f);
		Application.LoadLevel(sceneToLoad);
		
		
	}
	
	void Update()
	{
		if(progress != null && progress.progress>0.49f)
		{
			progressBar.fillAmount = progress.progress;
		}
		
	}
}
