using Godot;
using System;


namespace MMO.Core;

public partial class MoveDatabase : AnimationPlayer
{
    [Export]
    public bool IsParryable { get; set; }

    [Export]
    public bool IsVulnerable { get; set; }

    [Export]
    public bool IsInterruptable { get; set; }

    [Export]
    public bool IsGrabable { get; set; }

    // No GDScript o tipo era inferido como int (0), mas como posições 
    // costumam ser decimais no Godot, tipamos como float (0.0f). 
    // Mude para int se for estritamente um número inteiro.
    [Export]
    public float RootPositionZ { get; set; } = 0.0f;

    public bool GetBooleanValue(string animation, int track, float timecode)
    {
        Animation data = GetAnimation(animation);

        // Em C#, o método ValueTrackInterpolate retorna um tipo 'Variant'.
        // É necessário fazer o cast (conversão explícita) para bool.
        return (bool)data.ValueTrackInterpolate(track, timecode);
    }
}
