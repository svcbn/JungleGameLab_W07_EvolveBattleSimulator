using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class UISkillSlot : UIBase
{
	private enum Images
	{
		Circle,
		Square,
		Mask,
		Icon,
		Dim,
		CooldownIndicator,
	}

	private enum Texts
	{
		PresentText
	}

	private static readonly Color s_SelectedColor = new(1, 0, 0, 0.5f);

	private static readonly Color s_UnselectedColor = new(1, 1, 1, 0.5f);

	private static readonly float s_SelectedScale = 1.1f;

	private SkillInfo _info;

	private ISkill _skill;

	private Image _border;

	private Color _selectedColor = s_SelectedColor;
	
	private Coroutine _colorHandler;
	
	private Coroutine _scaleHandler;

	public SkillInfo SkillInfo => _info;

	protected override void Awake()
	{
		base.Awake();
	}

	private void OnEnable()
	{
		IsEnabled = true;
		if (_border != null)
		{
			_border.color = s_UnselectedColor;
		}
		SetBorder();
		Get<Image>((int)Images.Dim).gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		UnregisterSkillEvents();
		if (_colorHandler != null)
		{
			Utility.StopCoroutine(_colorHandler);
		}

		if (_scaleHandler != null)
		{
			Utility.StopCoroutine(_scaleHandler);
		}

		if (_border != null)
		{
			_border.enabled = false;
		}
	}

	public override void Init()
	{
		Bind<Image, Images>();
		Bind<TextMeshProUGUI, Texts>();
		
		if (_info != null)
		{
			SetSkillIcon();
		}

		if (_skill != null)
		{
			RemoveBorderRect();
		}
		
		SetBorder();
	}

	public void SetSkill(ISkill skill)
	{
		UnregisterSkillEvents();
		_skill = skill;
		RegisterSkillEvents();
		RemoveBorderRect();
		SetBorder();

		Get<Image, Images>(Images.CooldownIndicator).fillAmount = 0;
		Get<TextMeshProUGUI, Texts>(Texts.PresentText).enabled = _skill is IPassiveSkill { HasPresentNumber: true };
	}

	public void SetInfo(SkillInfo info)
	{
		_info = info;
		SetSkillIcon();
		SetBorder();
	}
	
	public void SetBorderColor(Color color, bool overrideAlpha = true)
	{
		if (_border is null)
		{
			return;
		}

		_selectedColor = overrideAlpha ? color : new Color(color.r, color.g, color.b, s_SelectedColor.a);
		_border.color = _selectedColor;
	}

	private void SetSkillIcon()
	{
		var icon = Get<Image>((int)Images.Icon);
		if (_info != null)
		{
			icon.sprite = _info.Sprite;
		}
	}

	private void RemoveBorderRect()
	{
		var rect = Get<Image>((int)Images.Mask).GetComponent<RectTransform>();
		if (rect == null)
		{
			return;
		}

		rect.offsetMin = rect.offsetMax = Vector2.zero;
	}

	private void SetBorder()
	{
		Images borderType = Images.Square;
		if (_skill is IPassiveSkill || _info?.SkillType == SkillType.Passive)
		{
			borderType = Images.Circle;
			ShowMask();
		}
		else
		{
			HideMask();
		}

		Get<Image, Images>(Images.Circle).enabled = false;
		Get<Image, Images>(Images.Square).enabled = false;

		_border = Get<Image, Images>(borderType);
		_border.enabled = true;
		Get<Image, Images>(Images.Mask).transform.SetParent(_border.transform);
	}

	private void ShowMask()
	{
		var mask = Get<Image, Images>(Images.Mask);
		mask.color = new(mask.color.r, mask.color.g, mask.color.b, 1f);
		mask.GetComponent<Mask>().enabled = true;
	}

	private void HideMask()
	{
		var mask = Get<Image, Images>(Images.Mask);
		mask.color = new(mask.color.r, mask.color.g, mask.color.b, 0f);
		mask.GetComponent<Mask>().enabled = false;
	}
}
