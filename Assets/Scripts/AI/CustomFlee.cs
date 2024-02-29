using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class CustomFlee : Action
{
	[SerializeField]
	private SharedFloat _fleeDistance;

	[SerializeField]
	private SharedFloat _wallCheckDistance;

	[SerializeField]
	private SharedGameObject _target;

	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private SharedBool _canMove;
	
	[SerializeField]
	private SharedBool _hasCooldownSkill;

	private SharedVector3 _selectedTarget;

	private CharacterMovement _movement;
	private CharacterJump _jump;

	public override void OnAwake()
	{
		base.OnAwake();

		_movement = GetComponent<CharacterMovement>();
		_jump = GetComponent<CharacterJump>();
	}

	public override void OnStart()
	{
		base.OnStart();

		_selectedTarget = SelectFleeTarget();
	}

	public override TaskStatus OnUpdate()
	{

		if (Vector2.Distance(_target.Value.transform.position, transform.position) < _fleeDistance.Value * 0.9f
			&& _canMove.Value == true
			&& _hasCooldownSkill.Value == false)
		{
			_movement.PlayerInput = (_selectedTarget.Value - transform.position).normalized;
			
			if (GetProbabilitySuccess(0.5f))
				_jump.OnJump(_movement.PlayerInput);

			if (Vector3.Distance(_selectedTarget.Value, transform.position) < _fleeDistance.Value * 0.5f 
				|| IsHitWall() == true)
			{
				_selectedTarget = SelectFleeTarget();
			}

			return TaskStatus.Running;
		}

		return TaskStatus.Success;
	}

	public override void OnEnd()
	{
		_movement.PlayerInput = Vector2.zero;
	}

	private SharedVector3 SelectFleeTarget()
	{
		SharedVector3 directionToLeftTarget;
		SharedVector3 directionToRightTarget;
		SharedVector3 selectedTarget;

		directionToLeftTarget = new Vector3(_target.Value.transform.position.x - _fleeDistance.Value, transform.position.y, 0);
		directionToRightTarget = new Vector3(_target.Value.transform.position.x + _fleeDistance.Value, transform.position.y, 0);

		if (Vector3.Distance(directionToLeftTarget.Value, transform.position) < Vector3.Distance(directionToRightTarget.Value, transform.position))
		{
			if (IsHitWall() == true)
				selectedTarget = directionToRightTarget;
			else
				selectedTarget = directionToLeftTarget;
		}
		else
		{
			if (IsHitWall() == true)
				selectedTarget = directionToLeftTarget;
			else
				selectedTarget = directionToRightTarget;
		}

		return selectedTarget;
	}

	private bool GetProbabilitySuccess(float probability)
	{ 
		if (Random.Range(0, 100f) < probability)
			return true;
		else 
			return false;
	}

	private bool IsHitWall()
	{
		if (Physics2D.Raycast(transform.transform.position, Vector2.left, _wallCheckDistance.Value, _layerMask)
			|| Physics2D.Raycast(transform.transform.position, Vector2.right, _wallCheckDistance.Value, _layerMask))
		{ 
			return true;
		}
		
		return false;
	}
}
