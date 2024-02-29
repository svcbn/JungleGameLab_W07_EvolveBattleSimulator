using BehaviorDesigner.Runtime.Tasks;

public class Jump : Action
{
	private CharacterJump _jump;
	private CharacterMovement _movement;

	public override void OnStart()
	{
		base.OnStart();

		_jump = GetComponent<CharacterJump>();
		_movement = GetComponent<CharacterMovement>();
	}

	public override TaskStatus OnUpdate()
	{
		_jump.OnJump(_movement.LookDirction);
		return TaskStatus.Success;
	}
}
