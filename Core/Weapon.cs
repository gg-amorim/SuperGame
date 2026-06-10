using Godot;
using MMO.Scripts.Players;
using System.Collections.Generic;
using System.ComponentModel;

namespace MMO.Core;

[GlobalClass]
public partial class Weapon : Area3D
{
    // Usando List<Area3D> para facilitar o uso do .Add() e .Contains() que fizemos no Hurtbox
    public List<Area3D> HitboxIgnoreList { get; set; } = new List<Area3D>();

    public bool IsAttacking { get; set; } = false;

    [Export]
    public PlayerModel Holder { get; set; }

    [Export]
    public float BaseDamage { get; set; } = 10f;

    // Como visto no arquivo Combat.cs, este dicionário mapeia o input (string) para a animação (string)
    public Dictionary<string, string> BasicAttacks { get; set; } = [];

    // Marcado como 'virtual' para que armas específicas (ex: Sword, Bow) possam dar 'override'
    public virtual HitData GetHitData()
    {
        GD.Print("someone tries to get hit by default Weapon");
        return HitData.Blank();
    }

}
