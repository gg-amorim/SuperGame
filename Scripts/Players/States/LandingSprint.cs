using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States;


public partial class LandingSprint : State
{
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    private const float TRANSITION_TIMING = 0.2f;
    
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
