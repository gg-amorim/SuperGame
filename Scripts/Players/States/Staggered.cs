using Godot;
using MMO.Core;
using System;

namespace MMO.Scripts.Players.States;

[GlobalClass]
public partial class Staggered : State
{
    // Constantes em C# precisam do sufixo 'f' para indicar que são do tipo float
    private const float AnimationEnd = 0.9833f;

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(AnimationEnd))
        {
            return BestInputThatCanBePaid(input);
        }

        return "okay";
    }

    /*
    public override void Update(InputPackage input, float delta)
    {
        // fazer coisas da gravidade
        // Eu sei que ele só fica parado no ar, mas ser apenas "staggered-e-caindo" é chato.
        // Idealmente eu queria que o stagger no ar tivesse uma animação de queda descontrolada
        // caindo em um buraco e ficando deitado no chão, mas ainda não fiz isso.
    }
    */
}
