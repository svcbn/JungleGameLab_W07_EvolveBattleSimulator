using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStrike : ActiveSkillBase
{
	private BloodStrikeData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<BloodStrikeData>();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		GameObject effect = null;
		// 애니메이션 재생
		if (_data.Effect != null)
		{
			// todo 상대위치에다가
			effect = Managers.Resource.Instantiate("Skills/" + _data.Effect.name);
			effect.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			effect.transform.position = Owner.Target.transform.position;
		}

		// 실제 피해
		Managers.Stat.GiveDamage(1 - Owner.playerIndex, _data.Damage);

		// 흡혈
		Managers.Stat.GiveHeal(Owner.playerIndex, _data.Amount);

		// 후딜
		yield return new WaitForSeconds(AfterDelay);

		Managers.Resource.Release(effect);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);

		bool isEnoughMP = CheckEnoughMP(RequireMP);

		return isEnemyInBox && isEnoughMP;
	}
}
