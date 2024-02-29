using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenu : UIPopup
{
	private enum Buttons
	{
		ExitButton,
		GoBackButton
	}

	public event Action ExitButtonClicked;
	
	public event Action GoBackButtonClicked;

	public override void Init()
	{
		base.Init();

		Bind<Button, Buttons>();

		RegisterButtonEvents();
	}

	private void RegisterButtonEvents()
	{
		foreach (Buttons buttonType in Enum.GetValues(typeof(Buttons)))
		{
			Button button = Get<Button>((int)buttonType);
			switch (buttonType)
			{
				case Buttons.ExitButton:
					button.onClick.AddListener(() => ExitButtonClicked?.Invoke());
					break;
				case Buttons.GoBackButton:
					button.onClick.AddListener(() => GoBackButtonClicked?.Invoke());
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}