using Godot;
using System;

public partial class ItemSpawner : AnimatableBody2D
{
	[Export] public Area2D SpawnArea;

	public override void _Ready()
	{
		TrySetSpawnArea();
		FailIfSpawnAreaNotSet();
		Position = GetRandomSpawnPosition();
	}

	public override void _Process(double delta)
	{
	}

	private void TrySetSpawnArea()
	{
		SpawnArea = GetTree().CurrentScene.GetNode<Area2D>("ItemSpawnArea");
	}

	private void FailIfSpawnAreaNotSet()
	{
		if (SpawnArea == null)
		{
			GD.PrintErr("ItemSpawner: SpawnArea not set");
			GetTree().Quit();
		}
	}

	private Vector2 GetRandomSpawnPosition()
	{
		var area = SpawnArea.GetNode<MeshInstance2D>("Area").Mesh.GetAabb();

		var width = area.Size.X / 2;
		var height = area.Size.Y / 2;
		var origin = area.GetCenter();

		var min_x = origin.X - width;
		var min_y = origin.Y - height;
		var max_x = origin.X + width;
		var max_y = origin.Y + height;

		var random_x = new Random().Next((int)min_x, (int)max_x);
		var random_y = new Random().Next((int)min_y, (int)max_y);

		GD.Print("Spawn position: " + random_x + ", " + random_y);

		return new Vector2(random_x, random_y);
	}
}
