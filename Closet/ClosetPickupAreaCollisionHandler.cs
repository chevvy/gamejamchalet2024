using Godot;

public partial class ClosetPickupAreaCollisionHandler : Area2D
{
	public ClosetItemType ClosetItemType;

	public void OnBodyEntered(Node2D body)
	{
		if (body is Character character) OnEnterClosetPickupArea(character);
	}

	public void OnBodyExited(Node2D body)
	{
		if (body is Character character) OnExitClosetPickupArea(character);
	}

	private void OnEnterClosetPickupArea(Character character)
	{
		character.CanReceiveItem = true;
		character._lastBumpedItemType = ClosetItemType;
	}

	private void OnExitClosetPickupArea(Character character)
	{
		character.CanReceiveItem = false;
	}

	private void OnDestroy()
	{
		QueueFree();
	}
}
