using Godot;
using MMO.Core;

namespace MMO.Scripts.Components;

public partial class InputComponent : Node
{
    public InputPackage GatherInput()
    {
        var newInput = new InputPackage();

        newInput.InputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

        if (newInput.InputDirection != Vector2.Zero)
        {
            newInput.Actions.Add("run");

            if (Input.IsActionPressed("sprint"))
                newInput.Actions.Add("sprint");
        }

        if (Input.IsActionJustPressed("jump"))
        {
            if (newInput.Actions.Contains("sprint"))
                newInput.Actions.Add("jump_sprint");
            else
                newInput.Actions.Add("jump_run");
        }

        if (newInput.Actions.Count == 0)
        {
            newInput.Actions.Add("idle");
        }

        return newInput;
    }
}