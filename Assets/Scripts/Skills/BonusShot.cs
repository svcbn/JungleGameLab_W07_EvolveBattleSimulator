using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BonusShot : PassiveSkillBase
{
	private BonusShotData _data;

	public override void Init()
	{
		base.Init();
		_data = LoadData<BonusShotData>();

		Managers.Stat.onTakeDamage += Execute; // 누가 맞았음.
	}

	void Execute(int playerIndex, int takeDamage)
	{
		// 1조건 : 적을 타격시
		if (Owner.playerIndex == playerIndex){ return; } 	
		
		// 2조건 : 적이 범위 안에 없을때
		bool isEnemyInRange = Vector2.Distance(Owner.transform.position, Owner.Target.transform.position) <= _data.range ;
		if( isEnemyInRange ){ return; }
		
		// 3조건 : 1 이상 값
		int finalDamage = Mathf.RoundToInt(takeDamage * _data.damageRate ); // 타격 데미지의 n% 
		if (finalDamage < 1){ finalDamage = 1; }                           // 최소 데미지 1

		StartCoroutine(ShootBonusArrow(finalDamage));
		
	}


	IEnumerator ShootBonusArrow(int damage)
	{
		IsEnabled = true;
        int colCount = 0;
        Collider2D[] cols = Physics2D.OverlapCircleAll(Owner.transform.position, 50f, _data.targetLayer);
        if (cols.Length == 0) // 타겟이 없을때 
        {
            for (int i = 0; i < _data.missileCount; i++)
            {
                GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

                g.target = null;
                g.bezierDelta  = _data.bezierDelta;
                g.bezierDelta2 = _data.bezierDelta2;
                g.Init(Owner , damage);

            }
            yield break;
        }


		List<Collider2D> validTargets = new List<Collider2D>();
		foreach (var col in cols)
		{
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

				if( _data.missilePrefab == null){ Debug.LogWarning("_data.missilePrefab is null"); yield break;}

				GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

				if(g == null){
					Debug.LogWarning("g is null");
				}

				g.target = validTargets[colCount].transform;
				g.bezierDelta  = _data.bezierDelta;  // 상대 원
				g.bezierDelta2 = _data.bezierDelta2; // 나 원
                g.Init(Owner , damage);

				colCount += 1;            
			}
		}
	}

	

	protected virtual bool CheckEnemyInBox(Vector2 center, Vector2 size)
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;
		
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * center.x, center.y);

		var boxes = Physics2D.OverlapBoxAll(centerInWorld, size, 0);
		bool flag = boxes.Length != 0;
		return boxes.Any(box => box.TryGetComponent<Character>(out var character) && character != Owner);
	}

	private void OnDrawGizmos() 
	{
		Gizmos.color = new Color(128f/255f, 0, 128f/255f);//Color.purple;
		Vector3 checkboxPos = Owner.transform.position;
		Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.checkBoxCenter, (Vector3)_data.checkBoxSize);	
	}
}

