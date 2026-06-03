using Godot;
using MMO.Core;

namespace MMO.Scenes.Player.States;

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
        Animation = "midair";
        StateName = "midair";
    }

    // Step 3: navigate to PlayerModel and add a new state to moves dictionary
    // Step 4: navigate to base State and add this new state to priority dictionary

    // Step 5: implement a check_relevance function to manage transitions, return action name or "okay"
    public override string CheckRelevance(InputPackage input)
    {
        Vector3 floorPoint = _downcast.GetCollisionPoint();
        if (_rootAttachment.GlobalPosition.DistanceTo(floorPoint) < _landingHeight)
        {
            // Criamos a cópia para zerar o Y e calcular o comprimento XZ
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

    // Step 6: implement an update function
    public override void Update(InputPackage input, float delta)
    {
        Vector3 velocity = Player.Velocity;
        velocity.Y -= _gravity * delta;
        Player.Velocity = velocity;

        Player.MoveAndSlide();
    }
}
