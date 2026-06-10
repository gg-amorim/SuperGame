using Godot;
using System;

namespace MMO.Core;

public partial class MovesData : Node
{
	private MoveDatabase _moveDatabase;

    public override void _Ready()
	{
		_moveDatabase = GetNode<MoveDatabase>("MoveDatabase");
	}

    public bool GetVulnerable(string animation, float timecode)
    {
        var data = _moveDatabase.GetAnimation(animation);
        var track = data.FindTrack("MoveDatabase:is_vulnerable", Animation.TrackType.Value);
        return _moveDatabase.GetBooleanValue(animation, track, timecode);
    }

    public bool GetInterruptable(string animation, float timecode)
    {
        var data = _moveDatabase.GetAnimation(animation);
        var track = data.FindTrack("MoveDatabase:is_interruptable", Animation.TrackType.Value);
        return _moveDatabase.GetBooleanValue(animation, track, timecode);
    }

    public bool GetParryable(string animation, float timecode)
    {
        var data = _moveDatabase.GetAnimation(animation);
        var track = data.FindTrack("MoveDatabase:is_parryable", Animation.TrackType.Value);
        return _moveDatabase.GetBooleanValue(animation, track, timecode);
    }
}
