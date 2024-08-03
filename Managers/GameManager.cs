using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;

	[Signal]
	public delegate void GameReadyEventHandler();

	public bool IsGameReady = false;

	public AnimationPlayer AnimationPlayer { get; set; }

	public override void _EnterTree()
	{
		GD.Print("GameManager entering tree");
		base._EnterTree();
		if (Instance != null)
			QueueFree();
		else
			Instance = this;
	}

	public override void _Ready()
	{
		base._Ready();
		GD.Print("_Ready on GameManager");
		AnimationPlayer.Play("plane_anim");
	}

	public void OnGameReady()
	{
		IsGameReady = true;
		EmitSignal(SignalName.GameReady);
		GD.Print("Game ready");
	}
}
