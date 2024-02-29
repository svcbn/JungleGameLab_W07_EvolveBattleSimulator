using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SolarCloakData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(SolarCloakData))]
public class SolarCloakData : PassiveSkillData
{
	[Header("HitBox")]
	[SerializeField] private Vector2 _hitBoxCenter;
	[SerializeField] private Vector2 _hitBoxSize;

	[SerializeField] GameObject _defaultEffect;
	[SerializeField] private int _maxHp;
	[SerializeField] private float _damageScale;

	public Vector2 HitBoxCenter => _hitBoxCenter;
	public Vector2 HitBoxSize => _hitBoxSize;
	public GameObject DefaultEffect => _defaultEffect;
	public int MaxHp => _maxHp;
	public float DamageScale => _damageScale;
}
