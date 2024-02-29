using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingShower : ActiveSkillBase
{
	private HealingShowerData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<HealingShowerData>();

		Duration         = _data.effectDuration;
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		if (_data.Effect != null){
			Vector3 effectScale = new(0.6f, 0.6f, 0.6f);
			PlayEffect( _data.Effect.name, Duration, Vector3.zero, x, effectScale: effectScale);
		}

		Managers.Stat.GiveHeal(Owner.playerIndex, _data.Amount);

		yield return new WaitForSeconds(AfterDelay);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = !CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);
		bool isEnoughMP   = CheckEnoughMP(RequireMP);
		bool isFullHP	  = CheckFullHp();

		return isEnemyInBox && isEnoughMP && !isFullHP ;
	}

	private bool CheckFullHp()
	{
		int CurrentHp = Managers.Stat.GetCurrentHp(Owner.playerIndex );
		int MaxHp     = Managers.Stat.GetMaxHp(Owner.playerIndex );

		if ( CurrentHp == MaxHp)
		{
			return true;
		}

		return false;
	}
}
