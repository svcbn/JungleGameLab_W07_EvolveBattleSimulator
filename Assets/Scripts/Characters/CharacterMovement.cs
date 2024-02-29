using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[SerializeField]
	public CharacterMovementData ChractorMovementData;
	public float CurrentSpeed { get; set; }

	public Vector2 LookDirction;

	public Vector2 PlayerInput { 
		get { return _playerInput; }
		set
		{
			_playerInput = Vector2.ClampMagnitude(value, 1f);
			
			if (_playerInput.x != 0)
				LookMoveDirction(_playerInput);
		}
	}
	Vector2 _playerInput;

	Vector3 _velocity;

	Character _character;
	CharacterStatus _status;

	private void Awake()
	{
		_character = GetComponent<Character>();
		_status = GetComponent<CharacterStatus>();
	}

	private void Start()
	{
		CurrentSpeed = ChractorMovementData.MaxSpeed;
	}

	private void FixedUpdate()
	{
		if (_character.CanMove == false) return;

		CurrentSpeed = ChractorMovementData.MaxSpeed * _status.SpeedChange;
		Vector3 desiredVelocity = new Vector3(PlayerInput.x, 0, 0) * CurrentSpeed;
		float maxSpeedChange = ChractorMovementData.MaxAcceleration * Time.deltaTime;

		_velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, maxSpeedChange);

		Vector3 displacement = _velocity * Time.deltaTime;
		transform.localPosition += displacement;
	}

	public void LookMoveDirction(Vector3 moveDirction)
	{
		LookDirction = new Vector2(Mathf.Abs(transform.localScale.x) * (moveDirction.x > 0 ? 1 : -1), 0) ; 
		transform.localScale = new Vector3(LookDirction.x, transform.localScale.y, transform.localScale.z);
	}
}
