using Godot;
using System;

public partial class ClosetGameReadyStateHandler : RigidBody2D
{
	public override void _Ready()
	{
		GD.Print("_Ready on ClosetGameReadyStateHandler");

		GameManager.Instance.GameReady += OnGameReady;

		Freeze = true;
	}

	public void OnGameReady()
	{
		GD.Print("Game ready on ClosetGameReadyStateHandler");

		FreezeMode = FreezeModeEnum.Static;
		Freeze = false;
	}

	private void OnTreeExiting()
	{
		GameManager.Instance.GameReady -= OnGameReady;
	}
}
