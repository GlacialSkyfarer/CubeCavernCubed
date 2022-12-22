using Godot;
using System;

public partial class TorchFlicker : OmniLight3D
{

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		RandomNumberGenerator randy = new RandomNumberGenerator();

		LightEnergy = randy.RandfRange(1.75f, 2f);

	}
}
