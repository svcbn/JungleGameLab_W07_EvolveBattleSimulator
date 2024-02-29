using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSelector : UIPopup
{
	private enum Elements
	{
		Items,
		Card,
		Descriptor,
		OrderPresenter
	}

	private enum Texts
	{
		PickerText,
		Up,
		Down,
		Left,
		Right,
		Select
	}
	
	private static readonly Color[] s_Colors = new[] { Color.red, Color.blue };
	
	private static readonly Vector3 s_InitialScale = new(0.95f, 0.95f, 0.95f);

	private Color _color;
	
	private int _playerIndex = 0;
	
	private readonly List<UISkillSlot> _slots = new();

	private SkillSelectorInput _input;

	private SkillSelector _selector;

	private UISkillDescriptor _descriptor;

	private TextMeshProUGUI _pickerText;
	
	private TextMeshProUGUI _upText;
	
	private TextMeshProUGUI _downText;
	
	private TextMeshProUGUI _leftText;
	
	private TextMeshProUGUI _rightText;
	
	private TextMeshProUGUI _selectText;
	
	private RectTransform _card;

	private int _currentIndex = -1;

	private int _columnCount = 3;
	
	private Coroutine _scaleHandler;

	private void Update()
	{
		if (_selector.Input == null)
		{
			return;
		}
		
		HandleDirectionInput();
		HandleSelect();
	}

	private void OnDisable()
	{
		ClearSlots();
		Utility.StopCoroutine(_scaleHandler);
	}

	private void HandleSelect()
	{
		if (_currentIndex == -1 || !Input.GetKeyDown(_input.Select))
		{
			return;
		}

		var slot = _slots[_currentIndex];
		if (!slot.IsEnabled)
		{
			return;
		}

		_selector.SelectSkill(_currentIndex);
		slot.SetBorderColor(_color);
		slot.ShowChoiceEffect();
		slot.Disable();
		var presenter = Get<GameObject, Elements>(Elements.OrderPresenter);
		Managers.Resource.Release(presenter.transform.GetChild(0).gameObject);
	}

	private void HandleDirectionInput()
	{
		Direction direction;
		if (Input.GetKeyDown(_input.Up))
		{
			direction = Direction.Up;
		}
		else if (Input.GetKeyDown(_input.Left))
		{
			direction = Direction.Left;
		}
		else if (Input.GetKeyDown(_input.Down))
		{
			direction = Direction.Down;
		}
		else if (Input.GetKeyDown(_input.Right))
		{
			direction = Direction.Right;
		}
		else
		{
			return;
		}

		Move(direction);
	}

	public override void Init()
	{
		base.Init();

		Bind<GameObject, Elements>();
		Bind<TextMeshProUGUI, Texts>();

		_pickerText = Get<TextMeshProUGUI>((int)Texts.PickerText);
		_upText = Get<TextMeshProUGUI>((int)Texts.Up);
		_downText = Get<TextMeshProUGUI>((int)Texts.Down);
		_leftText = Get<TextMeshProUGUI>((int)Texts.Left);
		_rightText = Get<TextMeshProUGUI>((int)Texts.Right);
		_selectText = Get<TextMeshProUGUI>((int)Texts.Select);

		_card = Get<GameObject>((int)Elements.Card).GetComponent<RectTransform>();
		
		_descriptor = Get<GameObject>((int)Elements.Descriptor).GetOrAddComponent<UISkillDescriptor>();
		
		InitializeSlots();
	}

	public void SetSelector(SkillSelector selector)
	{
		_selector = selector;
		_selector.InputChanged += input =>
		{
			if (_input != _selector.Input)
			{
				_input = _selector.Input;
				_playerIndex = ++_playerIndex % s_Colors.Length;
				_color = _input.Color.a > 0 ? _input.Color : s_Colors[_playerIndex];
			}

			SetInputText();
			
			_slots[_currentIndex].SetBorderColor(_color);
		};
		_input = selector.Input;
		_color = _input.Color.a > 0 ? _input.Color : s_Colors[_playerIndex];
		SetInputText();
		InitializeSlots();
	}
	
	public void SetOrderPresenter(List<int> order, Color[] colors)
	{
		var presenter = Get<GameObject, Elements>(Elements.OrderPresenter);
		if (presenter == null)
		{
			return;
		}
		
		foreach (Transform child in presenter.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}

		int playerIndex = 0;
		foreach (var index in order)
		{
			foreach (var _ in Enumerable.Range(0, index))
			{
				var go = Managers.Resource.Instantiate("UI/Popup/UIOrderPresenter");
				go.transform.SetParent(presenter.transform);
				go.GetOrAddComponent<Image>().color = colors[playerIndex];
			}
			playerIndex = ++playerIndex % colors.Length;
		}
	}

	private void SetInputText()
	{
		_pickerText.text = _input.Owner;
		foreach (var text in System.Enum.GetValues(typeof(Texts)).
			         Cast<Texts>().
			         Select(text => Get<TextMeshProUGUI>((int)text)))
		{
			text.color = _color;
		}
		
		_upText.text = _input.Up.ToString();
		_downText.text = _input.Down.ToString();
		_leftText.text = _input.Left.ToString();
		_rightText.text = _input.Right.ToString();
		_selectText.text = _input.Select.ToString();
		if (_card != null)
		{
			_scaleHandler = Utility.Lerp(s_InitialScale, Vector3.one, 0.1f, scale => _card.localScale = scale);
		}
	}

	private void InitializeSlots()
	{
		_currentIndex = -1;
		ClearSlots();

		if (_selector?.Skills?.Any() is true)
		{
			GameObject items = Get<GameObject>((int)Elements.Items);

			foreach (var info in _selector.Skills)
			{
				var slot = CreateSlot(info);
				slot.transform.SetParent(items.transform);
				slot.transform.localScale = Vector3.one;
				_slots.Add(slot);
			}

			SelectItem(0);
		}
	}

	private void ClearSlots()
	{
		GameObject items = Get<GameObject>((int)Elements.Items);
		foreach (Transform child in items.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}
		_slots?.Clear();
	}

	private UISkillSlot CreateSlot(SkillInfo info)
	{
		GameObject go = Managers.Resource.Instantiate("UI/Popup/UISkillSlot");
		
		var slot = go.GetOrAddComponent<UISkillSlot>();
		slot.SetInfo(info);
		
		return slot;
	}

	private void Move(Direction direction)
	{
		if (_slots?.Any() is not true)
		{
			return;
		}

		int childCount = _slots.Count;
		int row = _currentIndex / _columnCount;
		int column = _currentIndex % _columnCount;

		switch (direction)
		{
			case Direction.Left:
				column -= 1;
				break;
			case Direction.Right:
				column += 1;
				break;
			case Direction.Up:
				row -= 1;
				break;
			case Direction.Down:
				row += 1;
				break;
			case Direction.None:
				return;
		}

		// Check out of bounds
		if (row < 0 || row >= childCount / (float)_columnCount || column < 0 || column >= _columnCount)
		{
			return;
		}

		// Check out of range
		if (row * _columnCount + column >= childCount)
		{
			return;
		}

		int newIndex = row * _columnCount + column;
		SelectItem(newIndex);
	}

	private void SelectItem(int newIndex)
	{
		if (_currentIndex != -1)
		{
			_slots[_currentIndex].Unselect();
		}

		_currentIndex = newIndex;
		_slots[_currentIndex].SetBorderColor(_color);
		_slots[_currentIndex].Select();

		SetDescriptor();
	}

	private void SetDescriptor()
	{
		_descriptor.SetSkillInfo(_slots[_currentIndex].SkillInfo);
	}
}
