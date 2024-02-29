using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BonusShotData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(BonusShotData))]
public class BonusShotData : PassiveSkillData
{

	public Vector2 checkBoxCenter;
	public Vector2 checkBoxSize;    

	public GuidedBulletMover missilePrefab;
    public LayerMask targetLayer;
    public float duration;
    public int missileCount;
    
    public float damageRate;
    
    public float range;
    public float bezierDelta;
    public float bezierDelta2;
}
