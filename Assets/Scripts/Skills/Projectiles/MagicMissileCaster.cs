using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class MagicMissileCaster : MonoBehaviour
{
	public GuidedBulletMover missilePrefab;
    public LayerMask damageLayer;
    public float duration = 1f;
    public int missileCount = 8;
    public float fireBallDelay = 1f;
    
    
    public float range = 5f;
    public float bezierDelta  = 10f;
    public float bezierDelta2 = 10f;
    public int damage = 1;


    public GameObject caster;

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Spell());
        }    
    }


    IEnumerator Spell()
    {
        int colCount = 0;
        Collider2D[] cols = Physics2D.OverlapCircleAll(caster.transform.position, range,damageLayer);
        if (cols.Length == 0) // 타겟이 없을때 
        {
            for (int i = 0; i < missileCount; i++)
            {
                GuidedBulletMover g = Instantiate(missilePrefab, caster.transform.position, Quaternion.identity);

                g.target = null;
                g.bezierDelta  = bezierDelta;
                g.bezierDelta2 = bezierDelta2;
                //g.Init(this);
            }
            yield break;
        }

        for(int i = 0; i < missileCount; i++)
        {
            if (i%cols.Length==0) { colCount = 0; }
            GuidedBulletMover g = Instantiate(missilePrefab, caster.transform.position, Quaternion.identity);

            g.target = cols[colCount].transform;
            g.bezierDelta  = bezierDelta;  // 상대 원
            g.bezierDelta2 = bezierDelta2; // 나 원
            //g.Init();

            colCount += 1;            
        }
        yield break;
    }
}
