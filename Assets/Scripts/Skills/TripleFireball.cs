using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleFireball : ActiveSkillBase
{
	private TripleFireballData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<TripleFireballData>();
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		for (int i = 0; i < 3; i++)
		{
			FireBallProjectile f = Instantiate(_data.prefab, Owner.transform.position, Quaternion.identity);

			f.speed = _data.speed;
			f.delay = _data.delay;
			f.targetIndex = 1- Owner.playerIndex;
			f.lifespan = _data.lifespan;
			f.damage = _data.Damage;

			yield return new WaitForSeconds(_data.interval);
		}

		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}


	IEnumerator PlayEffect(Transform pos)
	{
		GameObject effect = null;
		if (_data.Effect != null)
		{
			effect = Managers.Resource.Instantiate("Skills/" + _data.Effect.name);
			effect.transform.position = Owner.transform.position;
		}

		yield return null; //new WaitForSeconds(0.5f); // 이펙트 재생 시간

		Managers.Resource.Release(effect);
	}


	public override bool CheckCanUse()
	{
		float dist = Vector2.Distance((Vector2)Managers.Stat.Characters[0].transform.position, (Vector2)Managers.Stat.Characters[1].transform.position);
		bool isEnemyInRange = (dist <= _data.range);

		return isEnemyInRange;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 checkboxPos = Owner.transform.position;
		Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.CheckBoxCenter, (Vector3)_data.CheckBoxSize);
	}
}

