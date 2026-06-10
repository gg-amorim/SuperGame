using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States;

// Step 1: redefine your ClassName
public partial class Midair : State
{
    private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    private RayCast3D _downcast;
    private BoneAttachment3D _rootAttachment;

    private float _landingHeight = 1.163f;

    public override void _Ready()
    {
        _downcast = GetNode<RayCast3D>("../../Downcast");
        _rootAttachment = GetNode<BoneAttachment3D>("../../Root");
    }

    public override string DefaultLifecycle(InputPackage input)
    {
        Vector3 floorPoint = _downcast.GetCollisionPoint();
        if (_rootAttachment.GlobalPosition.DistanceTo(floorPoint) < _landingHeight)
        {
            Vector3 xzVelocity = Player.Velocity;
            xzVelocity.Y = 0;

            if (xzVelocity.LengthSquared() >= 10.0f)
            {
                return "landing_sprint";
            }
            return "landing_run";
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
