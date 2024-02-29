using System.Collections;
using UnityEngine;

public class Slash : ActiveSkillBase
{
	private SlashData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<SlashData>();
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		// 애니메이션 재생
		if (_data.Effect != null){
			PlayEffect(_data.Effect.name, 1, _data.Offset, Owner.transform.localScale.x);
		}

		// 실제 피해
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * _data.HitBoxCenter.x, _data.HitBoxCenter.y);
		var boxes = Physics2D.OverlapBoxAll(centerInWorld, _data.HitBoxSize, 0);
		DebugRay(centerInWorld, _data.HitBoxSize);

		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			//StatManager 쪽에 데미지 연산 요청
			int dmg = Mathf.RoundToInt(Managers.Stat.GetMaxHp(1 - Owner.playerIndex) * (_data.PercentageDamage / 100f));
			Managers.Stat.GiveDamage(1 - Owner.playerIndex, dmg);
			character.Status.SetKnockbackEffect(.2f, 3f, transform.position);
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);
		bool isEnoughMP   = CheckEnoughMP(RequireMP);

		return isEnemyInBox && isEnoughMP;
	}
}

