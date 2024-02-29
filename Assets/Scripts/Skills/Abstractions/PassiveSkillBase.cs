using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class PassiveSkillBase : SkillBase, IPassiveSkill
{
	private int _presentNumber;
	
	private bool _isEnabled;
	

	public bool IsEnabled
	{
		get => _isEnabled;

		set
		{
			_isEnabled = value;
			RaisePropertyChanged();
		}
	}

	public bool HasPresentNumber { get; protected set; }

	public int PresentNumber
	{
		get => _presentNumber;
		
		protected set
		{
			_presentNumber = value;
			RaisePropertyChanged();
		}
	}

	protected override T LoadData<T>()
	{
		T data = base.LoadData<T>();
		if (data is PassiveSkillData passiveSkillData)
		{
			HasPresentNumber = passiveSkillData.HasPresentNumber;
			PresentNumber   = passiveSkillData.PresentNumber;
		}

		return data;
	}

	protected void PlayEffect(string effName, float duration, Transform parent, Vector2 offset, float sign = 1)
	{
		StartCoroutine(PlayEffectCo(effName, duration, parent, offset, sign));
	}

	IEnumerator PlayEffectCo(string effName, float duration, Transform parent, Vector2 offset, float sign)
	{
		GameObject effect = Managers.Resource.Instantiate("Skills/"+effName, parent ); // paraent를 character.gameObject로

		if (effect)
		{
			effect.transform.localPosition = Vector3.zero;
		}
		else
		{
			Debug.LogError($"effect is null. effName :{effName}");
		}

		effect.transform.localPosition += (Vector3)offset;

        // todo 크기 변경 필요 : 이펙트 크게 나오는 이유임.
		effect.transform.localScale = new Vector3(sign * Owner.transform.localScale.x, Owner.transform.localScale.y, Owner.transform.localScale.z); 
        

		yield return new WaitForSeconds(duration); // 이펙트 재생 시간
		Managers.Resource.Release(effect);
		
	}

	protected void PlayEffect(string effName, float duration, float sign = 1, Vector2 offset = default, Transform parent = default)
	{
		StartCoroutine(PlayEffectCo(effName, duration, sign, offset, parent));
	}

	IEnumerator PlayEffectCo(string effName, float duration, float sign, Vector2 offset, Transform parent)
	{
		if (parent == default)
		{
			parent = Owner.transform;
		}
		GameObject effect = Managers.Resource.Instantiate("Skills/" + effName, parent); // paraent를 character.gameObject로

		if (effect)
		{
			effect.transform.localPosition = Vector3.zero;
		}
		else
		{
			Debug.LogError($"effect is null. effName :{effName}");
		}

		effect.transform.localPosition += (Vector3)offset;
		effect.transform.localScale = new Vector3(sign * Owner.transform.localScale.x, Owner.transform.localScale.y, Owner.transform.localScale.z);

		yield return new WaitForSeconds(duration); // 이펙트 재생 시간
		Managers.Resource.Release(effect);

	}
}
