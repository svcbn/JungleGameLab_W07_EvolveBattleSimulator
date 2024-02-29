using UnityEngine;

[CreateAssetMenu(fileName = nameof(SlashData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(SlashData))]
public class SlashData : ActiveSkillData
{
	[SerializeField] private int _percentageDamage;

	public int PercentageDamage => _percentageDamage;
}