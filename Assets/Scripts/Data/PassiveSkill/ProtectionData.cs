
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProtectionData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(ProtectionData))]
public class ProtectionData : PassiveSkillData
{

	public int absProtectionValue;

	public float effectDuration; 

}
