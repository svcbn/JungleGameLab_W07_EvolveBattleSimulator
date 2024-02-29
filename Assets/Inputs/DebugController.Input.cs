using System;
using UnityEngine.InputSystem;

partial class DebugController
{
	private DebugInputActions _inputActions;

	private DebugInputContext _inputContext;

	private void EnableInput()
	{
		_inputActions ??= new();
		_inputContext ??= new(this);
		_inputActions.Debug.SetCallbacks(_inputContext);
		Managers.Input.RegisterInputActions(_inputActions, "DEBUG");
		_inputActions.Enable();
	}

	private class DebugInputContext : DebugInputActions.IDebugActions
	{
		private readonly DebugController _controller;

		public DebugInputContext(DebugController controller)
		{
			_controller = controller;
		}

		public void OnReturn(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				if (_controller._isToggled)
				{
					_controller.ExecuteCommand();
				}
			}
		}

		public void OnToggle(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				_controller._isToggled = !_controller._isToggled;
				Action handlePlayerInput = _controller._isToggled ? _controller._disablePlayerInput : _controller._enablePlayerInput;
				handlePlayerInput?.Invoke();
			}
		}
	}
}