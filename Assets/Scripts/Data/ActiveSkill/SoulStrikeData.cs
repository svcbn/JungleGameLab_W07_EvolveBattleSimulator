using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SoulStrikeData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(SoulStrikeData))]
public class SoulStrikeData : ActiveSkillData
{
	[SerializeField] private int _percentageDamage;
	[SerializeField] private float _faintTime;

	public float FaintTime => _faintTime;
	public int PercentageDamage => _percentageDamage;
}
