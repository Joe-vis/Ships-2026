using Godot;
using System;

public partial class ShipWave : MeshInstance3D
{
	// Common
	private float _time = 0;
	// Floating
	[Export] private float _floatFrequency = 0.1f;
	[Export] private float _floatAmplitude = 0.1f;
	// Roll
	[Export] private float _rollFrequency = 0.1f;
	[Export] private float _pitchFrequency = 0.1f;
	[Export] private float _rollAmplitude = 0.1f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_time += (float) delta;
		// Transform3D ship = GlobalTransform;
		// ship.Basis = ship.Basis.Rotated(Vector3.Forward, Mathf.Sin(_time * _rollFrequency) * _rollAmplitude);
		// ship.Basis = ship.Basis.Rotated(Vector3.Left, Mathf.Sin(_time * _rollFrequency) * _rollAmplitude);
		Position = Vector3.Up * Mathf.Sin(_time * _floatFrequency) * _floatAmplitude;
		Vector3 rotation = Rotation;
		rotation.X = Mathf.Sin(_time * _pitchFrequency) * _rollAmplitude;
		rotation.Z = Mathf.Sin(_time * _rollFrequency) * _rollAmplitude;
		Rotation = rotation;

		// GlobalTransform = ship;
	}
}
