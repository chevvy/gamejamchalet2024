using Godot;
using System;

public partial class HealingItem : Area2D
{
	public void OnBodyEntered(Node2D body)
	{
		if (body is Character character) OnPickUpByCharacter(character);
	}

	private void OnPickUpByCharacter(Character character)
	{
		character.ReceiveItem(ItemType.BANDAGE);
		OnDestroy();
	}

	private void OnDestroy()
	{
		QueueFree();
	}
}
