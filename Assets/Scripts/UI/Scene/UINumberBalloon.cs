using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINumberBalloon : UIScene
{
	private string _text;

	private Color _color = Color.black;

	private Vector3 _position;

	private Coroutine _positionHandler;
	
	private Coroutine _colorHandler;
	
	private float _yOffset = 5f;

	public Vector3 Position
	{
		get => _position;
		set => _position = value;
	}

	private enum Texts
	{
		BalloonInnerText
	}

	private void OnEnable()
	{
		_yOffset = 5f;
		SetPositionEffect();
	}

	private void OnDisable()
	{
		if (_positionHandler != null)
		{
			Utility.StopCoroutine(_positionHandler);
			_positionHandler = null;
		}

		if (_colorHandler != null)
		{
			Utility.StopCoroutine(_colorHandler);
			_colorHandler = null;
		}

		_position = Vector3.zero;
	}

	public override void Init()
	{
		var canvas = GetComponent<Canvas>();
		canvas.sortingOrder = 99;
		
		Bind<TextMeshProUGUI, Texts>();
		SetInnerText();
	}
	
	public void SetInitialPosition(Vector3 position)
	{
		_position = position;
		transform.position = _position;

		if (_positionHandler != null)
		{
			Utility.StopCoroutine(_positionHandler);
			SetPositionEffect();
		}
	}

	public void SetText(string text, Color color)
	{
		_text = text;
		_color = color;
		SetInnerText();
	}

	private void SetInnerText()
	{
		TextMeshProUGUI textBox = Get<TextMeshProUGUI, Texts>(Texts.BalloonInnerText);
		textBox.text = _text;
		textBox.color = _color;
		if (_colorHandler != null)
		{
			Utility.StopCoroutine(_colorHandler);
		}
		SetColorEffect();
	}

	private void SetPositionEffect()
	{
		float end = _yOffset;
		_positionHandler = Utility.Lerp(0, end, 1.5f, yOffset => transform.position = _position + Vector3.up * yOffset, () =>
		{
			_positionHandler = null;
			Managers.Resource.Release(gameObject);
		});
	}

	private void SetColorEffect()
	{
		TextMeshProUGUI textBox = Get<TextMeshProUGUI, Texts>(Texts.BalloonInnerText);
		_colorHandler = Utility.Lerp(1, 0f, 1f, alpha => textBox.color = new(textBox.color.r, textBox.color.g, textBox.color.b, alpha), () => _colorHandler = null);
	}
}
