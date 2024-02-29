using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(KillerBootsData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(KillerBootsData))]
public class KillerBootsData : PassiveSkillData
{
	[SerializeField]
	private float _speedUpRatio;

	[SerializeField]
	private float _effectDurationTime;

	public float SpeedUpRatio => _speedUpRatio;
	public float EffectDuration => _effectDurationTime;
}
