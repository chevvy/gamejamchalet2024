using Godot;
using System;

public partial class LifeBar : Control
{
	private ProgressBar bar;
	public override void _Ready()
	{
		bar = GetNode<ProgressBar>("ProgressBar");
	}

	public override void _Process(double delta)
	{
	}

	public void Increase(int value)
	{
		bar.Value += value;
	}
}
