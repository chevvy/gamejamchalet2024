using Godot;
using System;

public partial class LifeBar : Control
{
	[Export]
	public const int LOSE_LIFE_PER_TICK = 1;

	private ProgressBar bar;

	private Timer lifeTimer;

	public override void _Ready()
	{
		bar = GetNode<ProgressBar>("ProgressBar");
		lifeTimer = GetNode<Timer>("PassifLifeLossTimer");

		GameManager.Instance.GameReady += Initialize;
	}

	private void Initialize()
	{
		lifeTimer.Start();
	}

	private void OnTreeExiting()
	{
		GameManager.Instance.GameReady -= Initialize;
	}

	public void Increase(int value)
	{
		bar.Value += value;
	}

	public void LoseLifeFromTimer()
	{
		bar.Value -= LOSE_LIFE_PER_TICK;
	}
}
