using Godot;
using System;

public partial class Closet : RigidBody2D
{
	[Export] public ClosetItemType ClosetItemType;

	private ClosetPickupAreaCollisionHandler _closetPickupAreaCollisionHandler;

	public override void _Ready()
	{
		GD.Print("_Ready on ClosetGameReadyStateHandler");

		GameManager.Instance.GameReady += OnGameReady;

		SetClosetItemType(ClosetItemType);

		Freeze = true;
	}

	private void SetClosetItemType(ClosetItemType itemType)
	{
		_closetPickupAreaCollisionHandler = GetNode<CollisionObject2D>("ClosetPickupArea") as ClosetPickupAreaCollisionHandler;
		_closetPickupAreaCollisionHandler.ClosetItemType = itemType;
	}

	public void OnGameReady()
	{
		GD.Print("Game ready on ClosetGameReadyStateHandler");

		FreezeMode = FreezeModeEnum.Static;
		Freeze = false;
	}

	private void OnTreeExiting()
	{
		GameManager.Instance.GameReady -= OnGameReady;
	}

	public void ApplyPlaneMovement(Vector2 direction){
		GD.Print(direction);
		ApplyImpulse(direction);
	}
}
