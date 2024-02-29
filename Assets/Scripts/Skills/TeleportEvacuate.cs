using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using System.Collections;
using UnityEngine;

public class TeleportEvacuate : ActiveSkillBase
{
	private TeleportEvacuateData _data;

	Vector3 effectScale = new(0.6f, 0.6f, 0.6f);

	public override void Init()
	{
		base.Init();

		_data = LoadData<TeleportEvacuateData>();
	}

	public override void Execute()
	{		
		base.Execute();

		if (_data.beforeEffect != null) { PlayEffect("Teleport_Before", 1f, Vector2.zero, effectScale:effectScale); }
	}

	public override IEnumerator ExecuteImplCo()
	{
		Owner.transform.position = CalcTeleportPos();

		Owner.Target.GetComponent<CharacterStatus>().SetSlowEffect(_data.SlowTime, _data.SlowRatio);

		if (_data.afterEffect != null) { PlayEffect("Teleport_After", 1f, _data.Offset, effectScale:effectScale); }

		yield return new WaitForSeconds(AfterDelay);
	}

	Vector3 CalcTeleportPos()
	{
		Vector3 destV;

		Camera mainCamera = Camera.main;
		Vector3 centerV = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));

		Transform ownerTrans = Owner.transform;
		Transform enemyTrans = Owner.GetTarget().transform;

		// 적 기준, 좌 우 텔포위치
		Vector3 enemyLeft  = enemyTrans.position + enemyTrans.right * -_data.telpoDistance;
		Vector3 enemyRight = enemyTrans.position + enemyTrans.right *  _data.telpoDistance;


		if( enemyTrans.position.x - centerV.x > 0){ // 중앙 오른쪽에 있으면
			destV = enemyLeft; // 텔포 위치는 적의 왼쪽
		}else{
			destV = enemyRight;
		}

		// 현재 높이 유지
		destV = new Vector3(destV.x, ownerTrans.position.y , destV.z );
		return destV;
	}


	
	void OnDrawGizmos() 
	{
		// Gizmos.color = Color.red;
		// Vector3 checkboxPos = Owner.transform.position;
		// Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.CheckBoxCenter, (Vector3)_data.CheckBoxSize);
    }

	public override bool CheckCanUse()
	{
		Vector3 center = (Vector2)Owner.transform.position + _data.CheckBoxCenter;
		Vector3 size =  _data.CheckBoxSize;

		var boxes = Physics2D.OverlapBoxAll( center, size , 0);
		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			if ( character == Owner.GetTarget().GetComponent<Character>() ){
				return true;
			}
		}
		return false;
	}

}

