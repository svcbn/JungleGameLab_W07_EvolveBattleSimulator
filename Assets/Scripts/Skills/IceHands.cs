using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHands : PassiveSkillBase
{
	CharacterStatus _status;

	IceHandsData _data;

	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = LoadData<IceHandsData>();

		Managers.Stat.onTakeDamage += Excute;
	}

	void Excute(int playerIndex, int finalDamage)
	{
		if (Owner.playerIndex != playerIndex)
		{
			IsEnabled = true;

			Owner.Target.GetComponent<CharacterStatus>().SetSlowEffect(_data.EffectDuration, _data.SpeedDownRatio);
		}
	}
}
