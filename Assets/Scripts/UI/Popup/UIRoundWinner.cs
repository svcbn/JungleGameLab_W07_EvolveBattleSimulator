using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRoundWinner : UIPopup
{
	private enum Texts
	{
		WinnerText
	}
	
	private TextMeshProUGUI _winnerText;
	
	public override void Init()
	{
		base.Init();
		
		Bind<TextMeshProUGUI, Texts>();

		_winnerText = Get<TextMeshProUGUI>((int)Texts.WinnerText);
	}
	
	public void SetWinner(Character winner)
	{
		_winnerText.text = $"{winner.name} Win";
	}
}
