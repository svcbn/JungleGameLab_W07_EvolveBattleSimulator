using System;
using UnityEngine;

[Serializable]
public class SkillSelectorInput
{
	[SerializeField]
	private KeyCode _up;

	[SerializeField]
	private KeyCode _down;

	[SerializeField]
	private KeyCode _left;

	[SerializeField]
	private KeyCode _right;

	[SerializeField]
	private KeyCode _select;

	[SerializeField]
	private string _owner;

	[SerializeField]
	private Color _color;

	public KeyCode Up => _up;

	public KeyCode Down => _down;

	public KeyCode Left => _left;

	public KeyCode Right => _right;

	public KeyCode Select => _select;
	
	public string Owner => _owner;
	
	public Color Color => _color;
}
