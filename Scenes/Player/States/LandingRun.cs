using Godot;
using MMO.Core;

namespace MMO.Scenes.Player.States;

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
        Vector3 velocity = Player.Velocity;
        velocity.Y -= _gravity * delta;
        Player.Velocity = velocity;
        Player.MoveAndSlide();
    }

    
}
