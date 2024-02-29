using System.Collections;
using UnityEngine;

public class TripleStrike : ActiveSkillBase
{
	private TripleStrikeData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<TripleStrikeData>();
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
			//effect = Managers.Resource.Instantiate("Skills/" + _data.Effect.name);
			//effect.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			//effect.transform.position = Owner.transform.position;
			PlayEffect(_data.Effect.name, 1, _data.Offset, Owner.transform.localScale.x);
		}

		yield return new WaitForSeconds(0);
		for (int i = 0; i < 3; i++)
		{
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

				// Todo : statmanager 쪽에 연산 요청
				Managers.Stat.GiveDamage(1 - Owner.playerIndex, _data.Damage);
			}

			yield return new WaitForSeconds(_data.DelayBetween);
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

	//public void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Vector3 hitboxPos = Owner.transform.position;
	//	// if ( gameObject != null){
	//	// 	hitboxPos = transform.position;
	//	// }

	//	//Debug.Log( $"OnDrawGizmos() | hitboxPos {hitboxPos}  hitBoxCenter {_data.HitBoxCenter}  hitBoxSize {_data.HitBoxSize} " );

	//	Gizmos.DrawCube(hitboxPos + (Vector3)_data.HitBoxCenter, (Vector3)_data.HitBoxSize);
	//}
}
