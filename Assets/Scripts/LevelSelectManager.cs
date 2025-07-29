using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour {

	public GameObject worldSelectMenu;
	public GameObject levelSelectMenu;
	public GameObject mainMenu;

	public GameObject loadingHolder;

	public GameObject shopMenu;

	private GameObject menuBeforeShopWasOpened;

	public MenuManager menuManager;

	public Text starNumberText;
	public Text[] coinsNumberTexts;

	public GameObject soundOffImageHolder;

	public List<GameObject> listOfNativeAds;

	
	public GameObject clicksBlocker;

	public static LevelSelectManager levelSelectManager;

	void Awake()
	{
		RefreshStarsAndCoins();

		if (GlobalVariables.backFromGameplay)
		{
			if (GlobalVariables.playLoadingDepartAtTheBegining)
			{
				StartCoroutine("LoadingDepartCoroutine");
			}
		}
			
		
		SoundManager.Instance.Play_MenuMusic();
		SoundManager.Instance.Stop_GameplayMusic();

		if (SoundManager.musicOn == 0)
			soundOffImageHolder.SetActive(true);

		levelSelectManager = this;
	}

	IEnumerator LoadingDepartCoroutine()
	{

		menuManager.ShowMenu(levelSelectMenu);
		LevelsParser.levelParser.SetWorldLevels(LevelsParser.selectedPack, LevelsParser.selectedWorld);

		loadingHolder.SetActive(true);

		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingDepart", 0, 0);

		yield return new WaitForSeconds(0.8f);

		loadingHolder.SetActive(false);
	}

	public void WorldSelected()
	{
		StartCoroutine("WorldSelectedCoroutine");	
	}

	IEnumerator WorldSelectedCoroutine()
	{
		clicksBlocker.SetActive(true);
		loadingHolder.SetActive(true);
		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingArrive", 0, 0);

		yield return new WaitForSeconds(0.5f);

		
		menuManager.ShowMenu(levelSelectMenu);

		LevelsParser.levelParser.SetWorldLevels(LevelsParser.selectedPack, LevelsParser.selectedWorld);

		yield return new WaitForSeconds(0.5f);

		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingDepart", 0, 0);

		yield return new WaitForSeconds(0.8f);

		loadingHolder.SetActive(false);
		clicksBlocker.SetActive(false);
	}

	public void ShowWorldSelectMenu()
	{
		if (!GlobalVariables.playLastLevel)
			StartCoroutine("ShowWorldselectMenuCoroutine");
		else
		{
			GlobalVariables.playLastLevel = false;

			LevelsParser.selectedPack = LevelsParser.levelParser.lastUnlockedPack;
			LevelsParser.selectedWorld = LevelsParser.levelParser.lastUnlockedWorld;
			LevelsParser.selectedLevel = LevelsParser.levelParser.lastUnlockedLevel;

			GlobalVariables.startInterstitialShown = true;

			LevelSelected();
		}
	}

	IEnumerator ShowWorldselectMenuCoroutine()
	{
		clicksBlocker.SetActive(true);
		loadingHolder.SetActive(true);
		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingArrive", 0, 0);

		yield return new WaitForSeconds(0.5f);

		
		menuManager.ShowMenu(worldSelectMenu);

		if (GameObject.Find("WorldPacksHolder").transform.childCount == 0)
			LevelsParser.levelParser.SetPackWorlds();
		else
			LevelsParser.levelParser.SetTabs(GameObject.Find("WorldPacksHolder").transform);

		yield return new WaitForSeconds(0.5f);

		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingDepart", 0, 0);

		yield return new WaitForSeconds(0.8f);

		loadingHolder.SetActive(false);
		clicksBlocker.SetActive(false);
	}

	public void LevelSelected()
	{
		StartCoroutine("LevelSelectedCoroutine");
	}

	IEnumerator LevelSelectedCoroutine()
	{
		clicksBlocker.SetActive(true);
		AsyncOperation async = SceneManager.LoadSceneAsync("Level");
		async.allowSceneActivation = false;

		loadingHolder.SetActive(true);
		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingArrive", 0, 0);

		GlobalVariables.playLoadingDepartAtTheBegining = true;

		yield return new WaitForSeconds(0.9f);

		async.allowSceneActivation = true;
	}

	public void OpenShop()
	{
		menuBeforeShopWasOpened = menuManager.currentMenu.gameObject;

		menuManager.ShowMenu(shopMenu);
	}

	public void CloseShop()
	{
		menuManager.ShowMenu(menuBeforeShopWasOpened);
	}

	public void BackButtonPressed()
	{
		StartCoroutine("BackButtonCoroutine");
	}

	IEnumerator BackButtonCoroutine()
	{
		clicksBlocker.SetActive(true);
		loadingHolder.SetActive(true);
		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingArrive", 0, 0);

		yield return new WaitForSeconds(0.5f);

		if (menuManager.currentMenu.gameObject == worldSelectMenu)
		{
			
			menuManager.ShowMenu(mainMenu);

			
			LevelsParser.selectedPack = LevelsParser.levelParser.lastUnlockedPack;
			LevelsParser.selectedWorld = LevelsParser.levelParser.lastUnlockedWorld;
			LevelsParser.selectedLevel = LevelsParser.levelParser.lastUnlockedLevel;
		}
		else if (menuManager.currentMenu.gameObject == levelSelectMenu)
		{
			
			menuManager.ShowMenu(worldSelectMenu);

			if (GameObject.Find("WorldPacksHolder").transform.childCount == 0)
				LevelsParser.levelParser.SetPackWorlds();
		}
			
		yield return new WaitForSeconds(0.5f);

		loadingHolder.transform.GetChild(0).GetComponent<Animator>().Play("LoadingDepart", 0, 0);

		yield return new WaitForSeconds(0.8f);

		loadingHolder.SetActive(false);
		clicksBlocker.SetActive(false);
	}

	
	public void RefreshStarsAndCoins()
	{
		
		starNumberText.text = GlobalVariables.stars.ToString();

		for (int i = 0; i < coinsNumberTexts.Length; i++)
		{
			coinsNumberTexts[i].text = GlobalVariables.coins.ToString();
		}
	}

	public void PlayButtonClickSound()
	{
		
		SoundManager.Instance.Play_Sound(SoundManager.Instance.buttonClick);
	}

	public void ToggleSound()
	{
		if (SoundManager.musicOn == 1)
		{
			SoundManager.musicOn = 0;
			SoundManager.soundOn = 0;
			PlayerPrefs.SetInt("SoundOn", 0);
			PlayerPrefs.SetInt("MusicOn", 0);
			PlayerPrefs.Save();

			soundOffImageHolder.SetActive(true);

			SoundManager.Instance.MuteAllSounds();
		}
		else
		{
			SoundManager.musicOn = 1;
			SoundManager.soundOn = 1;
			PlayerPrefs.SetInt("SoundOn", 1);
			PlayerPrefs.SetInt("MusicOn", 1);
			PlayerPrefs.Save();

			soundOffImageHolder.SetActive(false);

			SoundManager.Instance.UnmuteAllSounds();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (menuManager.popupOpened)
				menuManager.ClosePopUpMenu(menuManager.currentPopUpMenu.gameObject);
			else if (mainMenu.activeInHierarchy && !GlobalVariables.removeAdsOwned)
			{}

			else if (levelSelectMenu.activeInHierarchy || worldSelectMenu.activeInHierarchy)
				BackButtonPressed();
			else if (shopMenu.activeInHierarchy)
				CloseShop();
			else if (mainMenu.activeInHierarchy)
				Application.Quit();
		}
	}
}
