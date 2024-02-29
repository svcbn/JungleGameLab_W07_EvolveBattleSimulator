
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TripleFireballData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(TripleFireballData))]
public class TripleFireballData : ActiveSkillData
{
	public FireBallProjectile prefab;
    public float speed;
	public float delay;
	public int targetIndex;
	public float lifespan;
	public LayerMask targetLayer;

	public float range;
	public float interval;
}
