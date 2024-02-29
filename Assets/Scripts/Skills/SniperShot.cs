using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperShot : ActiveSkillBase
{
	private SniperShotData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<SniperShotData>();
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		GameObject effect = null;
		GameObject effect2 = null;

		// 애니메이션 재생
		if (_data.Effect != null)
		{
			effect = Managers.Resource.Instantiate("Skills/" + _data.Effect.name);
			effect.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			effect.transform.position = Owner.transform.position;
		}
		//셀프 넉백
		//Owner.GetComponent<CharacterStatus>().SetKnockbackEffect(_data.knockbackInfo.duration, _data.knockbackInfo.power, Owner.Target.transform.position);

		// 실제 피해

		//StatManager 쪽에 데미지 연산 요청
		Managers.Stat.GiveDamage(1 - Owner.playerIndex, _data.Damage);

		Owner.Target.GetComponent<CharacterStatus>().SetKnockbackEffect(_data.knockbackInfo.duration, _data.knockbackInfo.power, Owner.transform.position);


		if (_data.HitEffect != null)
		{
			effect2 = Managers.Resource.Instantiate("Skills/" + _data.HitEffect.name);
			effect2.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			effect2.transform.position = Owner.Target.transform.position;
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);

		Managers.Resource.Release(effect);
		Managers.Resource.Release(effect2);

	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);

		bool isEnoughMP = CheckEnoughMP(RequireMP);

		return isEnemyInBox && isEnoughMP;
	}
}
