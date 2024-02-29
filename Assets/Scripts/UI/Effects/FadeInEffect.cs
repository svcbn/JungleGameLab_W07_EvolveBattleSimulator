using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
	[SerializeField]
	private float _duration = 0.3f;

	private Image _image;

	private float _alpha;

	private Coroutine _colorHandler;

	private void Awake()
	{
		_image = GetComponent<Image>();
		_alpha = _image.color.a;
	}

	private void OnEnable()
	{
		_colorHandler = Utility.Lerp(0, _alpha, _duration, (value) =>
		{
			var color = _image.color;
			color.a = value;
			_image.color = color;
		});
	}

	private void OnDisable()
	{
		Utility.StopCoroutine(_colorHandler);
	}
}
