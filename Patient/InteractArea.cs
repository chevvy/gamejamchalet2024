using Godot;
using System;

public partial class InteractArea : Area2D
{

	// Used to determine if the parent is a patient and if we should give items
	[Export] public bool IsPatient = false;
	public Patient Patient = null;
	public void OnBodyEntered(Node2D body)
	{
		if (body is Patient patient)
		{
			Patient = patient;
		}

		if (body is Character character && !IsPatient && body != this)
		{
			character.ReceiveItem(ClosetItemType.BANDAGE);
		}
	}

	public void OnBodyExited(Node2D body)
	{
		if (body is Patient)
		{
			Patient = null;
		}
	}
}
