using Godot;
using MMO.Core;

namespace MMO.Scenes.Player.States;


public partial class LandingSprint : State
{
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    private const float TRANSITION_TIMING = 0.2f;
    public override void _Ready()
    {
        Animation = "landing_sprint";
        StateName = "landing_sprint";
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

   
    public override void Update(InputPackage input, float delta)
    {
        Vector3 velocity = Player.Velocity;
        velocity.Y -= _gravity * delta;
        Player.Velocity = velocity;
        Player.MoveAndSlide();
    }

}
