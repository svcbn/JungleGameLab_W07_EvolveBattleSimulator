using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(IceHandsData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(IceHandsData))]
public class IceHandsData : PassiveSkillData
{
	[SerializeField]
	private float _speedDownRatio;

	[SerializeField]
	private float _effectDurationTime;


	public float SpeedDownRatio => _speedDownRatio;
	public float EffectDuration => _effectDurationTime;
}
