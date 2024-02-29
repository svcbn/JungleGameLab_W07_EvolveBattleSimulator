using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
	private Dictionary<string, object> _data = new();

	public T Load<T>(string name = null)
		where T : class
	{
		if (string.IsNullOrEmpty(name))
		{
			name = typeof(T).Name;
		}

		if (_data.TryGetValue(name, out var cache))
		{
			return cache as T;
		}

		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{name}");
		var data = JsonUtility.FromJson<T>(textAsset.text);
		_data.Add(name, data);

		return data;
	}
}