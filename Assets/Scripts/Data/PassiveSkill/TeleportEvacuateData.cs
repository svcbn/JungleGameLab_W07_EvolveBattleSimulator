using UnityEngine;

[CreateAssetMenu(fileName = nameof(TeleportEvacuateData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(TeleportEvacuateData))]
public class TeleportEvacuateData : ActiveSkillData
{
	public float SlowTime;
	public float SlowRatio;

    public ParticleSystem beforeEffect; // Prefab/Particle/Teleport_blue 1
    public ParticleSystem afterEffect;  // Prefab/Particle/Level_Up_blue 1


    public int telpoDistance;

}