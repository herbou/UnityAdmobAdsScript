using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;

public class Admob : MonoBehaviour
{
	private BannerView adBanner;
	private InterstitialAd adInterstitial;
	private RewardBasedVideoAd adReward;

	private string idApp, idBanner, idInterstitial, idReward;

	[SerializeField] Button BtnInterstitial;
	[SerializeField] Button BtnReward;
	[SerializeField] Text TxtPoints;


	void Start ()
	{
		BtnInterstitial.interactable = false;

		idApp = "ca-app-pub-3940256099942544~3347511713";
		idBanner = "ca-app-pub-3940256099942544/6300978111";
		idInterstitial = "ca-app-pub-3940256099942544/1033173712";
		idReward = "ca-app-pub-3940256099942544/5224354917";

		adReward = RewardBasedVideoAd.Instance;

		MobileAds.Initialize (idApp);

		RequestBannerAd ();
		RequestInterstitialAd ();
	}



	#region Banner Methods --------------------------------------------------

	public void RequestBannerAd ()
	{
		adBanner = new BannerView (idBanner, AdSize.Banner, AdPosition.Bottom);
		AdRequest request = AdRequestBuild ();
		adBanner.LoadAd (request);
	}

	public void DestroyBannerAd ()
	{
		if (adBanner != null)
			adBanner.Destroy ();
	}

	#endregion

	#region Interstitial methods ---------------------------------------------

	public void RequestInterstitialAd ()
	{
		adInterstitial = new InterstitialAd (idInterstitial);
		AdRequest request = AdRequestBuild ();
		adInterstitial.LoadAd (request);

		//attach events
		adInterstitial.OnAdLoaded += this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening += this.HandleOnAdOpening;
		adInterstitial.OnAdClosed += this.HandleOnAdClosed;
	}

	public void ShowInterstitialAd ()
	{
		if (adInterstitial.IsLoaded ())
			adInterstitial.Show ();
	}

	public void DestroyInterstitialAd ()
	{
		adInterstitial.Destroy ();
	}

	//interstitial ad events
	public void HandleOnAdLoaded (object sender, EventArgs args)
	{
		//this method executes when interstitial ad is Loaded and ready to show
		BtnInterstitial.interactable = true; //button is ready to click (enabled)
	}

	public void HandleOnAdOpening (object sender, EventArgs args)
	{
		//this method executes when interstitial ad is shown
		BtnInterstitial.interactable = false; //disable the button
	}

	public void HandleOnAdClosed (object sender, EventArgs args)
	{
		//this method executes when interstitial ad is closed
		adInterstitial.OnAdLoaded -= this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening -= this.HandleOnAdOpening;
		adInterstitial.OnAdClosed -= this.HandleOnAdClosed;

		RequestInterstitialAd (); //request new interstitial ad after close
	}

	#endregion

	#region Reward video methods ---------------------------------------------

	public void RequestRewardAd ()
	{
		AdRequest request = AdRequestBuild ();
		adReward.LoadAd (request, idReward);

		adReward.OnAdLoaded += this.HandleOnRewardedAdLoaded;
		adReward.OnAdRewarded += this.HandleOnAdRewarded;
		adReward.OnAdClosed += this.HandleOnRewardedAdClosed;
	}

	public void ShowRewardAd ()
	{
		if (adReward.IsLoaded ())
			adReward.Show ();
	}
	//events
	public void HandleOnRewardedAdLoaded (object sender, EventArgs args)
	{//ad loaded
		ShowRewardAd ();

	}

	public void HandleOnAdRewarded (object sender, EventArgs args)
	{//user finished watching ad
		int points = int.Parse (TxtPoints.text);
		points += 50; //add 50 points
		TxtPoints.text = points.ToString ();
	}

	public void HandleOnRewardedAdClosed (object sender, EventArgs args)
	{//ad closed (even if not finished watching)
		BtnReward.interactable = true;
		BtnReward.GetComponentInChildren <Text> ().text = "More Points";

		adReward.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
		adReward.OnAdRewarded -= this.HandleOnAdRewarded;
		adReward.OnAdClosed -= this.HandleOnRewardedAdClosed;
	}

	#endregion

	//other functions
	//btn (more points) clicked
	public void OnGetMorePointsClicked ()
	{
		BtnReward.interactable = false;
		BtnReward.GetComponentInChildren <Text> ().text = "Loading...";
		RequestRewardAd ();
	}

	//------------------------------------------------------------------------
	AdRequest AdRequestBuild ()
	{
		return new AdRequest.Builder ().Build ();
	}

	void OnDestroy ()
	{
		DestroyBannerAd ();
		DestroyInterstitialAd ();

		//dettach events
		adInterstitial.OnAdLoaded -= this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening -= this.HandleOnAdOpening;
		adInterstitial.OnAdClosed -= this.HandleOnAdClosed;


		adReward.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
		adReward.OnAdRewarded -= this.HandleOnAdRewarded;
		adReward.OnAdClosed -= this.HandleOnRewardedAdClosed;

	}

}
