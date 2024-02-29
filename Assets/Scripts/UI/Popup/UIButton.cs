using TMPro;
using UnityEngine.UI;

public class UIButton : UIPopup
{
	private enum ButtonType
	{
		BasicButton,
	}

	public override void Init()
	{
		Bind<Button, ButtonType>();
	}

	public void SetText(string text)
	{
		gameObject.FindChild<TextMeshProUGUI>().text = text;
	}
}
