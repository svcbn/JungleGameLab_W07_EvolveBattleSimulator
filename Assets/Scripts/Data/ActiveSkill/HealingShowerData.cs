using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(HealingShowerData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(HealingShowerData))]
public class HealingShowerData : ActiveSkillData
{
	[SerializeField] private int _amount;

	public int Amount => _amount;

	public float effectDuration = 0.5f; // 이펙트 지속시간
}
