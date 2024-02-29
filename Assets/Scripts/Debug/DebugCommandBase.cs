public abstract class DebugCommandBase
{
	public DebugCommandBase(string id, string description, string format)
	{
		Id = id;
		Description = description;
		Format = format;
	}

	public string Id { get; }

	public string Description { get; }

	public string Format { get; }

	public abstract void Execute(object parameter = null);
}