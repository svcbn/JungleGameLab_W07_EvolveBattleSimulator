using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

partial class UISkillSlot
{
	private void CheckCooldown()
	{
		Image image = Get<Image>((int)Images.CooldownIndicator);
		image.fillAmount = 1 - _skill.CurrentCooldown / _skill.Cooldown;
	}

	private void RegisterSkillEvents()
	{
		if (_skill != null)
		{
			_skill.PropertyChanged += OnSkillPropertyChanged;
		}
	}

	private void UnregisterSkillEvents()
	{
		if (_skill != null)
		{
			_skill.PropertyChanged -= OnSkillPropertyChanged;
		}
	}

	private void OnSkillPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(ISkill.CurrentCooldown):
				CheckCooldown();
				break;
			case nameof(IPassiveSkill.PresentNumber) when _skill is IPassiveSkill passiveSkill:
				SetPresentText(passiveSkill);
				break;
			case nameof(IPassiveSkill.IsEnabled) when _skill is IPassiveSkill { IsEnabled: true }:
				ShowEnableEffect();
				break;
		}
	}

	private void ShowEnableEffect()
	{
		Vector3 startScale = Vector3.one;
		Vector3 endScale = Vector3.one * 1.2f;
		const float startAlpha = 1f;
		const float endAlpha = 0.2f;
		const float duration = 0.12f;
		
		var icon = Get<Image, Images>(Images.Icon);
		_scaleHandler = Utility.Lerp(startScale, endScale, duration, vector => transform.localScale = vector,
			() => _scaleHandler = Utility.Lerp(endScale, startScale, duration, vector => transform.localScale = vector));
		_colorHandler = Utility.Lerp(startAlpha, endAlpha, duration, alpha => icon.color = new(icon.color.r, icon.color.g, icon.color.b, alpha),
			() => _colorHandler = Utility.Lerp(endAlpha, startAlpha, duration, alpha => icon.color = new(icon.color.r, icon.color.g, icon.color.b, alpha)));
	}

	private void SetPresentText(IPassiveSkill skill)
	{
		TextMeshProUGUI textBox = Get<TextMeshProUGUI, Texts>(Texts.PresentText);
		textBox.text = skill.PresentNumber.ToString();
	}
}
