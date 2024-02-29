
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ArrowShootData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(ArrowShootData))]
public class ArrowShootData : ActiveSkillData
{
	public GuidedBulletMover missilePrefab;
    public LayerMask targetLayer;
    public float duration;
    public int missileCount;
    
    
    public float range;
    public float bezierDelta;
    public float bezierDelta2;
}
