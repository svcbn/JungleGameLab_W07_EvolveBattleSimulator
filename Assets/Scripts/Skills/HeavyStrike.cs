using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HeavyStrike : ActiveSkillBase
{
	private HeavyStrikeData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<HeavyStrikeData>();
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		GameObject effect = null;
		// 애니메이션 재생
		if (_data.Effect != null)
		{
			effect = Managers.Resource.Instantiate("Skills/" + _data.Effect.name);
			effect.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			effect.transform.position = Owner.transform.position;
		}

		// 실제 피해
		yield return new WaitForSeconds(0);
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * _data.HitBoxCenter.x, _data.HitBoxCenter.y);
		var boxes = Physics2D.OverlapBoxAll(centerInWorld, _data.HitBoxSize, 0);
		DebugRay(centerInWorld, _data.HitBoxSize);

		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			// Todo : statmanager 쪽에 데미지 연산 요청
			Managers.Stat.GiveDamage(1 - Owner.playerIndex, _data.Damage);

			// Todo : Charactorstatus 쪽에 스턴 요청
			character.Status.SetKnockbackEffect(2, 50, transform.position);
			character.Status.SetFaintEffect(5);
		}

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
