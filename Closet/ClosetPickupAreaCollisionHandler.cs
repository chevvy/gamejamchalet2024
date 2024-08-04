using Godot;

public partial class ClosetPickupAreaCollisionHandler : Area2D
{
	public ClosetItemType ClosetItemType;

	public void OnBodyEntered(Node2D body)
	{
		if (body is Character character) OnEnterClosetPickupArea(character);
	}

	private void OnEnterClosetPickupArea(Character character)
	{
		character.CanReceiveItem = true;
		character.ItemType = ClosetItemType;
	}

	private void OnExitClosetPickupArea(Character character)
	{
		character.ItemType = null;
	}

	private void OnDestroy()
	{
		QueueFree();
	}
}
