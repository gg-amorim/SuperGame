using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States;

// Step 1: redefine your ClassName
public partial class LandingRun : State
{
    private const float TRANSITION_TIMING = 0.2f;
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    public override void _Ready()
    {
        Animation = "landing_run";
        StateName = "landing_run";
    }

    public override string CheckRelevance(InputPackage input)
    {
        if (WorksLongerThan(TRANSITION_TIMING))
        {
            input.Actions.Sort(MovesPrioritySort);
            return input.Actions[0];
        }

        return "okay";
    }

    // Step 6: implement an update function
    public override void Update(InputPackage input, float delta)
    {
        //Vector3 velocity = Player.Velocity;

        //// Mantém a gravidade
        //velocity.Y -= _gravity * delta;

        //// FREIO DE ATERRISSAGEM:
        //// Se o jogador não estiver apertando nada (InputDirection == Zero), 
        //// ou se você quiser que ele freie independentemente do input durante a animação de pouso:
        //if (input.InputDirection == Vector2.Zero)
        //{
        //    // O valor '10.0f' é a força do freio. Quanto maior, mais rápido ele para.
        //    velocity.X = Mathf.Lerp(velocity.X, 0, 10.0f * delta);
        //    velocity.Z = Mathf.Lerp(velocity.Z, 0, 10.0f * delta);
        //}
        //else
        //{
        //    // Opcional: Se quiser que ele já possa guiar o personagem suavemente ao pousar
        //    Vector3 direction = (Player.Transform.Basis * new Vector3(-input.InputDirection.X, 0, -input.InputDirection.Y)).Normalized();
        //    velocity.X = Mathf.Lerp(velocity.X, direction.X * 3.0f, 10.0f * delta); // 3.0f é o Speed do Run
        //    velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * 3.0f, 10.0f * delta);
        //}

        //Player.Velocity = velocity;
        //Player.MoveAndSlide();

        Vector3 velocity = Player.Velocity;
        velocity.Y -= _gravity * delta;
        Player.Velocity = velocity;
        Player.MoveAndSlide();
    }

    
}
