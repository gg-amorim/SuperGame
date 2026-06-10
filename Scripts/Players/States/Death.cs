using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States; // Ajuste para a sua estrutura de pastas

[GlobalClass]
public partial class Death : State 
{

    private const float AnimationEnd = 3f;

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(AnimationEnd))
        {
            // Diferente dos outros, este força a volta para o "idle" diretamente
            return "idle";
        }

        return "okay";
    }

    public override void Update(InputPackage input, float delta)
    {
        // Método vazio, deixado de propósito como no GDScript ("pass")
    }

    public override void OnEnterState()
    {
        // Método vazio, deixado de propósito como no GDScript ("pass")
    }

    public override void OnExitState()
    {
        // O valor gigante que você usou no GDScript!
        // Assumindo que a função GainHealth no seu HumanoidResources aceita um float ou double.
        Resources.GainHealth(987651468f);
    }
}