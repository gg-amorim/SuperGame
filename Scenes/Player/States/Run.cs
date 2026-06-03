using Godot;
using MMO.Core;

namespace MMO.Scenes.Player.States;

public partial class Run : State
{
    private const float Speed = 3.0f;
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");


    public override void _Ready()
    {
        Animation = "run";
        StateName = "run";
    }
    public override string CheckRelevance(InputPackage input)
    {
        if (!Player.IsOnFloor()) return "midair";

        input.Actions.Sort(MovesPrioritySort);
        if (input.Actions[0] == "run")
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
        newVelocity.X = direction.X * Speed;
        newVelocity.Z = direction.Z * Speed;

        if (!Player.IsOnFloor())
        {
            newVelocity.Y -= _gravity * delta;
        }

        return newVelocity;
    }
}
