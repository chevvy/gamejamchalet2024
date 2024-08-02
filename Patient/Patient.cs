using Godot;
using System;

public partial class Patient : Node2D
{

	private DecrementingBar decrementingBar;
	private LifeBar lifeBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		decrementingBar = GetNode<DecrementingBar>("Bars/DecrementingBar");
		lifeBar = GetNode<LifeBar>("Bars/LifeBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void OnHealthTickTimeout()
	{
		if (!decrementingBar.IsEmpty())
		{
			GainHealth();
		}
	}

	private void GainHealth()
	{
		lifeBar.Increase(5);
	}
}
