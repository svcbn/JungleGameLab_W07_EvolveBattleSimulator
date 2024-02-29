using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : ActiveSkillBase
{
	private ArrowShootData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<ArrowShootData>();
	}

	public override IEnumerator ExecuteImplCo()
	{
        int colCount = 0;
        Collider2D[] cols = Physics2D.OverlapCircleAll(Owner.transform.position, _data.range, _data.targetLayer);
        if (cols.Length == 0) // 타겟이 없을때 
        {
            for (int i = 0; i < _data.missileCount; i++)
            {
                GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

                g.target = null;
                g.bezierDelta  = _data.bezierDelta;
                g.bezierDelta2 = _data.bezierDelta2;
                g.Init(Owner,_data.Damage);
            }
            yield break;
        }

		List<Collider2D> validTargets = new List<Collider2D>();
		foreach (var col in cols)
		{

			//if (col.gameObject != Owner.gameObject) // Owner is the self gameObject
			if (col.GetComponent<Character>() != Owner)
			{
				validTargets.Add(col);
			}
		}

		if( validTargets.Count > 0)
		{
			for(int i = 0; i < _data.missileCount; i++)
			{
				if (i%validTargets.Count==0) { colCount = 0; }

				if(_data == null){
					Debug.LogWarning("_data is null");
				}
				if(_data.missilePrefab == null){
					Debug.LogWarning("_data.missilePrefab is null");
				}

				GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

				if(g == null){
					Debug.LogWarning("g is null");
				}

				g.target = validTargets[colCount].transform;
				g.bezierDelta  = _data.bezierDelta;  // 상대 원
				g.bezierDelta2 = _data.bezierDelta2; // 나 원
                g.Init(Owner,_data.Damage);

				colCount += 1;            
			}
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}

	
	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);


		return isEnemyInBox;
	}

	private void OnDrawGizmos() 
	{
		Gizmos.color = Color.green;
		Vector3 checkboxPos = Owner.transform.position;
		Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.CheckBoxCenter, (Vector3)_data.CheckBoxSize);	
	}
}

