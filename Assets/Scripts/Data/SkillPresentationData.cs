using UnityEngine;

public class SkillPresentationData
{
	[SerializeField]
	private SkillInfo _info;

	[SerializeField]
	private ISkill _skill;

	public SkillInfo Info
	{
		get => _info;
		set => _info = value;
	}

	public ISkill Skill
	{
		get => _skill;
		set => _skill = value;
	}
}