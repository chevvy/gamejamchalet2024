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

	private ItemType itemType;

	public override void _Ready()
	{
		bar = GetNode<ProgressBar>("ProgressBar");
		timer = GetNode<Timer>("TickTimer");

		timer.Start();

		SetupItemType();
		SetupBarColor();
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

	private void SetupItemType()
	{
		if (itemName == "A")
		{
			itemType = ItemType.BANDAGE;
		}
		else
		{
			// Default, shouldn't get here ...
			GD.PrintErr("Invalid item type export on progress bar");
			itemType = ItemType.BANDAGE;
		}
	}

	private void SetupBarColor()
	{
		Modulate = color;
	}
}
