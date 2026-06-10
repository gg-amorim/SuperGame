using MMO.Core;
using System.Collections.Generic;
namespace MMO.Scripts.Weapons.Models;

public partial class Sword : Weapon
{
    public override void _Ready()
    {
        BaseDamage = 10;
        BasicAttacks = new Dictionary<string, string>()
        {
            {"light_attack_pressed", "slash_1" }
        };
    }

    public override HitData GetHitData() => Holder.CurrentState.FormHitData(this);

}
