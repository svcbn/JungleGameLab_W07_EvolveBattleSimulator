using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarCloak : PassiveSkillBase
{
	private SolarCloakData _data;
	public bool IsCoolReady { get; protected set; } = true;
	public int Damage { get; protected set; }

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<SolarCloakData>("Data/SolarCloakData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/SolarCloakData"); return; }

		Id = _data.Id;
		Cooldown = _data.Cooldown;

		PresentNumber = _data.PresentNumber;
		HasPresentNumber = _data.HasPresentNumber;

		GameObject effect = Managers.Resource.Instantiate("Skills/" + _data.DefaultEffect.name);
		effect.transform.SetParent(Owner.transform);
		effect.transform.localPosition = Vector3.zero;

		Managers.Stat.AddMaxHp(Owner.playerIndex, _data.MaxHp);

	}

	public override void Reset()
	{
		base.Reset();
		Damage = Mathf.RoundToInt(Managers.Stat.GetExtraHp(Owner.playerIndex) * _data.DamageScale);
		if (Damage <= 0) Damage = 1;
		PresentNumber = Damage;
	}

	private void Update()
	{
		if (IsCoolReady)
		{
			IsEnabled = true;
			IsCoolReady = false;
			if (_data.Effect != null)
			{
				PlayEffect(_data.Effect.name, 1, 1, Vector2.down * 1f);
			}
			DoDamage();
			CalculateCoolTime();
		}
	}

	void DoDamage()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;


		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * _data.HitBoxCenter.x, _data.HitBoxCenter.y);
		var boxes = Physics2D.OverlapBoxAll(centerInWorld, _data.HitBoxSize, 0);
		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			Managers.Stat.GiveDamage(1 - Owner.playerIndex, Damage);
		}
	}


	void CalculateCoolTime()
	{
		Owner.StartCoroutine(CoCalculateTime(Cooldown, time => CurrentCooldown = time, () => IsCoolReady = true));
	}

	private static IEnumerator CoCalculateTime(float time, Action<float> onUpdate, Action onFinish)
	{
		float timer = 0;
		while (timer < time)
{
			yield return null;
			timer += Time.deltaTime;
			onUpdate?.Invoke(timer);
		}

		onFinish?.Invoke();
	}

}
