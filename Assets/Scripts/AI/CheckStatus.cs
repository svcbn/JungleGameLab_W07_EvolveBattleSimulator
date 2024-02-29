using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Mono.Cecil.Cil;
using System.Linq;

public class CheckStatus : Action
{
	Character _character;
	CharacterStatus _status;

	public SharedGameObject _target;
	
	public SharedVector3 _direction;

	public SharedFloat _distance;

	public SharedBool _hasCooldownSkill;
	public SharedBool _canUseSkill;
	public SharedBool _isActing;
	public SharedBool _canMove;


	public override void OnAwake()
	{
		base.OnAwake();

		_character = GetComponent<Character>();
		_status = GetComponent<CharacterStatus>();
	}

	public override void OnStart()
	{
		base.OnStart();

		var direction = _character.Target.transform.position - transform.position;
		var distance = direction.magnitude;

		_direction.Value = direction;
		_distance.Value = distance; ;

		_hasCooldownSkill.Value = _character._hasCooldowmSkill;
		_target.Value = _character.Target;

		// CanUseSkill Control
		if (_character.CanUseSkills != null && _character.CanUseSkills.Count > 0
			&& _status.CurrentStatus[StatusType.Faint] == false
			&& _status.CurrentStatus[StatusType.Knockback] == false)
		{
			_canUseSkill.Value = true;
		}
		else if (_character.Skills.Any(skill => skill.Id == 104))
		{ 
			_canUseSkill.Value = true;
		}
		else
		{
			_canUseSkill.Value = false;
		}

		// IsAction Control 
		if (_character.CurrentSkill != null)
		{
			_isActing.Value = true;
		}
		else
		{
			_isActing.Value = false;
		}

		// CanMove Control
		if (_character.CurrentSkill != null)
		{
			if (_character.CurrentSkill.IsRestrictMoving == true
				|| _status.CurrentStatus[StatusType.Faint] == true
				|| _status.CurrentStatus[StatusType.Knockback] == true)
			{
				_canMove.Value = false;
			}
			else
			{
				_canMove.Value = true;
			}
		}
		else
		{
			if (_status.CurrentStatus[StatusType.Faint] == true
				|| _status.CurrentStatus[StatusType.Knockback] == true)
			{
				_canMove.Value = false;
			}
			else
			{
				_canMove.Value = true;
			}
		}
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}
