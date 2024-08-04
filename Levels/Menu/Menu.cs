using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (
			Input.IsActionJustPressed("p5_interact") ||
			Input.IsActionJustPressed("p4_interact") ||
			Input.IsActionJustPressed("p3_interact") ||
			Input.IsActionJustPressed("p2_interact") ||
			Input.IsActionJustPressed("p1_interact")
		)
		{
			var gameManager = GetNode<GameManager>("/root/GameManager");

			gameManager.LoadScene("res://main.tscn");
		}
	}
}
