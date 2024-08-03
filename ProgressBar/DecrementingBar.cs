using Godot;
using System;

public partial class DecrementingBar : Control
{
	[Signal]
	public delegate void BarEmptyEventHandler();

	[Signal]
	public delegate void BarValueChangedEventHandler();

	[Export]
	public string itemName;

	[Export]
	private Color color;

	private ProgressBar bar;
	private Timer timer;

	private ClosetItemType itemType;

	public override void _Ready()
	{
		bar = GetNode<ProgressBar>("ProgressBar");
		timer = GetNode<Timer>("TickTimer");

		if (GameManager.Instance != null)
		{
			GameManager.Instance.GameReady += Initialize;
		}

		SetupItemType();
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
		LowerValue(5);
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

	private void SetupItemType()
	{
		if (itemName == "A")
		{
			itemType = ClosetItemType.BANDAGE;
		}
		else if (itemName == "B")
		{
			itemType = ClosetItemType.PILLZ;
		}
		else if (itemName == "C")
		{
			itemType = ClosetItemType.SERINGE;
		}
		else
		{
			// Default, shouldn't get here ...
			GD.PrintErr("Invalid item type export on progress bar");
			itemType = ClosetItemType.BANDAGE;
		}
	}

	private void SetupBarColor()
	{
		Modulate = color;
	}
}
