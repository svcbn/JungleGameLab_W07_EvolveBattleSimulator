using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BloodStrikeData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(BloodStrikeData))]
public class BloodStrikeData : ActiveSkillData
{
	[SerializeField] private int _amount;

	public int Amount => _amount;
}
