public class UIScene : UIBase
{
	public override void Init()
	{
		Managers.UI.SetCanvas(gameObject, false);
	}
}