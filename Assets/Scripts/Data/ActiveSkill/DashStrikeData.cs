using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DashStrikeData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(DashStrikeData))]
public class DashStrikeData : ActiveSkillData
{
	public float dashDistance;
	public float faintDuration;
}
