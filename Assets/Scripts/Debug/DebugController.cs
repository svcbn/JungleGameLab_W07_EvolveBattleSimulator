using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class DebugController : MonoBehaviour
{
	private List<DebugCommandBase> _commands;

	private bool _isToggled;

	private bool _showHelp;

	private string _commandString;

	private Vector2 _scrollView;

	private Action _enablePlayerInput;

	private Action _disablePlayerInput;

	private void Awake()
	{
		Init();
		EnableInput();
	}

	private void Start()
	{
		//var player = GameObject.Find("Player").GetComponent<PlayerController>();
		//_enablePlayerInput = () => typeof(PlayerController).GetMethod("EnableInput", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(player, null);
		//_disablePlayerInput = () => typeof(PlayerController).GetMethod("DisableInput", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(player, null);
	}

	private void OnGUI()
	{
		if (!_isToggled)
		{
			return;
		}

		int fontSize = Mathf.Max(15, (int)(Screen.height / 54f));
		GUI.skin.textField.fontSize = fontSize;
		GUI.skin.label.fontSize = fontSize;

		float y = 0f;

		if (_showHelp)
		{
			float helpBoxHeight = fontSize * 5f;
			GUI.Box(new(0, y, Screen.width, helpBoxHeight), string.Empty);
			Rect viewport = new(0, 0, Screen.width - 30, fontSize * _commands.Count);

			_scrollView = GUI.BeginScrollView(new(0, y + 5f, Screen.width, helpBoxHeight - 10f), _scrollView, viewport);
			foreach (var (command, index) in _commands.Select((command, index) => (command, index)))
			{
				Rect labelRect = new(5, (fontSize * 1.5f) * index, viewport.width - 100, fontSize * 1.5f);
				GUI.Label(labelRect, $"{command.Format} - {command.Description}");
			}

			GUI.EndScrollView();

			y += helpBoxHeight;
		}

		GUI.Box(new(0, y, Screen.width, fontSize * 2f), string.Empty);
		GUI.backgroundColor = new();
		GUI.SetNextControlName("textfield");
		_commandString = GUI.TextField(new(10f, y + (fontSize / 2f), Screen.width - 20f, fontSize * 1.5f), _commandString);
		GUI.FocusControl("textfield");
	}

	private void ExecuteCommand()
	{
		_showHelp = false;

		var (id, parameter) = ParseCommandString();
		foreach (var command in _commands.Where(command => command.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
		{
			command.Execute(parameter);
		}

		_commandString = string.Empty;
	}

	private (string Id, string Parameter) ParseCommandString()
	{
		int index;
		if ((index = _commandString.IndexOf(' ')) == -1)
		{
			return (_commandString, string.Empty);
		}

		return (_commandString[..index], _commandString[index..].TrimStart());
	}

	private void Init()
	{
		_commands = new()
		{
			new DebugCommand(
				id: "shine",
				description: "Shine the all skill slots",
				format: "shine",
				execute: () =>
				{
					MethodInfo methodInfo = typeof(UISkillSlot).GetMethod("ShowEnableEffect",
						BindingFlags.Instance | BindingFlags.NonPublic);
					
					if (methodInfo != null)
					{
						foreach (UISkillSlot slot in FindObjectsOfType<UISkillSlot>())
						{
							methodInfo.Invoke(slot, null);
						}
					}
				}),
			
			new DebugCommand<int>(
				id: "player1",
				description: "Reset skill cooldown of player 1",
				format: "player1 <skill index>",
				execute: index =>
				{
					Managers.Stat.Characters[0].Skills[index].Reset();
				},
				parser: int.TryParse),
			
			new DebugCommand<int>(
				id: "player2",
				description: "Reset skill cooldown of player 2",
				format: "player2 <skill index>",
				execute: index =>
				{
					Managers.Stat.Characters[1].Skills[index].Reset();
				},
				parser: int.TryParse),
			
			new TransformObjectCommand(
				id: "move",
				description: "Move the position of specified GameObject",
				format: "move <name> <position>",
				type: TransformObjectCommand.TransformType.Position),

			new TransformObjectCommand(
				id: "rotate",
				description: "Rotate the specified GameObject",
				format: "rotate <name> <rotation>",
				type: TransformObjectCommand.TransformType.Rotation),

			new DebugCommand<float>(
				id: "time_scale",
				description: "Change the time scale",
				format: "time_scale <scale>",
				execute: scale => Time.timeScale = scale,
				parser: float.TryParse),

			new DebugCommand(
				id: "reload",
				description: "Reload the current scene",
				format: "reload",
				execute: () => SceneManager.LoadScene(SceneManager.GetActiveScene().name)),

			new DebugCommand(
				id: "exit",
				description: "Exit the game",
				format: "exit",
				execute: () =>
				{
#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
#else
					Application.Quit();
#endif
				}),

			new DebugCommand("help", "Shows a list of commands", "help", () => _showHelp = true),
		};
	}
}