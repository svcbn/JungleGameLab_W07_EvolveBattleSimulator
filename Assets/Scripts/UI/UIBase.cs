using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
	private readonly Dictionary<Type, UnityEngine.Object[]> _objects = new();

	protected virtual void Awake()
	{
		Init();
	}

	public abstract void Init();

	public void RegisterEvent(Action<PointerEventData> handler, UIEventType eventType = UIEventType.Click)
	{
		var eventHandlerComponent = gameObject.GetOrAddComponent<UIEventHandler>();
		switch (eventType)
		{
			case UIEventType.Click:
				eventHandlerComponent.Clicked -= handler;
				eventHandlerComponent.Clicked += handler;
				break;
			case UIEventType.PointerEnter:
				eventHandlerComponent.PointerEntered -= handler;
				eventHandlerComponent.PointerEntered += handler;
				break;
			case UIEventType.PointerExit:
				eventHandlerComponent.PointerExited -= handler;
				eventHandlerComponent.PointerExited += handler;
				break;
		}
	}

	protected void Bind<TObject, TType>()
		where TObject : UnityEngine.Object
		where TType : Enum
	{
		string[] names = Enum.GetNames(typeof(TType));
		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(TObject), objects);
		for (int index = 0; index < names.Length; index++)
		{
			objects[index] = typeof(TObject) == typeof(GameObject) ? gameObject.FindChild(names[index], true) : gameObject.FindChild<TObject>(names[index], true);

			if (objects[index] == null)
			{
				Debug.Log($"Failed to bind: {names[index]}");
			}
		}
	}

	protected T Get<T, TEnum>(TEnum index)
		where T : UnityEngine.Object
		where TEnum : Enum
	{
		if (!_objects.TryGetValue(typeof(T), out var objects))
		{
			return null;
		}

		return objects[Convert.ToInt32(index)] as T;
	}

	protected T Get<T>(int index)
		where T : UnityEngine.Object
	{
		if (!_objects.TryGetValue(typeof(T), out var objects))
		{
			return null;
		}

		return objects[index] as T;
	}
}