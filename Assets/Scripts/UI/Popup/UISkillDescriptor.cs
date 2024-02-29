using System;
using TMPro;
using UnityEngine;

public class UISkillDescriptor : UIPopup
{
	private enum Elements
	{
		Name,
		SkillType,
		BeforeDelay,
		Cooldown,
		Description
	}

	private static readonly Vector3 s_InitialScale = new(0.95f, 0.95f, 0.95f);

	private Coroutine _scaleHandler;

	private SkillInfo _skillInfo;

	private RectTransform _card;

	private void OnDisable()
	{
		Utility.StopCoroutine(_scaleHandler);
	}

	public override void Init()
	{
		base.Init();
		Bind<TextMeshProUGUI, Elements>();
		SetText();
		
		_card = gameObject.FindChild<RectTransform>("Card");
	}

	public void SetSkillInfo(SkillInfo skillInfo)
	{
		_skillInfo = skillInfo;
		SetText();
		SetScaleEffect();
	}

	private void SetText()
	{
		if (_skillInfo == null)
		{
			return;
		}

		Get<TextMeshProUGUI>((int)Elements.Name).text = _skillInfo.Name;
		Get<TextMeshProUGUI>((int)Elements.SkillType).text = _skillInfo.SkillType == SkillType.Passive ? "Passive" : "Active";
		Get<TextMeshProUGUI>((int)Elements.Cooldown).text = $"쿨타임: {_skillInfo.CoolDown}초";
		var beforeDelay = Get<TextMeshProUGUI, Elements>(Elements.BeforeDelay);
		beforeDelay.gameObject.SetActive(_skillInfo.SkillType != SkillType.Passive);
		beforeDelay.text = $"시전 시간: {_skillInfo.BeforeDelay}초";
		Get<TextMeshProUGUI>((int)Elements.Description).text = _skillInfo.Description;
	}

	private void SetScaleEffect()
	{
		if (_card != null)
		{
			_scaleHandler = Utility.Lerp(s_InitialScale, Vector3.one, 0.1f, scale => _card.localScale = scale);
		}
	}
}
