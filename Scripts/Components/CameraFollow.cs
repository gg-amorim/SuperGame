using Godot;
using System;

namespace MMO.Scripts.Components;

public partial class CameraFollow : Camera3D
{
    // [Export] expõe a variável no Inspector do Godot
    [Export]
    public Node3D SpringArm { get; set; }

    [Export]
    public float LerpPosition { get; set; } = 1.0f;

    public override void _Process(double delta)
    {
        // Boa prática: sempre verificar se o nó referenciado não é nulo 
        // antes de tentar acessar suas propriedades no C#
        if (SpringArm != null)
        {
            Position = Position.Lerp(SpringArm.Position, (float)delta * LerpPosition);
        }
    }
}
