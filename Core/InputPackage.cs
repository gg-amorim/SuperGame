using Godot;
using System.Collections.Generic;

namespace MMO.Core;

public class InputPackage
{
    public List<string> Actions { get; set; } = [];
    public List<string> CombatActions { get; set; } = [];

    public Vector2 InputDirection { get; set; }
}

