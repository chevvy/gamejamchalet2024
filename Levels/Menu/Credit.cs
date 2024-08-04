using Godot;
using System;

public partial class Credit : Control
{
	private Timer _timer;

	public override void _Ready()
	{
		GD.Print("Thank you for playing!");

		_timer = new Timer();
		_timer.OneShot = true;
		_timer.Timeout += LoadMainScene;

		AddChild(_timer);

		_timer.Start(5);
	}

	private void LoadMainScene()
	{
		GD.Print("Loading main scene");

		var gameManager = GetNode<GameManager>("/root/GameManager");

		gameManager.LoadScene("res://main.tscn");
	}
}
