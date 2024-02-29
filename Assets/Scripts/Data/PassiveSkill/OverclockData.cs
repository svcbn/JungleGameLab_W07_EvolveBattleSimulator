using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(OverclockData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(OverclockData))]
public class OverclockData : PassiveSkillData
{
	public int maxStack;
	public int bonusDamagePerStack;
	public int maxStackBonusMultiplier;
}
