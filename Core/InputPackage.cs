using Godot;
using System.Collections.Generic;

namespace MMO.Core;

public partial class InputPackage : Node
{
    public List<string> Actions { get; set; } = [];

    public Vector2 InputDirection { get; set; }
}

