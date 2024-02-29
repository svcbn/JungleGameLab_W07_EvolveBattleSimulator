using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerBoots : PassiveSkillBase
{
	CharacterStatus _status;

	KillerBootsData _data;

	private Coroutine _hasteCR;

	private float _activeTime;
	
	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = LoadData<KillerBootsData>();

		Managers.Stat.onTakeDamage += Excute;
	}

	void Excute(int playerIndex, int finalDamage)
	{
		if (Owner.playerIndex != playerIndex)
		{
			_activeTime = 0;

			IsEnabled = true;

			if (_hasteCR == null)
				_hasteCR = StartCoroutine(nameof(HasteEffect));
		}
	}

	IEnumerator HasteEffect()
	{
		_status.HasteRatio += _data.SpeedUpRatio;

		while (_activeTime < _data.EffectDuration)
		{
			_activeTime += Time.deltaTime;
			yield return null;
		}

		_status.HasteRatio -= _data.SpeedUpRatio;
		_hasteCR = null;
	}
}
