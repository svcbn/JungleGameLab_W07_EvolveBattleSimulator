using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameEnd : UIPopup
{
	private enum Buttons
	{
		ExitButton
	}
	
	private enum Texts
	{
		WinnerText
	}
	
	private TextMeshProUGUI _winnerText;

	public override void Init()
	{
		base.Init();
		
		Bind<Button, Buttons>();
		Bind<TextMeshProUGUI, Texts>();

		Get<Button>((int)Buttons.ExitButton).onClick.AddListener(OnClickExitButton);
		
		_winnerText = Get<TextMeshProUGUI>((int)Texts.WinnerText);
	}

	public void SetWinner(Character winner)
	{
		_winnerText.text = $"{winner.name} Win";
	}
	
	private void OnClickExitButton()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
