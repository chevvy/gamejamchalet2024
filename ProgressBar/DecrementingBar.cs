using Godot;
using System;

public partial class DecrementingBar : Control
{
	[Signal]
	public delegate void BarEmptyEventHandler();

	[Signal]
	public delegate void BarValueChangedEventHandler();

	private ProgressBar bar;
	private Timer timer;

	public override void _Ready()
	{
		bar = GetNode<ProgressBar>("ProgressBar");
		timer = GetNode<Timer>("TickTimer");

		timer.Start();
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
		LowerValue(2);
	}

	private void LowerValue(int value)
	{
		bar.Value -= value;
		NotifyValueChanges();
	}

	private void NotifyValueChanges()
	{
		EmitSignal("BarValueChanged");
		if (bar.Value == 0)
		{
			EmitSignal("BarEmpty");
		}
	}

}
