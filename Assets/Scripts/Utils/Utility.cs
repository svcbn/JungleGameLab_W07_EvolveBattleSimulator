using System.Collections;
using UnityEngine;

public static class Utility
{
	public static Coroutine Lerp(float from, float to, float duration, System.Action<float> action, System.Action callback = null)
	{
		return Managers.Instance.StartCoroutine(LerpCoroutine(from, to, duration, action, callback));
	}

	public static Coroutine Lerp(Vector3 from, Vector3 to, float duration, System.Action<Vector3> action, System.Action callback = null)
	{
		return Managers.Instance.StartCoroutine(LerpCoroutine(from, to, duration, action, callback));
	}

	public static void StopCoroutine(Coroutine coroutine)
	{
		if (coroutine != null)
		{
			Managers.Instance.StopCoroutine(coroutine);
		}
	}

	private static IEnumerator LerpCoroutine(float from, float to, float duration, System.Action<float> action, System.Action callback)
	{
		float time = 0;
		while (time < duration)
		{
			time += Time.deltaTime;
			float value = Mathf.Lerp(from, to, time / duration);
			action?.Invoke(value);
			yield return null;
		}
		action?.Invoke(to);

		callback?.Invoke();
	}

	private static IEnumerator LerpCoroutine(Vector3 from, Vector3 to, float duration, System.Action<Vector3> action, System.Action callback)
	{
		float time = 0;
		while (time < duration)
		{
			time += Time.deltaTime;
			Vector3 value = Vector3.Lerp(from, to, time / duration);
			action?.Invoke(value);
			yield return null;
		}
		action?.Invoke(to);

		callback?.Invoke();
	}
}
