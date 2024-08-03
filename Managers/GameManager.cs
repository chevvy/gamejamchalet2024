using Godot;
using System;

public partial class GameManager : Node
{
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void GameReadyEventHandler(); 
	[Export]
	public AnimationPlayer AnimationPlayer { get; set; }


	public override void _Ready()
	{
		base._Ready();
		AnimationPlayer.Play("plane_anim");
	}

	public void OnGameReady()
	{
		EmitSignal(SignalName.GameReady);
		GD.Print("Game ready");
	}
}
