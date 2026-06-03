using MMO.Core;
using Godot;


namespace MMO.Scenes.Player.States;

public partial class Sprint : State
{
    private const float SPEED = 5.0f;
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");


    public override void _Ready()
    {
        Animation = "sprint";
        StateName = "sprint";
    }

    public override string CheckRelevance(InputPackage input)
    {
        if (!Player.IsOnFloor())
            return "midair";

        input.Actions.Sort(MovesPrioritySort);
        if (input.Actions[0] == "sprint")
        {
            return "okay";
        }
        return input.Actions[0];
    }

    public override void Update(InputPackage input, float delta)
    {
        Player.Velocity = VelocityByInput(input, delta);
        Player.MoveAndSlide();
    }

    private Vector3 VelocityByInput(InputPackage input, float delta)
    {
        Vector3 newVelocity = Player.Velocity;

        Vector3 direction = (Player.Transform.Basis * new Vector3(-input.InputDirection.X, 0, -input.InputDirection.Y)).Normalized();
        newVelocity.X = direction.X * SPEED;
        newVelocity.Z = direction.Z * SPEED;

        if (!Player.IsOnFloor())
        {
            newVelocity.Y -= _gravity * delta;
        }

        return newVelocity;
    }
}
