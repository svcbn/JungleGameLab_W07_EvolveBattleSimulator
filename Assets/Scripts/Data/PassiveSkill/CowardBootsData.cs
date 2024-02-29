using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CowardBootsData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(CowardBootsData))]
public class CowardBootsData : PassiveSkillData
{
	[SerializeField]
	private float _speedUpRatio;

	[SerializeField]
	private float _effectDurationTime;

	public float SpeedUpRatio => _speedUpRatio;
	public float EffectDuration => _effectDurationTime;
}
