using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unstoppable : PassiveSkillBase
{
	CharacterStatus _status;

	UnstoppableData _data;

	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = LoadData<UnstoppableData>();

		_status.HasteRatio += _data.SpeedUpRatio;
	}

	public override void Reset()
	{
		base.Reset();
	}
}
