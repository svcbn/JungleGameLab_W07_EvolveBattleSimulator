
using UnityEngine;

[CreateAssetMenu(fileName = nameof(MagicMissileData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(MagicMissileData))]
public class MagicMissileData : ActiveSkillData
{
	public GuidedBulletMover missilePrefab;
    public LayerMask targetLayer; // Player Layer
    public float duration;
    public int missileCount;
    
    
    public float range;
    public float bezierDelta;
    public float bezierDelta2;
}
