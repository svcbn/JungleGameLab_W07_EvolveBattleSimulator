using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : UIScene
{
	private enum Elements
	{
		TimerText
	}
	
	private TextMeshProUGUI _timerText;

	public override void Init()
	{
		base.Init();
		
		Bind<TextMeshProUGUI, Elements>();
		_timerText = Get<TextMeshProUGUI>((int)Elements.TimerText);
	}
	
	public void SetTimerText(string text)
	{
		if (text == null)
		{
			return;
		}
		
		_timerText.text = text;
	}
}
