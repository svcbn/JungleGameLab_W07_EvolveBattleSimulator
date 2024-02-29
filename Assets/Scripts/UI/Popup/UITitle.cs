using UnityEngine.UI;
using UnityEngine;

public class UITitle : UIPopup
{
	private enum Buttons
	{
		StartButton,
		ExitButton
	}

	public System.Action StartButtonClicked;

	public System.Action ExitButtonClicked;

	public override void Init()
	{
		base.Init();

		Bind<Button, Buttons>();
		Get<Button>((int)Buttons.StartButton).onClick.AddListener(OnStartButtonClick);
		Get<Button>((int)Buttons.ExitButton).onClick.AddListener(OnExitButtonClick);
	}

	private void OnStartButtonClick()
	{
		StartButtonClicked?.Invoke();
	}

	private void OnExitButtonClick()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
