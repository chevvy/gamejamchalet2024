using Godot;
using System;

public partial class Closet : RigidBody2D
{
	[Export] public ClosetItemType ClosetItemType;
	[Export] public Texture2D ClosetTexture;

	[Export] public bool DisableMovement = false;

	private ClosetPickupAreaCollisionHandler _closetPickupAreaCollisionHandler;
	private Sprite2D _closetSprite;

	private Vector2 _initialPosition;

	public override void _Ready()
	{
		GD.Print("_Ready on ClosetGameReadyStateHandler");

		GameManager.Instance.GameReady += OnGameReady;

		SetClosetItemType(ClosetItemType);
		SetClosetTexture(ClosetTexture);

		Freeze = true;

		_initialPosition = Position;
	}

	public override void _Process(double delta)
	{
		if (4500 < Math.Abs(Position.Y) || 4500 < Math.Abs(Position.X))
		{
			OnAdjustNeeded();
		}
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

	public void ApplyPlaneMovement(Vector2 direction)
	{
		if (DisableMovement)
		{
			return;
		}
		ApplyImpulse(direction * 4);
	}

	private void OnAdjustNeeded()
	{
		Freeze = true;
		CallDeferred("FinishAdjustingTeleport");
	}

	public void FinishAdjustingTeleport()
	{
		Position = _initialPosition;
		Freeze = false;
	}
}
