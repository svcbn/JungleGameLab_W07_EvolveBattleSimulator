using System.ComponentModel;

public interface IPassiveSkill : ISkill
{
	bool IsEnabled { get; set; }

	bool HasPresentNumber { get; }

	int PresentNumber { get; }
}