using Godot;
using System;

public partial class ToitRumble : AudioStreamPlayer2D
{
	public void PlayRumble()
	{
		GD.Print("Play rumble");	
		Play();
	}
	
}
