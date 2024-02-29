using BehaviorDesigner.Runtime.Tasks;

public class SetMoveBTVariables : Action
{
	private Character _character;

	public override void OnAwake()
	{
		base.OnAwake();

		_character = GetComponent<Character>();
		//_character.SetMoveBTVariables();
	}

	public override TaskStatus OnUpdate()
	{
		//_character.SetMoveBTVariables();
		return TaskStatus.Success;
	}

}
