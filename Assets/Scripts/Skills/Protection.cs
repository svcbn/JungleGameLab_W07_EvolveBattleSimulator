using System.Collections;
using UnityEngine;

public class Protection : PassiveSkillBase
{
	ProtectionData _data;

	static bool registered = false;

	public override void Init()
	{
		_data = LoadData<ProtectionData>();

		Excute();

		if( !registered ){
			Managers.Stat.onTakeDamage += PlayGuardEffect;
			registered = true;
		}
	}

	public override void Reset()
	{
		base.Reset();
		PresentNumber = _data.PresentNumber;
	}

	void Excute()
	{
		Modifier modifier = Managers.Stat.GetAbsProtectionModifier(Owner.playerIndex, GetType().Name);
		modifier.value = _data.absProtectionValue;
	}


	void PlayGuardEffect(int actorIndex, int value)
	{
		if( Owner.playerIndex != actorIndex ) return;

		StartCoroutine(PlayEffect());
	}


	IEnumerator PlayEffect()
	{
		if( _data.Effect == null){ yield break;}

		GameObject effect = Managers.Resource.Instantiate("Skills/"+_data.Effect.name, Owner.transform );


		effect.transform.localPosition = new(0,0,0);

		float direc = Owner.transform.localScale.x < 0 ? -1 : 1;
		Vector3 effScale = new(0.4f,0.4f,0.4f);
		effect.transform.localScale = new Vector3(direc * effScale.x, effScale.y, effScale.z );

		yield return new WaitForSeconds(_data.effectDuration);

		Managers.Resource.Release(effect);
	}
}