using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
{
	private float _currentCooldown;
	
	private Character _owner;
	
	public uint Id { get; protected set; }
	
	public SkillType SkillType { get; protected set; }

	public Character Owner
	{
		get => _owner;
		set
		{
			if (_owner != null && _owner != value)
			{
				throw new InvalidOperationException($"Owner is already set. Owner: {_owner.name}");
			}

			_owner = value;
		}
	}

	public float Cooldown { get; protected set; }

	public float CurrentCooldown
	{
		get => _currentCooldown;
		protected set
		{
			_currentCooldown = value;
			RaisePropertyChanged();
		}
	}
	
	public event PropertyChangedEventHandler PropertyChanged;

	public virtual void Init()
	{
	}
	
	public virtual void Reset()
	{
	}

	protected virtual T LoadData<T>()
		where T : ScriptableSkillData
	{
		T data = Managers.Resource.Load<T>($"Data/{typeof(T).Name}");
		
		if (data == null)
		{
			Debug.LogWarning($"{name} is not found");
			return null;
		}

		Id = data.Id;
		Cooldown  = data.Cooldown;
		SkillType = data.Type;

		return data;
	}
	
	protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new(propertyName));
	}
}