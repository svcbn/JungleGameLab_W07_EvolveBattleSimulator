using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Overclock : PassiveSkillBase
{
	OverclockData _data;

	int _currentStack = 0;
	int CurrentStack 
	{ 
		get { return _currentStack; } 
		set
		{
			_currentStack = PresentNumber = value;
			IsEnabled = true;
		}
	}
	Coroutine _stackCycleCR;

	private void Awake()
	{
		_data = LoadData<OverclockData>();
	}

	public override void Reset()
	{
		base.Reset();
		CurrentStack = 0;
		Modifier modifier = Managers.Stat.GetDamageModifier(Owner.playerIndex, GetType().Name);
		modifier.value = _data.bonusDamagePerStack * CurrentStack;

		if(_stackCycleCR != null) { StopCoroutine(_stackCycleCR); }
		_stackCycleCR = StartCoroutine(CR_StackCycle());

	}

	IEnumerator CR_StackCycle()
	{
		CurrentCooldown = 0;
		while (CurrentCooldown < Cooldown)
		{
			CurrentCooldown += Time.deltaTime;
			yield return null;
		}
		if(CurrentStack < _data.maxStack)
		{
			CurrentStack++;
			//스택에 맞게 버프 적용
			Modifier modifier = Managers.Stat.GetDamageModifier(Owner.playerIndex, GetType().Name);
			modifier.value = _data.bonusDamagePerStack * CurrentStack;
			//최대 스택일때 특수 처리
			if (CurrentStack >= _data.maxStack) modifier.value *= _data.maxStackBonusMultiplier;
		}
		if (_stackCycleCR != null) { StopCoroutine(_stackCycleCR); }
		_stackCycleCR = StartCoroutine(CR_StackCycle());
	}
}
