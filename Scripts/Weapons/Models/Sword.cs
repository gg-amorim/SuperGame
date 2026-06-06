using MMO.Core;
using System.Collections.Generic;
namespace MMO.Scripts.Weapons.Models;

public partial class Sword : Weapon
{
    public override void _Ready()
    {
        BasicsAttacks = new Dictionary<string, string>()
        {
            {"light_attack_pressed", "slash_1" }
        };
    }
}
