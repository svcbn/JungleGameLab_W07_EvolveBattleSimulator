using System.ComponentModel;
using UnityEngine;

public interface ISkill : INotifyPropertyChanged
{
	/// <summary>
	/// 스킬의 아이디
	/// </summary>
	uint Id { get; }

	/// <summary>
	/// 스킬의 타입
	/// </summary>
	SkillType SkillType { get; }

	/// <summary>
	/// 스킬을 소유한 캐릭터
	/// </summary>
	Character Owner { get; set; }

	/// <summary>
	/// 스킬의 쿨타임
	/// </summary>
	float Cooldown { get; }

	/// <summary>
	/// 스킬의 현재 쿨타임
	/// </summary>
	float CurrentCooldown { get; }

	/// <summary>
	/// 스킬 초기화 시 호출
	/// </summary>
	void Init();

	/// <summary>
	/// 매 전투 시작 시 호출
	/// </summary>
	void Reset();
}