using UnityEngine;
using System.Collections;
using UnityEngine.UI;

  
public class Rate : MonoBehaviour {

	string rateURL;
	[Header("Linkovi za RATE")]
	public string rateUrlAndroid;
	public string rateUrlIOS;
	public string rateUrlWinPhone;
	public string rateUrlWinStore;
	public string rateUrlMAC;
	public static int appStartedNumber,alreadyRated;
	bool rateClicked = false;

	
	void Start () {

		#if UNITY_ANDROID
		rateURL = rateUrlAndroid;
		#elif UNITY_IOS
		rateURL = rateUrlIOS;
		#elif (UNITY_WP8 || UNITY_WP8_1)
		rateURL = rateUrlWinPhone;
		#elif (UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
		rateURL = rateUrlWinStore;
		#elif UNITY_STANDALONE_OSX
		rateURL = rateUrlMAC;
		#endif
	}

	
	
	
	
	public void RateClicked(int number)
	{
		if(!rateClicked)
		{
			SoundManager.Instance.Play_ButtonClick();

			alreadyRated = 1;
			PlayerPrefs.SetInt("alreadyRated",alreadyRated);
			PlayerPrefs.Save();
			rateClicked=true;
			StartCoroutine("ActivateStars",number);
		}
	}

	
	
	
	
	IEnumerator ActivateStars(int number)
	{
		switch(number)
		{
		case 1: case 2: case 3:
			for(int i=1;i<=number;i++)
			{
				GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG"+i+"/Star"+i).GetComponent<Image>().enabled = true;
			}
			yield return new WaitForSeconds(0.5f);
			HideRateMenu(GameObject.Find("PopUpRate"));
			break;
		case 4: case 5:
			for(int i=1;i<=number;i++)
			{
				GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG"+i+"/Star"+i).GetComponent<Image>().enabled = true;
			}
			Application.OpenURL(rateURL);
			yield return new WaitForSeconds(0.5f);
			HideRateMenu(GameObject.Find("PopUpRate"));
			yield return new WaitForSeconds(0.5f);

			break;
		}
		yield return null;
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();

	}

	
	
	
	public void ShowRateMenu()
	{
		transform.GetComponent<Animator>().Play("Open");
	}

	
	
	
	
	public void HideRateMenu(GameObject menu)
	{
		SoundManager.Instance.Play_ButtonClick();
		GameObject.Find("Canvas").GetComponent<MenuManager>().ClosePopUpMenu(menu);
	}

	
	
	
	
	public void NoThanks()
	{
		SoundManager.Instance.Play_ButtonClick();
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();
		HideRateMenu(GameObject.Find("PopUpRate"));
	}
}
