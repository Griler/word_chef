using UnityEngine;
using System.Collections;
using UnityEngine.UI;

	
public class MenuManager : MonoBehaviour 
{
	
	public Menu currentMenu;
	public Menu currentPopUpMenu;


	public GameObject[] disabledObjects;
	GameObject ratePopUp, crossPromotionInterstitial;

	public bool popupOpened;
	
	void Start () 
	{
		if(Application.loadedLevelName=="MainScene")
		{
			crossPromotionInterstitial = GameObject.Find("PopUps/PopUpInterstitial");
			ratePopUp = GameObject.Find("PopUps/PopUpRate");
		}

		if (disabledObjects!=null) {
			for(int i=0;i<disabledObjects.Length;i++)
			{
				disabledObjects[i].SetActive(false);
			}
		}
		
		if(Application.loadedLevelName!= "MapScene")
			ShowMenu(currentMenu.gameObject);	

		
		if (!GlobalVariables.backFromGameplay)
		{
			if(Application.loadedLevelName=="MainScene")
			{
				
				
				if(PlayerPrefs.HasKey("alreadyRated"))
				{
					Rate.alreadyRated = PlayerPrefs.GetInt("alreadyRated");
				}
				else
				{
					Rate.alreadyRated = 0;
				}
				
				if(Rate.alreadyRated==0)
				{
					Rate.appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
					Debug.Log("appStartedNumber "+Rate.appStartedNumber);
					
					if(Rate.appStartedNumber>=6)
					{
						Rate.appStartedNumber=0;
						PlayerPrefs.SetInt("appStartedNumber",Rate.appStartedNumber);
						PlayerPrefs.Save();
						ShowPopUpMenu(ratePopUp);
						
					}

				}

			}
		}
	}
	
	
	
	
	
	public void EnableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (!gameObject.activeSelf) 
			{
				gameObject.SetActive (true);
			}
		}
	}

	
	
	
	
	public void DisableObject(GameObject gameObject)
	{
		Debug.Log("Disable Object");
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}
	
	
	
	
	
	public void LoadScene(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevel (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}
	
	
	
	
	
	public void LoadSceneAsync(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevelAsync (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}

	
	
	
	
	public void ShowMenu(GameObject menu)
	{
		if (currentMenu != null && menu != currentMenu.gameObject)
		{
			currentMenu.IsOpen = false;

			StartCoroutine(CloseMenuCoroutine(currentMenu.gameObject));
		}
		
		currentMenu = menu.GetComponent<Menu> ();
		menu.gameObject.SetActive (true);
		currentMenu.IsOpen = true;
	}

	
	
	
	
	public void CloseMenu(GameObject menu)
	{
		if (menu != null) 
		{
			menu.GetComponent<Menu> ().IsOpen = false;
			menu.SetActive (false);
		}
	}

	IEnumerator CloseMenuCoroutine(GameObject menu)
	{
		yield return new WaitForSeconds(0.34f);

		menu.SetActive(false);
	}

	
	
	
	
	public void ShowPopUpMenu(GameObject menu)
	{
		if (!popupOpened)
		{
			menu.gameObject.SetActive (true);
			currentPopUpMenu = menu.GetComponent<Menu> ();
			currentPopUpMenu.IsOpen = true;

			popupOpened = true;

			SoundManager.Instance.Play_Sound(SoundManager.Instance.showPopup);
		}
	}

	
	
	
	
	public void ClosePopUpMenu(GameObject menu)
	{
		StartCoroutine("HidePopUp",menu);
	}

	
	
	
	
	IEnumerator HidePopUp(GameObject menu)
	{
		SoundManager.Instance.Play_Sound(SoundManager.Instance.hidePopup);
		menu.GetComponent<Menu> ().IsOpen = false;
		yield return new WaitForSeconds(1.2f);

		popupOpened = false;

		menu.SetActive (false);

		currentPopUpMenu = null;
	}

	
	
	
	
	public void ShowMessage(string message)
	{
		Debug.Log(message);
	}

	
	
	
	
	
	public void ShowPopUpMessage(string messageTitleText, string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);

	}

	
	
	
	
	public void ShowPopUpMessageTitleText(string messageTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
	}

	
	
	
	
	public void ShowPopUpMessageCustomMessageText(string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	
	
	
	
	
	public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	
	
	
	
	public void ShowPopUpDialogTitleText(string dialogTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
	}

	
	
	
	
	public void ShowPopUpDialogCustomMessageText(string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	public void StartLoading()
	{
		transform.Find("LoadingMenu").gameObject.SetActive(true);
		transform.Find("LoadingMenu/AnimationHolder").GetComponent<Animator>().Play("Arriving");
	}
}
