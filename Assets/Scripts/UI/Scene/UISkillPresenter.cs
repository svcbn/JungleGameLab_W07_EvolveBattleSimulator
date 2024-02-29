using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillPresenter : UIScene
{
	private enum Elements
	{
		Mask,
		Icon
	}

	private Image _mask;

	private Image _icon;
	
	private Character _character;
	
	private float _originalMaskAlpha;
	
	private float _originalIconAlpha;
	
	private bool _isFeedbackFinished = true;
	
	private bool _isCoroutineRunning = false;

	private void Update()
	{
		if (_character != null)
		{
			transform.position = _character.transform.position + Vector3.up * 2f;
			UpdateCooldown();
			SetIcon();
		}
	}

	public override void Init()
	{
		Bind<Image, Elements>();
		_mask = Get<Image>((int)Elements.Mask);
		_icon = Get<Image>((int)Elements.Icon);
		_originalIconAlpha = _icon.color.a;
		_originalMaskAlpha = _mask.color.a;
		//SetIcon();
	}

	public void SetSkill(Character character)
	{
		_character = character;
		//SetIcon();
	}

	private void UpdateCooldown()
	{
		if (_character.CurrentSkill == null)
		{
			return;
		}
		
		if (!_isCoroutineRunning && !_isFeedbackFinished && _mask.fillAmount >= 1f)
		{
			_isCoroutineRunning = true;
			//_isSkillChanged = false;
			Utility.Lerp(_icon.color.a, 0.1f, 0.15f,
				value => _icon.color = new(_icon.color.r, _icon.color.g, _icon.color.b, value), () => _mask.fillAmount = 0);
			Utility.Lerp(transform.localScale, Vector3.one * 2f, 0.15f, value => transform.localScale = value, () =>
			{
				_isCoroutineRunning = false;
				_isFeedbackFinished = true;
				_mask.color = new(_mask.color.r, _mask.color.g, _mask.color.b, 0);
			});
		}
	}

	private void SetIcon()
	{
		if (_character == null || _character.CurrentSkill == null)
		{
			_mask.color = new(_mask.color.r, _mask.color.g, _mask.color.b, 0);
			return;
		}

		_mask.fillAmount = _character.CurrentSkill.CurrentCooldown / _character.CurrentSkill.BeforeDelay;

		if (_mask.fillAmount < 1 && _isFeedbackFinished)
		{
			_mask.color = new(_mask.color.r, _mask.color.g, _mask.color.b, _originalMaskAlpha);
		
			SkillInfo info = GameManager.Skill.GetInfo(_character.CurrentSkill.Id);
			if (info != null)
			{
				_icon.sprite = info.Sprite;
			}

			_icon.color = new(_icon.color.r, _icon.color.g, _icon.color.b, _originalIconAlpha);
			transform.localScale = Vector3.one;
			_isFeedbackFinished = false;	
		}
	}

}
