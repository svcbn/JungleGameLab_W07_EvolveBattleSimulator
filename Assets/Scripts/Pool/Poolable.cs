using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
	public IObjectPool<Poolable> Pool { get; set; }
}