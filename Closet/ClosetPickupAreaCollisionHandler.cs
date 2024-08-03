using Godot;

public partial class ClosetPickupAreaCollisionHandler : Area2D
{
	[Export] public ClosetItemType ClosetItemType;

	public void OnBodyEntered(Node2D body)
	{
		if (body is Character character) OnEnterClosetPickupArea(character);
	}

	private void OnEnterClosetPickupArea(Character character)
	{
	}

	private void OnDestroy()
	{
		QueueFree();
	}
}
