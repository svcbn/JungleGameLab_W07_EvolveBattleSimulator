using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillData
{
	public List<SkillInfo> Skills;
}

[Serializable]
public class SkillInfo : ISerializationCallbackReceiver
{
	// Required Data //
	public uint Id;

	public string Name;

	public string Description;

	public string SpriteName;

	// Optional Data (overwrite by serializer) //
	public SkillType SkillType;

	public float CoolDown;

	public float BeforeDelay;

	public Sprite Sprite;

	public ScriptableSkillData Data;

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		Sprite = Managers.Resource.Load<Sprite>($"Sprites/{SpriteName}");
		Data = Managers.Resource.Load<ScriptableSkillData>($"Data/{SpriteName}Data");
		if (Data != null)
		{
			SkillType = Data.Type;
			CoolDown = Data.Cooldown;

			if (Data is ActiveSkillData activeSkillData)
			{
				BeforeDelay = activeSkillData.BeforeDelay;
			}
		}
	}
}