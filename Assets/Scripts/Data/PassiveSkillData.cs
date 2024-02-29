using UnityEngine;

public class PassiveSkillData : ScriptableSkillData
{
	[SerializeField] private int _presentNumber;
	
	[SerializeField] private bool _hasPresentNumber;

	public int PresentNumber => _presentNumber;
	
	public bool HasPresentNumber => _hasPresentNumber;
}
