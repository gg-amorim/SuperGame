using MMO.Core;
using Godot;


namespace MMO.Scripts.Players.States;

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
        if (Player.Velocity.LengthSquared() > 0.01f)
        {

            float targetAngle = Mathf.Atan2(Player.Velocity.X, Player.Velocity.Z);
            float smoothRotation = Mathf.LerpAngle(Player.Visuals.Rotation.Y, targetAngle, 10f * delta);

            Vector3 newRotation = new(
                Player.Visuals.Rotation.X,
                smoothRotation,
                Player.Visuals.Rotation.Z
            );

            Player.Visuals.Rotation = newRotation;
            Player.Model.Rotation = newRotation;
        }

        Player.MoveAndSlide();
    }

    private Vector3 VelocityByInput(InputPackage input, float delta)
    {
        Vector3 newVelocity = Player.Velocity;

        // CORREÇÃO: Usamos GlobalBasis para garantir a orientação correta no mundo
        Vector3 direction = Player.CameraMount.GlobalBasis * new Vector3(input.InputDirection.X, 0, input.InputDirection.Y);

        // CRÍTICO: Zeramos o eixo Y para que olhar para cima/baixo não altere a velocidade horizontal
        direction.Y = 0;
        direction = direction.Normalized();

        newVelocity.X = direction.X * SPEED;
        newVelocity.Z = direction.Z * SPEED;

        if (!Player.IsOnFloor())
        {
            newVelocity.Y -= _gravity * delta;
        }

        return newVelocity;
    }
}
