using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireBallProjectile : MonoBehaviour
{
	public float speed;
	public float delay;
	public int targetIndex;
	public float lifespan;
	public int damage;

	private Character _targetCharacter;
	private Vector2 _direction;
	private float _actualSpeed;
	private float _startTime;

	private void Start()
	{
		_startTime = Time.time;
		_targetCharacter = Managers.Stat.Characters[targetIndex];
		_actualSpeed = 0;
		_direction = _targetCharacter.transform.position - transform.position;
		float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);

		Invoke("ShootProjectile", delay);
	}

	void ShootProjectile()
	{
		_actualSpeed = speed;
	}

	private void Update()
	{
		transform.position += (Vector3)_direction * (_actualSpeed * Time.deltaTime);

		if (Time.time > _startTime + lifespan)
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<Character>(out var hitCharacter))
		{
			if (hitCharacter.playerIndex == targetIndex)
			{
				Managers.Stat.GiveDamage(targetIndex, damage);
				hitCharacter.Status.SetKnockbackEffect(.5f, 10f, transform.position);

				gameObject.SetActive(false);
				Destroy(gameObject);
			}
		}
	}
}