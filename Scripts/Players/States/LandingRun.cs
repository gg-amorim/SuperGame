using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States;

// Step 1: redefine your ClassName
public partial class LandingRun : State
{
    private const float TRANSITION_TIMING = 0.2f;
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(TRANSITION_TIMING)) return BestInputThatCanBePaid(input);
       
        return "okay";
    }

    public override void Update(InputPackage input, float delta)
    {
        Vector3 velocity = Player.Velocity;
        velocity.Y -= _gravity * delta;
        Player.Velocity = velocity;
        Player.MoveAndSlide();
    }

    
}
