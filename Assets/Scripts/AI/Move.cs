using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TMPro;
using UnityEngine;


public class Move : Action
{
	[SerializeField] 
	private SharedFloat _arriveDistance;

	[SerializeField]
	private SharedGameObject _target;

	[SerializeField]
	private SharedBool _canMove;

	private CharacterMovement _movement;
	private CharacterJump _jump;


	public override void OnAwake()
	{
		base.OnAwake();

		_movement =  GetComponent<CharacterMovement>();
		_jump = GetComponent<CharacterJump>();
	}

	public override void OnStart()
	{
		base.OnStart();
	}

	public override TaskStatus OnUpdate()
	{
		if (Vector2.Distance(_target.Value.transform.position, transform.position) > _arriveDistance.Value
			&& _canMove.Value == true)
		{
			var directionToTarget = _target.Value.transform.position - transform.position;

			_movement.PlayerInput = directionToTarget.normalized;

			//if (GetProbabilitySuccess(1f))
			//	_jump.OnJump(_movement.PlayerInput);

			return TaskStatus.Running;
		}

		return TaskStatus.Success;
	}

	public override void OnEnd()
	{
		_movement.PlayerInput = Vector2.zero;

		// 한 명이 적이 있는 방향의 반대방향을 바라본 채로 멈추는 현상 있음. 개선 시도한 코드.
		// float x = (_target.Value.transform.position - transform.position).x < 0 ? -1 : 1;
		// transform.localScale = new ( x * transform.localScale.x, 
		// 								 transform.localScale.y, 
		// 								 transform.localScale.z );
	}

	private bool GetProbabilitySuccess(float probability)
	{
		if (Random.Range(0, 100f) < probability)
			return true;
		else
			return false;
	}
}
