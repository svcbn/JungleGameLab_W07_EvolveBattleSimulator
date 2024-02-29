using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BerserkerData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(BerserkerData))]
public class BerserkerData : PassiveSkillData
{
	[SerializeField]
	private float _speedUpRatio;

	[SerializeField]
	private float _cooldownRatio;
	
	[SerializeField]
	private float _powerUpRatio;


	public float SpeedUpRatio => _speedUpRatio;
	public float CooldownRatio => _cooldownRatio;
	public float PowerUpRatio => _powerUpRatio;
}
