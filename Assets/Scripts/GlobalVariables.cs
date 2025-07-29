using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

	public static string applicationID;

	public static int stars;
	public static int coins;

	public static int hintCost;

	public static bool playLoadingDepartAtTheBegining;

	
	public static bool backFromGameplay;

	
	public static bool playLastLevel;

	public static int bonusWordsRequired;
	public static int bonusWordsCollected;
	public static int bonusCoinsAmount;

	public static bool removeAdsOwned = true;

	
	public static string removeAdsPrice = "";
	public static string smallCoinsPackPrice = "";
	public static string mediumCoinsPackPrice = "";
	public static string bigCoinsPackPrice = "";
	public static string defaultRemoveAdsPrice = "1.99$";
	public static string defaultSmallCoinsPackPrice = "0.99$";
	public static string defaultMediumCoinsPackPrice = "1.99$";
	public static string defaultBigCoinsPackPrice = "4.99$";

	public static bool startInterstitialShown;

	public static GlobalVariables globalVariables;

	void Awake()
	{
		startInterstitialShown = false;
		playLoadingDepartAtTheBegining = false;
		backFromGameplay = false;
		playLastLevel = true;

		applicationID = "com.Word.Building.Puzzle.Games";

		hintCost = 25;

		if (!PlayerPrefs.HasKey("Stars"))
		{
			PlayerPrefs.SetInt("Stars", 0);
			stars = 0;
		}
		else
		{
			stars = PlayerPrefs.GetInt("Stars");
		}

		if (!PlayerPrefs.HasKey("Coins"))
		{
			PlayerPrefs.SetInt("Coins", 100);
			coins = 100;
		}
		else
		{
			coins = PlayerPrefs.GetInt("Coins");
		}

		if (!PlayerPrefs.HasKey("BonusWordsRequired"))
		{
			PlayerPrefs.SetInt("BonusWordsRequired", 4);
			PlayerPrefs.SetInt("BonusWordsCollected", 0);
			PlayerPrefs.SetInt("BonusCoinsAmount", 5);
			PlayerPrefs.Save();

			bonusWordsRequired = 4;
			bonusWordsCollected = 0;
			bonusCoinsAmount = 5;
		}
		else
		{
			bonusWordsRequired = PlayerPrefs.GetInt("BonusWordsRequired");
			bonusWordsCollected = PlayerPrefs.GetInt("BonusWordsCollected");
			bonusCoinsAmount = PlayerPrefs.GetInt("BonusCoinsAmount");
		}




		globalVariables = this;
		DontDestroyOnLoad(this);
	}

	public void AddCoins(int value)
	{
		coins += value;
		PlayerPrefs.SetInt("Coins", coins);
		PlayerPrefs.Save();
	}
}