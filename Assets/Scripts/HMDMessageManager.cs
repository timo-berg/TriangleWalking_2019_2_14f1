using TMPro;
using UnityEngine;



public class HMDMessageManager : Singleton<HMDMessageManager>
{
	#region fields

		public TextMeshProUGUI messageText;
		public GameObject canvas;

		public Color normalColor;
		public Color warningColor;
		public Color errorColor;

		Follow follow;
		float targetShowDuration;
		float showingTime;
		bool isShowingText;

	#endregion



	protected override void Awake()
	{	
		base.Awake();

		normalColor = messageText.color;

		follow = GetComponent<Follow>();

		HideMessage();
	}


	
	public void HideMessage()
	{
		canvas.SetActive(false);
		messageText.text = "";
		isShowingText = false;
	}



	public void ShowError(string text, float duration = -1)
	{
		ShowText(text, duration, errorColor);
	}



	public void ShowMessage(string text, float duration = -1)
	{
		// print("\tOnScreen: " + text.Replace('\n', ' '));

		ShowText(text, duration, normalColor);
	}



	public void ShowWarning(string text, float duration = -1)
	{
		ShowText(text, duration, warningColor);
	}



	void ShowText(string text, float duration, Color color)
	{
		if ( false == isShowingText )
			follow.AppearAtTargetPos();

		targetShowDuration = duration;
		showingTime = 0;
		isShowingText = true;

		canvas.SetActive(true);
		messageText.text = text;
		messageText.color = color;
	}



	void Update()
	{
		if ( false == isShowingText )
			return;
	
		showingTime += Time.deltaTime;
		if ( targetShowDuration > 0 && showingTime > targetShowDuration )
			HideMessage();
	}

}
