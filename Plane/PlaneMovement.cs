using Godot;
using System;

public partial class PlaneMovement : Node
{
	// Called when the node enters the scene tree for the first time.
	double LastTimePlaneMoved = 20;
	double IntervalOfPlaneMovement = 3;

	Vector2 LastDirection = Vector2.Zero;

	RandomNumberGenerator random = new RandomNumberGenerator();

	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float ftest =  -1;
		if (LastTimePlaneMoved > IntervalOfPlaneMovement){
		Vector2 direction = new Vector2(random.RandfRange(ftest,1),random.RandfRange(ftest,1));
		GeneratePlaneMovement(direction - LastDirection*2);
		LastDirection = direction;
		LastTimePlaneMoved = 0;
		}
		else{
			LastTimePlaneMoved += delta;
		}
	}

	public void GeneratePlaneMovement(Vector2 direction){
		GetTree().CallGroup("PlaneMovement","ApplyPlaneMovement",direction);
	}


}
