using Godot;
using System;

public partial class DecrementingBar : Control
{
	[Signal]
	public delegate void BarEmptyEventHandler();

	[Signal]
	public delegate void BarValueChangedEventHandler();

	[Export]
	public ClosetItemType itemType;

	[Export]
	private int DOWN_PER_SECOND = 3;

	[Export]
	private Color color;

	private ProgressBar bar;
	private Timer timer;

	public override void _Ready()
	{
		bar = GetNode<ProgressBar>("ProgressBar");
		timer = GetNode<Timer>("TickTimer");

		if (GameManager.Instance != null)
		{
			GameManager.Instance.GameReady += Initialize;
		}

		SetupBarColor();
	}

	private void Initialize()
	{
		timer.Start();
	}

	private void OnTreeExiting()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.GameReady -= Initialize;
		}
	}

	public bool IsEmpty()
	{
		return bar.Value == 0;
	}

	public void Increase(int value)
	{
		bar.Value += value;
	}

	private void OnTickTimerTimeout()
	{
		LowerValue(DOWN_PER_SECOND);
	}

	private void LowerValue(int value)
	{
		bar.Value -= value;
		NotifyValueChanges();
	}

	private void NotifyValueChanges()
	{
		EmitSignal(SignalName.BarValueChanged);
		if (bar.Value == 0)
		{
			EmitSignal(SignalName.BarEmpty);
		}
	}

	private void SetupBarColor()
	{
		Modulate = color;
	}
}
