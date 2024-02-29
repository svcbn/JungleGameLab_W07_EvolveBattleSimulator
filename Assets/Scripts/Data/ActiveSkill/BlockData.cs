
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BlockData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(BlockData))]
public class BlockData : ActiveSkillData
{
	public float beforehandTime = .2f;
    public float Duration = 5f;
}
