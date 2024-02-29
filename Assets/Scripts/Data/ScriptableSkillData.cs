using UnityEngine;

public class ScriptableSkillData : ScriptableObject
{
	[SerializeField] private uint _id;
	[SerializeField] private SkillType _type;
	[SerializeField] private float _cooldown;
	[SerializeField] private GameObject _effect;


	public uint Id => _id;
	public SkillType Type => _type;
	public float Cooldown => _cooldown;
	public GameObject Effect => _effect;
}