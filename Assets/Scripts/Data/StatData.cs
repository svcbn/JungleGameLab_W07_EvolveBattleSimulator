using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = nameof(StatData), menuName = "ScriptableObjects/" + nameof(StatData))]
public class StatData : ScriptableObject
{
	[Header("Hit Point")]
	public int startingMaxHp;
	public int hpGrowthPerRound;

	[Header("Mana Point")]
	public int startingMaxMp;
	public int mpGrowthPerRound;
}
