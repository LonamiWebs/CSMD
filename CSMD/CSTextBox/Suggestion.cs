using System;

public class Suggestion
{
	public readonly string Name;
	public readonly string Type;
	public readonly string Namespace;
	
	public Suggestion(string name, string type, string @namespace)
	{
		Name = name;
		Type = type;
		Namespace = @namespace;
	}
}
