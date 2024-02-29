public interface IActiveSkill : ISkill
{
	/// <summary>
	/// 스킬의 우선순위
	/// </summary>
	int Priority { get; }

	/// <summary>
	/// 스킬이 캐릭터의 움직임을 제약하는지 여부
	/// </summary>
	bool IsRestrictMoving { get; }

	/// <summary>
	/// 스킬의 쿨타임이 끝났는지 여부
	/// </summary>
	bool IsCoolReady { get; }

	/// <summary>
	/// 스킬이 실행 중인지 여부
	/// </summary>
	bool IsActing { get; }

	/// <summary>
	/// 스킬의 선 딜레이
	/// </summary>
	float BeforeDelay { get; }

	/// <summary>
	/// 스킬의 사용 시간
	/// </summary>
	float Duration { get; }

	/// <summary>
	/// 스킬의 후 딜레이
	/// </summary>
	float AfterDelay { get; }

	/// <summary>
	/// 스킬 사용에 필요한 mp
	/// </summary>
	int RequireMP { get; }
	
	/// <summary>
	/// 스킬을 사용할 수 있는지 여부를 반환
	/// </summary>
	/// <returns>스킬을 사용할 수 있는지 여부</returns>
	bool CheckCanUse();
	
	/// <summary>
	/// 스킬을 실행
	/// </summary>
	void Execute();
}