using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States; // Ajuste o namespace para sua estrutura

[GlobalClass]
public partial class Riposte : State
{
    private const float AnimationEnd = 4.8f;

    public float HitDamage { get; set; } = 100f;

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(AnimationEnd))
        {
            return BestInputThatCanBePaid(input);
        }

        return "okay";
    }

    public override void Update(InputPackage input, float delta)
    {
        // No futuro, isso será transformado em um parâmetro de animação no backend,
        // mas por enquanto controlamos via tempo manual!
        if (WorksBetween(2.2f, 3.6f))
        {
            Player.Model.ActiveWeapon.IsAttacking = true;
        }
        else
        {
            Player.Model.ActiveWeapon.IsAttacking = false;
        }
    }

    public override HitData FormHitData(Weapon weapon)
    {
        HitData hit = HitData.Blank(); // Utilizando a função estática que criamos

        hit.Damage = HitDamage;
        hit.HitMoveAnimation = AnimationStr; // 'Animation' herdada da classe State
        hit.IsParryable = IsParryable();  // 'IsParryable()' herdada da classe State
        hit.Weapon = Player.Model.ActiveWeapon;

        return hit;
    }

    public override void OnEnterState()
    {
        // Método vazio deixado de propósito (pass)
    }

    public override void OnExitState()
    {
        // Limpa a lista de ignorados para que o próximo golpe possa acertar os mesmos alvos
        Player.Model.ActiveWeapon.HitboxIgnoreList.Clear();

        // Garante que a arma será desligada caso a animação seja interrompida (ex: levou stagger)
        Player.Model.ActiveWeapon.IsAttacking = false;
    }
}