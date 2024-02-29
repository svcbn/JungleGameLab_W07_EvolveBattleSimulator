using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class ActiveSkillBase : SkillBase, IActiveSkill
{
	public int Priority { get; protected set; }

	public bool IsRestrictMoving { get; protected set; }

	public float BeforeDelay { get; protected set; }

	public float Duration { get; protected set; }

	public float AfterDelay { get; protected set; }

	public int RequireMP { get; protected set; }

	public bool IsCoolReady { get; protected set; }

	public bool IsActing { get; protected set; }

	public bool IsBeforeDelay { get; protected set; }

	public Coroutine UsingSkillCo { get; protected set; }

	protected CharacterStatus _status;
	private Coroutine _cooldownHandler;

	protected virtual void Awake()
	{
		IsCoolReady = true;

		_status = GetComponent<CharacterStatus>();
	}

	public virtual void Execute()
	{
		//Debug.Log(GetType().Name);

		IsCoolReady = false;
		IsActing = true;

		CalculateCooltime();

		IsBeforeDelay = true;

		Invoke("ExecuteImpl", BeforeDelay);
	}

	public override void Reset()
	{
		base.Reset();

		if (_cooldownHandler != null)
		{
			Owner.StopCoroutine(_cooldownHandler);
			CurrentCooldown = Cooldown;
			IsCoolReady = true;
		}
	}

	void ExecuteImpl()
	{
		Owner.NotifyActiveSkillExcute();

		IsBeforeDelay = false;
		UsingSkillCo = Owner.StartCoroutine(ExecuteImplCo());
	}

	public abstract IEnumerator ExecuteImplCo();


	public abstract bool CheckCanUse();

	protected virtual void CalculateCooltime()
	{
		_cooldownHandler = Owner.StartCoroutine(CoCalculateTime(Cooldown, time => CurrentCooldown = time, () =>
		{
			IsCoolReady = true;
			_cooldownHandler = null;
		}, _status.CooldownChange));
		float delay = BeforeDelay + Duration + AfterDelay;
		Owner.StartCoroutine(CoCalculateTime(delay, null, () => IsActing = false));
	}

	protected virtual bool CheckEnemyInBox(Vector2 center, Vector2 size)
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * center.x, center.y);

		var boxes = Physics2D.OverlapBoxAll(centerInWorld, size, 0);
		bool flag = boxes.Length != 0;
		return boxes.Any(box => box.TryGetComponent<Character>(out var character) && character != Owner);
	}

	protected virtual bool CheckEnoughMP(int _mp)
	{
		// float mp = owner의 mp 얻어오기
		// _mp와 owner의 현재 mp 비교
		return true;
	}

	private static IEnumerator CoCalculateTime(float time, Action<float> onUpdate, Action onFinish, float cooldownRatio = 1)
	{
		float timer = 0;
		while (timer < time)
		{
			yield return null;
			timer += Time.deltaTime * cooldownRatio;
			onUpdate?.Invoke(timer);
		}

		onFinish?.Invoke();
	}

	public void CancelExecute()
	{
		CancelInvoke("ExecuteImpl");
	}

	protected override T LoadData<T>()
	{
		T data = base.LoadData<T>();
		if (data is ActiveSkillData activeSkillData)
		{
			Priority = activeSkillData.Priority;
			IsRestrictMoving = activeSkillData.IsRestrictMoving;

			BeforeDelay = activeSkillData.BeforeDelay;
			AfterDelay = activeSkillData.AfterDelay;
			RequireMP = activeSkillData.RequireMP;
		}

		return data;
	}

	protected void PlayEffect(string effName, float duration, Vector2 offset, float sign = 1, Vector3 effectScale = default	)
	{
		StartCoroutine(PlayEffectCo(effName, duration, offset, sign, effectScale));
	}

	IEnumerator PlayEffectCo(string effName, float duration, Vector2 offset, float sign, Vector3 effectScale = default)
	{
		Transform parent = Owner.transform;
		GameObject effect =
			Managers.Resource.Instantiate("Skills/" + effName, parent); // paraent를 character.gameObject로

		if (effect)
		{
			effect.transform.localPosition = Vector3.zero;
		}
		else
		{
			Debug.LogError($"effect is null. effName :{effName}");
		}

		effect.transform.localPosition += (Vector3)offset;

		if( effectScale != default){
			effect.transform.localScale = new Vector3(sign * effectScale.x,
															 effectScale.y,
															 effectScale.z );
		}else{
			effect.transform.localScale = new Vector3(sign * Owner.transform.localScale.x,
															 Owner.transform.localScale.y,
															 Owner.transform.localScale.z );
		}

		yield return new WaitForSeconds(duration); // 이펙트 재생 시간
		Managers.Resource.Release(effect);
	}

	protected void DebugRay(Vector2 from, Vector2 dir)
	{
		Debug.DrawLine(from + new Vector2(dir.x, dir.y) / 2, from + new Vector2(-dir.x, dir.y) / 2, Color.red, 1f);
		Debug.DrawLine(from + new Vector2(-dir.x, -dir.y) / 2, from + new Vector2(dir.x, -dir.y) / 2, Color.red, 1f);
		Debug.DrawLine(from + new Vector2(-dir.x, dir.y) / 2, from + new Vector2(-dir.x, -dir.y) / 2, Color.red, 1f);
		Debug.DrawLine(from + new Vector2(dir.x, dir.y) / 2, from + new Vector2(dir.x, -dir.y) / 2, Color.red, 1f);
	}
}