using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShootProjectile : MonoBehaviour
{
	public Transform target;         // 추적할 목표물(Transform)
	public float speed = 7.0f;       // 발사체 속도
	public float rotationSpeed = 0.0001f; // 발사체 회전 속도

	private Vector2 direction;

	private void Start()
	{
		//StartCoroutine(nameof(DelayRotation));
		direction = Vector2.up;
	}

	private void FixedUpdate()
	{
		Vector2 dir = transform.right;
		Vector2 targetDir = target.position - transform.position;
		// 바라보는 방향과 타겟 방향 외적
		Vector3 crossVec = Vector3.Cross(dir, targetDir);
		// 상향 벡터와 외적으로 생성한 벡터 내적
		float inner = Vector3.Dot(Vector3.forward, crossVec);
		// 내적이 0보다 크면 오른쪽 0보다 작으면 왼쪽으로 회전
		float addAngle = inner > 0 ? rotationSpeed * Time.fixedDeltaTime : -rotationSpeed * Time.fixedDeltaTime;
		float saveAngle = addAngle + transform.rotation.eulerAngles.z;
		transform.rotation = Quaternion.Euler(0, 0, saveAngle);

		float moveDirAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
		direction = new Vector2(Mathf.Cos(moveDirAngle), Mathf.Sin(moveDirAngle));

		transform.position = direction * speed * Time.fixedDeltaTime;
	}

	//IEnumerator DelayRotation()
	//{
	//	while(true) 
	//	{
	//		if (target == null)
	//		{
	//			yield break; // 목표물이 없으면 아무 작업도 수행하지 않음
	//		}

	//		// 목표 방향 계산
	//		direction = (Vector2)target.position - (Vector2)transform.position;
	//		direction.Normalize();
			

	//		// 발사체 회전
	//		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	//		Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
	//		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

	//		yield return new WaitForSeconds(0.1f);

	//	}

	//}
}
