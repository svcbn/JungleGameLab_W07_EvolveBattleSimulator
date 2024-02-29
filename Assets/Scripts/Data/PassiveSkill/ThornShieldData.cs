using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ThornShieldData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(ThornShieldData))]
public class ThornShieldData : PassiveSkillData
{
	[SerializeField] private int _maxHp;
	[SerializeField] private float _damageScale;
	public int MaxHp => _maxHp;
	public float DamageScale => _damageScale;
}
