using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager
{
	private SkillData _skillData;

	private Dictionary<Character, IReadOnlyList<ISkill>> _skills = new();

	private Dictionary<uint, Type> _skillCache = new();

	public void SetCharacters(params Character[] characters)
	{
		_skills.Clear();
		foreach (var character in characters)
		{
			_skills.Add(character, character.Skills);
		}
	}

	public void Init()
	{
		_skills.Clear();
		_skillCache.Clear();
		GetAllSkills();
		_skillData = Managers.Data.Load<SkillData>();
	}

	public List<SkillInfo> GeneratePool(int count)
	{
		List<SkillInfo> newSkillPool = new();

		int numOfAssignedSkill = Mathf.Min(_skillCache.Count, _skillData.Skills.Count);
		int totalCount = _skills.SelectMany(pool => pool.Value).Count();
		while (newSkillPool.Count < count)
		{
			if (newSkillPool.Count >= numOfAssignedSkill - totalCount)
			{
				break;
			}

			int random = UnityEngine.Random.Range(0, _skillData.Skills.Count);
			SkillInfo newSkill = _skillData.Skills[random];

			// 만약 어셈블리 내에 없는 스킬이면
			if (!_skillCache.TryGetValue(newSkill.Id, out _))
			{
				// 패스
				continue;
			}

			// 이미 누가 가지고 있는 스킬이면
			if (_skills.Any(pool => pool.Value.Any(skill => skill.Id == newSkill.Id)))
			{
				// 패스
				continue;
			}

			// 이미 리스트에 넣은 스킬이면
			if (newSkillPool.Contains(newSkill))
			{
				// 패스
				continue;
			}

			newSkillPool.Add(newSkill);
		}

		int garanteedAttackCount = 4;
		while (newSkillPool.Count(skill => skill.SkillType == SkillType.Attack) < garanteedAttackCount
			&& _skillData.Skills.Where(skill => skill.SkillType == SkillType.Attack).Count(newSkill => !_skills.Any(pool => pool.Value.Any(skill => skill.Id == newSkill.Id))) >= garanteedAttackCount)
		{
			newSkillPool = GeneratePool(count);
		}

		return newSkillPool;
	}

	public SkillInfo GetInfo(uint id)
	{
		return _skillData.Skills.Find(skill => skill.Id == id);
	}

	public bool TryFindSkillTypeById(uint id, out Type type) 
	{
		return _skillCache.TryGetValue(id, out type);
	}

	// Test
#if UNITY_EDITOR
	public ISkill GetSkill(uint id, Character owner)
	{
		if (!TryFindSkillTypeById(id, out var skillType))
		{
			Debug.LogError($"Undefined skill type. ID: {id}");
			return null;
		}

		var skill = owner.gameObject.AddComponent(skillType) as ISkill;
		skill.Owner = owner;
		skill.Init();
		owner.AddSkill(skill);

		return skill;
	}
#endif

	private void GetAllSkills()
	{
		var skillTypes = AppDomain.CurrentDomain.GetAssemblies().
			SelectMany(s => s.GetTypes()).
			Where(type => typeof(ISkill).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

		foreach (var type in skillTypes)
		{
			ScriptableSkillData data = Managers.Resource.Load<ScriptableSkillData>($"Data/{type.Name}Data");
			if (data == null)
			{
				Debug.LogWarning($"{type.Name}Data is not found.");
				continue;
			}
			_skillCache.Add(data.Id, type);
		} 
	}
}