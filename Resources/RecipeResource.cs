using Godot;
using System;

public partial class RecipeResource : Resource
{

    [Export]
    public string result;
    [Export]
    public Godot.Collections.Array<string> ingredients;

}
