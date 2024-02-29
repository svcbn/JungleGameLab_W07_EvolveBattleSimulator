using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameUIManager
{
	private UISkillSelector _skillSelector;

	private UIMenu _menu;

	private UITimer _timer;

	public void ShowSkillSelector(SkillSelector selector, List<int> order, Color[] colors)
	{
		if (_skillSelector != null)
		{
			Managers.Resource.Release(_skillSelector.gameObject);
		}

		_skillSelector = Managers.UI.ShowPopupUI<UISkillSelector>();
		_skillSelector.SetSelector(selector);
		_skillSelector.SetOrderPresenter(order, colors);
	}

	public void HideSkillSelector()
	{
		Managers.Resource.Release(_skillSelector.gameObject);
		_skillSelector = null;
	}

	public void ShowSkillList(Character left, Character right)
	{
		var list = Managers.UI.ShowSceneUI<UISkillList>();
		list.RegisterCharacter(left, right);
	}

	/// <summary>
	/// Show a popup that shows the amount of damage or healing.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="number">대미지면 0 이하의 숫자, 힐이면 0 이상의 숫자를 입력</param>
	public void ShowHealthPopup(Character character, int number)
	{
		Color color;
		if (number < 0)
		{
			color = number <= -10 ? Color.red : Color.white;
		}
		else if (number > 0)
		{
			color = Color.green;
		}
		else { color = Color.gray; }

		number = Mathf.Abs(number);

		if (!_balloons.TryGetValue(character, out var balloons))
		{
			balloons = new();
			_balloons.Add(character, balloons);
		}

		for (int index = 0; index < balloons.Count; index++)
		{
			if (balloons[index].Position.y == 0)
			{
				balloons.RemoveAt(index);
				index--;
			}
		}

		var adjacentBalloons = balloons.Where(balloon => balloon.Position.y != 0).
			Where(balloon => Mathf.Abs(balloon.Position.x - character.transform.position.x) < 5f).
			ToList();
		Vector3 characterPosition = character.transform.position;
		Vector3 position = adjacentBalloons.Count == 0
			? characterPosition + Vector3.up
			: new(characterPosition.x, adjacentBalloons.Min(balloon => balloon.Position.y), characterPosition.z);

		foreach (var balloon in adjacentBalloons)
		{
			balloon.Position += Vector3.up * 0.5f;
		}
		
		var newBalloon = Managers.UI.ShowSceneUI<UINumberBalloon>();
		newBalloon.SetInitialPosition(position);
		newBalloon.SetText(number.ToString(), color);
		_balloons[character].Add(newBalloon);
	}
	
	private Dictionary<Character, List<UINumberBalloon>> _balloons = new();

	public void ShowTitle(Action onStart)
	{
		Managers.UI.ClearAllPopup();

		var title = Managers.UI.ShowPopupUI<UITitle>();
		title.StartButtonClicked += () => Managers.UI.ClosePopupUI(title);
		title.StartButtonClicked += onStart;
	}

	public void ShowMenu(Action goback = null, Action exit = null)
	{
		if (_menu != null)
		{
			Managers.UI.ClosePopupUI(_menu);
			_menu = null;
			return;
		}

		_menu = Managers.UI.ShowPopupUI<UIMenu>(usePool: false);
		RegisterMenuEvents(_menu);
		_menu.GoBackButtonClicked += goback;
		_menu.ExitButtonClicked += exit;
		
		return;

		static void RegisterMenuEvents(UIMenu menu)
		{
			menu.GoBackButtonClicked += () => Managers.UI.ClosePopupUI(menu);
			menu.ExitButtonClicked += () => Managers.UI.ClosePopupUI(menu);
		}
	}
	
	public void SetSkillPresenter(Character character)
	{
		var presenter = Managers.UI.ShowSceneUI<UISkillPresenter>();
		presenter.SetSkill(character);
	}

	public void UpdateTimer(int seconds)
	{
		if (_timer == null)
		{
			_timer = Managers.UI.ShowSceneUI<UITimer>();			
		}
		
		_timer.SetTimerText($"{seconds}");
	}
}
