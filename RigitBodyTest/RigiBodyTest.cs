using Godot;
using System;

public partial class RigiBodyTest : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _ApplyPlaneMovement(Vector2 direction){

		GD.Print("Inside Hit");
		_ApplyPlaneMovement(direction);
		


	}





}
