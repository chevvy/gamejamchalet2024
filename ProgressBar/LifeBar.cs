using Godot;
using System;

public partial class LifeBar : Control
{

	[Signal]
	public delegate void OnLifeZeroEventHandler();

	[Export]
	public const int LOSE_LIFE_PER_TICK = 2;

	private ProgressBar bar;

	private Timer lifeTimer;

	public override void _Ready()
	{
		bar = GetNode<ProgressBar>("ProgressBar");
		lifeTimer = GetNode<Timer>("PassifLifeLossTimer");

		if (GameManager.Instance != null)
		{
			GameManager.Instance.GameReady += Initialize;
		}
	}

	private void Initialize()
	{
		lifeTimer.Start();
	}

	private void OnTreeExiting()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.GameReady -= Initialize;
		}
	}

	public void Increase(int value)
	{
		bar.Value += value;
	}

	public void LoseLifeFromTimer()
	{
		Decrease(LOSE_LIFE_PER_TICK);
	}

	private void Decrease(int value)
	{
		bar.Value -= value;
		if (bar.Value <= 0)
		{
			EmitSignal(SignalName.OnLifeZero);
		}
	}
}
