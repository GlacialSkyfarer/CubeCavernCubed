using Godot;
using System;

public partial class PlayerLight : OmniLight3D
{
	
	[Export]
	NodePath playerPath;
	[Export] Vector3 offset;

	Node3D player;

    public override void _Ready()
    {
        player = GetNode<Node3D>(playerPath);
    }

    public override void _Process(double delta)
    {
        GlobalPosition = player.GlobalPosition + offset;
    }

}
