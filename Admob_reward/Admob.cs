using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
//reward ad

public class Admob : MonoBehaviour
{
	private RewardBasedVideoAd adReward;

	private string idApp, idReward;

	[SerializeField] Button BtnReward;
	[SerializeField] Text TxtPoints;


	void Start ()
	{
		idApp = "ca-app-pub-3940256099942544~3347511713";
		idReward = "ca-app-pub-3940256099942544/5224354917";

		adReward = RewardBasedVideoAd.Instance;

		MobileAds.Initialize (idApp);
	}


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
		adReward.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
		adReward.OnAdRewarded -= this.HandleOnAdRewarded;
		adReward.OnAdClosed -= this.HandleOnRewardedAdClosed;
	}

}
