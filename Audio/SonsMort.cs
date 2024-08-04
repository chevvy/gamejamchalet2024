using Godot;
using System;

public partial class SonsMort : AudioStreamPlayer2D
{
	public static SonsMort Instance;
	public override void _EnterTree()
	{
		GD.Print("GameManager entering tree");
		base._EnterTree();
		if (Instance != null)
			QueueFree();
		else
			Instance = this;
	}
	

	public void OnDeathSounds()
	{
		if (!Playing)
		{
			Play();
		}
	}
}
