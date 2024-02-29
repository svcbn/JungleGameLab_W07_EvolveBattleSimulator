using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
	public bool CurrentSkillIsNotNull;
	
	public List<IActiveSkill> CanUseSkills = new();
	public IActiveSkill CurrentSkill;
	public int playerIndex;
	public bool CanMove { get; private set; } = true;
	public CharacterStatus Status { get; private set; }

	public delegate void OnActiveSkillExcuteType();
	public event OnActiveSkillExcuteType OnActiveSkillExcute;
	public void NotifyActiveSkillExcute()
	{
		OnActiveSkillExcute?.Invoke();
	}

	[SerializeField]
	private GameObject _target;
	
	public GameObject Target => _target;

	private BehaviorTree _moveBehavior;
	private Rigidbody2D _rigidbody2;

	private readonly ObservableCollection<ISkill> _skills = new();
	
	public bool _hasCooldowmSkill;

	public IReadOnlyList<ISkill> Skills => _skills;

	public BehaviorTree BehaviorTree => _moveBehavior;

	public event NotifyCollectionChangedEventHandler CollectionChanged
	{
		add => _skills.CollectionChanged += value;
		remove => _skills.CollectionChanged -= value;
	}

	private void Awake()
	{
		_moveBehavior = GetComponent<BehaviorTree>();
		_rigidbody2 = GetComponent<Rigidbody2D>();
		Status = GetComponent<CharacterStatus>();
	}

	private void Start()
	{
		StartCoroutine(CheckSkills());
	}

	public void AddSkill(ISkill skill)
	{
		skill.Owner = this;
		skill.Init();
		_skills.Add(skill);
	}

	private void Update()
	{
		CurrentSkillIsNotNull = CurrentSkill is not null;
	}

	//private void OnDrawGizmos()
	//{
	//	foreach (var skill in _skills)
	//	{
	//		skill.OnDrawGizmos(transform);
	//	}
	//}

	private IEnumerator CheckSkills()
	{
		while (true)
		{
			CanUseSkills.Clear();
			_hasCooldowmSkill = false;
			if (CurrentSkill is { IsActing: true })
			{
				yield return new WaitForSeconds(0.05f);
				continue;
			}

			foreach (var skill in _skills.OfType<IActiveSkill>())
			{
				if (skill.IsCoolReady)
					_hasCooldowmSkill = true;

				if (skill.CheckCanUse() && CanUseSkills.Contains(skill) == false && skill.IsCoolReady)
					CanUseSkills.Add(skill);

				if (CurrentSkill != null && CurrentSkill.IsActing == false)
					CurrentSkill = null;
			}

			if (Skills.Any(skill => skill.Id == 104))
			{
				_hasCooldowmSkill = true;
			}

			yield return new WaitForSeconds(0.2f);
		}
	}

	public List<IActiveSkill> GetHighPrioritySkill()
	{

		int highPriority = int.MaxValue;
		List<IActiveSkill> _tmpSkills = new();
		
		foreach( var skill in CanUseSkills)
		{
			if (skill.Priority < highPriority)
			{
				highPriority = skill.Priority;
			}
		}

		foreach( var skill in CanUseSkills)
		{
			if (skill.Priority == highPriority)
			{
				_tmpSkills.Add(skill);
			}
		}
		return _tmpSkills;
	}

	public GameObject GetTarget()
	{
		if(_target == null){
			Debug.LogWarning("Target is null");
			return null;
		}

		return _target;
	}
}
