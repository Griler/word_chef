using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


public class AdsManager : MonoBehaviour {

	#region AdMob
	[Header("Admob")]
	public string adMobAppID = "";
	
	public string interstitalAdMobId = "ca-app-pub-3940256099942544/1033173712";
	
	public string videoAdMobId = "ca-app-pub-3940256099942544/5224354917";
	
	public string bannerAdMobId = "ca-app-pub-3940256099942544/6300978111";
	private BannerView bannerView;
	InterstitialAd interstitialAdMob;
	private RewardBasedVideoAd rewardBasedAdMobVideo; 
	AdRequest requestAdMobInterstitial, AdMobVideoRequest;
	#endregion
	[Space(15)]
	#region
	[Header("UnityAds")]
	public string unityAdsGameId;
	public string unityAdsVideoPlacementId = "rewardedVideo";
	#endregion

	static AdsManager instance;

	public static AdsManager Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(AdsManager)) as AdsManager;
			
			return instance;
		}
	}

	void Awake ()
	{
		gameObject.name = this.GetType().Name;
		DontDestroyOnLoad(gameObject);
		InitializeAds();
	}

	public void ShowInterstitial()
	{
		
	}

	public void IsVideoRewardAvailable()
	{
		if(isVideoAvaiable())
		{
			ShowVideoReward();
		}
		else
		{
			if (SceneManager.GetActiveScene().name == "MainScene")
				LevelSelectManager.levelSelectManager.menuManager.ShowPopUpMenu(Camera.main.GetComponent<ShopManager>().videoNotAvailablePopup);
			else
				GameplayManager.gameplayManager.menuManager.ShowPopUpMenu(Camera.main.GetComponent<ShopManager>().videoNotAvailablePopup);
		}
	}

	public void ShowVideoReward()
	{
		if(Advertisement.IsReady(unityAdsVideoPlacementId))
		{
			UnityAdsShowVideo();
		}
		else if(rewardBasedAdMobVideo.IsLoaded())
		{
			AdMobShowVideo();
		}
	}

	private void RequestInterstitial()
	{
		
		interstitialAdMob = new InterstitialAd(interstitalAdMobId);

		
		interstitialAdMob.OnAdLoaded += HandleOnAdLoaded;
		
		interstitialAdMob.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		
		interstitialAdMob.OnAdOpening += HandleOnAdOpened;
		
		interstitialAdMob.OnAdClosed += HandleOnAdClosed;
		
		interstitialAdMob.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		
		requestAdMobInterstitial = new AdRequest.Builder().Build();
		
		interstitialAdMob.LoadAd(requestAdMobInterstitial);
	}

	public void ShowAdMob()
	{
		if(interstitialAdMob.IsLoaded())
		{
			interstitialAdMob.Show();
		}
		else
		{
			interstitialAdMob.LoadAd(requestAdMobInterstitial);
		}
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
		interstitialAdMob.LoadAd(requestAdMobInterstitial);
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeftApplication event received");
	}

	private void RequestRewardedVideo()
	{
		
		rewardBasedAdMobVideo.OnAdLoaded += HandleRewardBasedVideoLoadedAdMob;
		
		rewardBasedAdMobVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadAdMob;
		
		rewardBasedAdMobVideo.OnAdOpening += HandleRewardBasedVideoOpenedAdMob;
		
		rewardBasedAdMobVideo.OnAdStarted += HandleRewardBasedVideoStartedAdMob;
		
		rewardBasedAdMobVideo.OnAdRewarded += HandleRewardBasedVideoRewardedAdMob;
		
		rewardBasedAdMobVideo.OnAdClosed += HandleRewardBasedVideoClosedAdMob;
		
		rewardBasedAdMobVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationAdMob;
		
		AdMobVideoRequest = new AdRequest.Builder().Build();
		
		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
	}

	public void HandleRewardBasedVideoLoadedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		
	}

	public void HandleRewardBasedVideoFailedToLoadAdMob(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);

	}

	public void HandleRewardBasedVideoOpenedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
	}

	public void HandleRewardBasedVideoStartedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
	}

	public void HandleRewardBasedVideoClosedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
	}

	public void HandleRewardBasedVideoRewardedAdMob(object sender, Reward args)
	{
		Camera.main.GetComponent<ShopManager>().AddCoinsAfterVideoWatched();
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);

	}

	public void HandleRewardBasedVideoLeftApplicationAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
	}

	void InitializeAds()
	{
		MobileAds.Initialize(adMobAppID);
		this.RequestBanner();
		this.rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
		this.RequestRewardedVideo();
		Advertisement.Initialize(unityAdsGameId);
		RequestInterstitial();
	}


	void AdMobShowVideo()
	{
		rewardBasedAdMobVideo.Show();	
	}

	void UnityAdsShowVideo()
	{
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResultUnity;

		Advertisement.Show(unityAdsVideoPlacementId, options);
	}

	void HandleShowResultUnity (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");
			Camera.main.GetComponent<ShopManager>().AddCoinsAfterVideoWatched();
			Advertisement.Initialize(unityAdsGameId);
		}else if(result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}else if(result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}

	bool isVideoAvaiable()
	{
		#if !UNITY_EDITOR
		if(Advertisement.IsReady(unityAdsVideoPlacementId))
		{
			return true;
		}
		else if(rewardBasedAdMobVideo.IsLoaded())
		{
			return true;
		}
		#endif
		return false;
	}

	private void RequestBanner()
	{
		
		bannerView = new BannerView(bannerAdMobId, AdSize.SmartBanner, AdPosition.Top);

		
		bannerView.OnAdLoaded += HandleBannerOnAdLoaded;
		
		bannerView.OnAdFailedToLoad += HandleBannerOnAdFailedToLoad;
		
		bannerView.OnAdOpening += HandleBannerOnAdOpened;
		
		bannerView.OnAdClosed += HandleBannerOnAdClosed;
		
		bannerView.OnAdLeavingApplication += HandleBannerOnAdLeavingApplication;

		
		AdRequest request = new AdRequest.Builder().Build();

		
		bannerView.LoadAd(request);
	}

	public void HandleBannerOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
		bannerView.Show();
	}

	public void HandleBannerOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
			+ args.Message);
	}

	public void HandleBannerOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleBannerOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
	}

	public void HandleBannerOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeftApplication event received");
	}
}
