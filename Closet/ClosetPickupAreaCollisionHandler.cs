using Godot;

public partial class ClosetPickupAreaCollisionHandler : Area2D
{
	public ClosetItemType ClosetItemType;

	public void OnBodyEntered(Node2D body)
	{
		GD.Print(string.Format("{0}.OnBodyEntered: {1}", this.Name, body.Name));

		if (body is Character character) OnEnterClosetPickupArea(character);
	}

	private void OnEnterClosetPickupArea(Character character)
	{
		character.ReceiveItem(ClosetItemType);
	}

	private void OnDestroy()
	{
		QueueFree();
	}
}
