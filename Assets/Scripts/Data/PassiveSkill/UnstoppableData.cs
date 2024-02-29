using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(UnstoppableData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(UnstoppableData))]
public class UnstoppableData : PassiveSkillData
{
	[SerializeField]
	private float _speedUpRatio;

	public float SpeedUpRatio => _speedUpRatio;
}
