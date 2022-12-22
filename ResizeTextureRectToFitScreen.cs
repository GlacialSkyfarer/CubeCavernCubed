using Godot;
using System;

public partial class ResizeTextureRectToFitScreen : TextureRect
{

	public override void _Process(double delta)
	{

		Size = GetViewportRect().Size;

	}
}
