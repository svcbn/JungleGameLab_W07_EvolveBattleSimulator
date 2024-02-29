using UnityEngine;

[CreateAssetMenu(fileName = nameof(TripleStrikeData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(TripleStrikeData))]
public class TripleStrikeData : ActiveSkillData
{
	[SerializeField] private float _delayBetween;

	public float DelayBetween => _delayBetween;

}
