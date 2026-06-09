using Godot;
using System;

public partial class SkeletonAnimator : AnimationPlayer
{
    public override void _Ready()
    {
        ConfigureBlendingTimes();
    }

    public void ConfigureBlendingTimes()
    {
        // No C#, o método usa PascalCase: SetBlendTime em vez de set_blend_time
        SetBlendTime("run", "jump_run", 0.5);
        SetBlendTime("landing_run", "run", 0.5);
        SetBlendTime("jump_sprint", "midair", 0.5);
        SetBlendTime("landing_run", "sprint", 0.3);
        SetBlendTime("landing_sprint", "run", 0.3);
        SetBlendTime("idle", "slash_1", 0.5);
        SetBlendTime("slash_2", "slash_1", 0.3);
    }
}
