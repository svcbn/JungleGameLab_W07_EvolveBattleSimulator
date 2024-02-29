using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
	private static Managers s_Instance;

	private DataManager _data = new();

	private InputManager _input = new();

	private PoolManager _pool = new();

	private ResourceManager _resource = new();

	private UIManager _ui = new();

	private StatManager _stat = new();

	public static Managers Instance
	{
		get
		{
			Init();
			return s_Instance;
		}
	}

	public static DataManager Data => Instance._data;

	public static InputManager Input => Instance._input;

	public static PoolManager Pool => Instance._pool;

	public static ResourceManager Resource => Instance._resource; 

	public static UIManager UI => Instance._ui;

	public static StatManager Stat => Instance._stat;

	private static void Init()
	{
		if (s_Instance == null)
		{
			if (GameObject.Find("@Managers") is not { } managersRoot)
			{
				managersRoot = new() { name = "@Managers" };
			}

			s_Instance = managersRoot.GetOrAddComponent<Managers>();
			DontDestroyOnLoad(s_Instance);

			s_Instance._input.Init();
			s_Instance._stat.Init();
			s_Instance._ui.Init();
		}
	}
}