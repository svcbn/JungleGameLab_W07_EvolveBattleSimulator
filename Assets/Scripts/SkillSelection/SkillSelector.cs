using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SkillSelector
{
	private SkillSelectorInput _input;
	
	private int _selectedCount = 0;
	
	public SkillSelector(IList<SkillInfo> skills)
	{
		Skills = new ReadOnlyCollection<SkillInfo>(skills);
	}

	public Action<SkillInfo> SkillSelected;

	public Action<SkillSelectorInput> InputChanged;

	public SkillSelectorInput Input
	{
		get => _input;
		set
		{
			_input = value;
			InputChanged?.Invoke(_input);
		}
	}

	public IReadOnlyList<SkillInfo> Skills { get; private set; }

	public bool CanSelect => _selectedCount < Skills.Count;

	public void SelectSkill(SkillInfo skill)
	{
		_selectedCount++;
		SkillSelected?.Invoke(skill);
	}

	public void SelectSkill(int index)
	{
		SelectSkill(Skills[index]);
	}
}
