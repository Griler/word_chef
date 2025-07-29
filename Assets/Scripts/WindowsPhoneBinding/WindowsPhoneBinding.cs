using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;



public class WindowsPhoneBinding : MonoBehaviour {

	
	
	
	
	public static event EventHandler ShowAdInterstitialHandler;
	public static event EventHandler ShowAdBannerHandler;
	public static event EventHandler HideAdBannerHandler;
	
	public static event EventHandler PurchaseInAppHandler;
	public static event EventHandler RestoreInAppHandler;
	public static event EventHandler RequestPricesHandler;
	
	public static WindowsPhoneBinding windowsPhoneBinding;

	public static Dictionary<string, float> allPrices;
	
	static WindowsPhoneBinding instance;
	
	public static WindowsPhoneBinding Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(WindowsPhoneBinding)) as WindowsPhoneBinding;
			
			return instance;
		}
	}
	
	void Awake()
	{
		name = "WindowsPhoneBinding";
		
		WindowsPhoneBinding.HideAdBanner ();
		
		if (windowsPhoneBinding == null)
		{
			DontDestroyOnLoad(gameObject);
			windowsPhoneBinding = this;
		}
		else if (windowsPhoneBinding != this)
		{
			Destroy(gameObject);
		}

		
		
		if (!PlayerPrefs.HasKey ("KupioDaNemaReklame"))
		{
			PlayerPrefs.SetInt ("KupioDaNemaReklame", 0);
		}
		
	}
	
	
	
	public static void ShowAdInterstitial()
	{
		int removeAds = PlayerPrefs.GetInt("KupioDaNemaReklame");
		
		if(removeAds == 0)
		{
			if(ShowAdInterstitialHandler != null)
			{
				ShowAdInterstitialHandler(null, null);
			}
		}
		
	}
	
	
	
	public static void ShowAdBanner()
	{
		int removeAds = PlayerPrefs.GetInt("KupioDaNemaReklame");
		
		if(removeAds == 0)
		{
			if(ShowAdBannerHandler != null)
			{
				ShowAdBannerHandler(null, null);
			}
		}
	}
	
	
	
	public static void HideAdBanner()
	{
		int removeAds = PlayerPrefs.GetInt("KupioDaNemaReklame");
		
		if(removeAds == 0)
		{
			if(HideAdBannerHandler != null)
			{
				HideAdBannerHandler(null, null);
			}
		}
	}

	
	
	public static void PurchaseInApp(string inAppId)
	{
		if(PurchaseInAppHandler != null)
		{
			PurchaseInAppHandler(inAppId, null);
		}
	}

	
	
	public static void InAppSuccessfullyPurchased(string inAppId)
	{
		Debug.Log("Inapp " + inAppId + "  successfully purchased!");
		GameObject.Find("Canvas/Panel/Message").GetComponent<Text>().text = "Inapp " + inAppId + "  successfully purchased!";
	}

	
	
	
	public static void RestoreInApp(string inAppId)
	{
		if(RestoreInAppHandler != null)
		{
			RestoreInAppHandler(inAppId, null);
		}
	}

	
	public static void InAppSuccessfullyRestored(string inAppId)
	{
		Debug.Log("Inapp " + inAppId + "  successfully restored!");
		GameObject.Find("Canvas/Panel/Message").GetComponent<Text>().text = "Inapp " + inAppId + "  successfully restored!";
	}

	
	public static void GetAllPrices()
	{
		if(RequestPricesHandler != null)
		{
			RequestPricesHandler(null, null);
		}
	}

	
	public static void SetAllPrices(string prices)
	{
		allPrices = new Dictionary<string, float>();

		string[] items = prices.Split(',');

		for (int i = 0; i < items.Length; i++)
		{
			allPrices.Add(items[i].Split('#')[0], float.Parse(items[i].Split('#')[1]));
		}

		GameObject.Find("Canvas/Panel/Message").GetComponent<Text>().text = "Prices successfully set!";
	}

	
	
	public static float GetSinglePrice(string inAppId)
	{
		return allPrices[inAppId];
	}

	
	public static void SinglePrice(string id)
	{
		GameObject.Find("Canvas/Panel/Message").GetComponent<Text>().text = "Cena za inapp:" + id + "   je  " + GetSinglePrice(id).ToString();
	}
}
