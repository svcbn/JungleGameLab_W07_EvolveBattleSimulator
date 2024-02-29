using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : PassiveSkillBase
{
	CharacterStatus _status;

	BerserkerData _data;

	bool _isActived;

	GameObject _effect;

	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = Managers.Resource.Load<BerserkerData>("Data/BerserkerData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/BerserkerData"); return; }

		Id = _data.Id;
	
		Managers.Stat.onTakeDamage += Excute;

		_effect = Managers.Resource.Instantiate("Skills/Stat_Berserker", transform);
		Transform offset = Resources.Load<GameObject>("Prefabs/Skills/Stat_Berserker").transform;
		
		_effect.transform.localPosition = offset.position;
		_effect.transform.localScale = offset.localScale;

		_effect.SetActive(false);

		CurrentCooldown = 0;
	}

	public override void Reset()
	{
		base.Reset();

		CurrentCooldown = 0;

		if (_isActived == true)
		{ 
			_isActived = false;
			_effect.SetActive(false);

			_status.HasteRatio -= _data.SpeedUpRatio;
			_status.CooldownChange -= _data.CooldownRatio;
			Managers.Stat.GetDamageModifier(Owner.playerIndex, GetType().Name).value = 0f;
		}
	}

	void Excute(int playerIndex, int finalDamage)
	{
		if ((Managers.Stat.GetCurrentHp(Owner.playerIndex) / (float)Managers.Stat.GetMaxHp(Owner.playerIndex)) <= 0.5f && _isActived == false)
		{
			_isActived = true;
			_effect.SetActive(true);

			CurrentCooldown = _data.Cooldown;

			IsEnabled = true;

			_status.HasteRatio += _data.SpeedUpRatio;
			_status.CooldownChange += _data.CooldownRatio;
			Managers.Stat.GetDamageModifier(Owner.playerIndex, GetType().Name).value = _data.PowerUpRatio * 100f;

			Modifier modifier = Managers.Stat.GetDamageModifier(Owner.playerIndex, GetType().Name);
			modifier.value = _data.PowerUpRatio;

		}
	}

}
