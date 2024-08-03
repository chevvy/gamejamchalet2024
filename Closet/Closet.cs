using Godot;
using System;

public partial class Closet : RigidBody2D
{
	[Export] public ClosetItemType ClosetItemType;
	[Export] public Texture2D ClosetTexture;

	private ClosetPickupAreaCollisionHandler _closetPickupAreaCollisionHandler;
	private Sprite2D _closetSprite;

	public override void _Ready()
	{
		GD.Print("_Ready on ClosetGameReadyStateHandler");

		GameManager.Instance.GameReady += OnGameReady;

		SetClosetItemType(ClosetItemType);
		SetClosetTexture(ClosetTexture);

		Freeze = true;
	}

	private void SetClosetItemType(ClosetItemType itemType)
	{
		_closetPickupAreaCollisionHandler = GetNode<CollisionObject2D>("ClosetPickupArea") as ClosetPickupAreaCollisionHandler;
		_closetPickupAreaCollisionHandler.ClosetItemType = itemType;
	}

	private void SetClosetTexture(Texture2D texture)
	{
		_closetSprite = GetNode<Sprite2D>("ClosetSprite");
		_closetSprite.Texture = texture;
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
		ApplyImpulse(direction*4);
	}
}
