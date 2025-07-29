using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour {

	public Text removeAdsPriceText;
	public Text smallPackPriceText;
	public Text mediumPackPriceText;
	public Text bigPackPriceText;

	
	public GameObject mainScreenCoinsHolder;
	public GameObject mainScreenStarsHolder;
	public GameObject levelSelectButtonsHolder;
	public GameObject worldSelectButtonsHolder;
	public GameObject mainSceneShopBackButtonHolder;
	public GameObject mainSceneShopCoinsHolder;

	
	public GameObject levelSceneCoinsHolder;
	public GameObject pauseButtonHolder;
	public GameObject levelSceneShopBackButtonHolder;
	public GameObject levelSceneShopCoinsHolder;

	
	public GameObject addCoinsAnimationHolder;

	
	public GameObject videoNotAvailablePopup;























	public void AddCoinsAnimation()
	{
		StartCoroutine("AddCoinsCoroutine");
	}

	IEnumerator AddCoinsCoroutine()
	{
		addCoinsAnimationHolder.SetActive(true);


		yield return new WaitForSeconds(2f);

		if (SceneManager.GetActiveScene().name == "MainScene")
			LevelSelectManager.levelSelectManager.RefreshStarsAndCoins();
		else if (SceneManager.GetActiveScene().name == "Level")
		{
			GameplayManager.gameplayManager.coinsText.text = GlobalVariables.coins.ToString();
			GameplayManager.gameplayManager.coinsTextShop.text = GlobalVariables.coins.ToString();
		}

		addCoinsAnimationHolder.SetActive(false);
	}

	public void WatchVideoForCoins()
	{
		AdsManager.Instance.IsVideoRewardAvailable();
	}

	public void AddCoinsAfterVideoWatched()
	{
		
		GlobalVariables.globalVariables.AddCoins(30);
		addCoinsAnimationHolder.transform.Find("AnimationHolder/CoinsHolder/CoinsNumberTextShop").GetComponent<Text>().text = "+30";
		AddCoinsAnimation();
	}


}
