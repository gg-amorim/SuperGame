using Godot;
using MMO.Core;
using System;

namespace MMO.Scripts.Players;

public partial class Hitbox : Area3D
{
    private PlayerModel _model;

    public override void _Ready()
    {
        // Pega o nó "avô" e faz o cast para PlayerModel
        _model = GetNode<PlayerModel>("../..");

        // No C#, a inscrição em sinais (Signals) é feita com += 
        AreaEntered += OnContact;
    }

    private void OnContact(Area3D area)
    {
        if (IsEligibleAttackingWeapon(area))
        {
            // Como já validamos que 'area' é uma 'Weapon' no método abaixo,
            // podemos fazer o cast com segurança.
            Weapon weapon = (Weapon)area;

            // Supondo que HitboxIgnoreList seja um List<Node> ou List<Hurtbox> na sua classe Weapon
            weapon.HitboxIgnoreList.Add(this);

            // current_move virou CurrentState na nossa conversão do PlayerModel
            _model.CurrentState.ReactOnHit(weapon.GetHitData());
        }
    }

    private bool IsEligibleAttackingWeapon(Area3D area)
    {
        // O C# permite Pattern Matching (area is Weapon weapon), 
        // o que já checa o tipo e cria a variável convertida na mesma linha.
        if (area is Weapon weapon &&
            weapon != _model.ActiveWeapon &&
            !weapon.HitboxIgnoreList.Contains(this) &&
            weapon.IsAttacking)
        {
            return true;
        }

        return false;
    }
}
