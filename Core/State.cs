using Godot;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace MMO.Core;

public partial class State : Node
{
    public CharacterBody3D Player { get; set; }
    public string Animation { get; set; }
    public string StateName { get; set; }
    public string QueuedMove { get; set; } = "none, drop error please";
    public bool HasQueuedMove { get; set; } = false;

    private ulong _enterStateTime { get; set; }

    public static readonly Dictionary<string, int> MovesPriority = new Dictionary<string, int>()
    {
        { "idle", 1 },
        { "run", 2 },
        { "sprint", 3 },
        { "jump_run", 10 },
        { "jump_sprint", 10 },
        { "midair", 10 },
        { "landing_run", 10 },
        { "landing_sprint", 10 },
    };

    public static int MovesPrioritySort(string a, string b)
    {
        int priorityA = MovesPriority.GetValueOrDefault(a, 0);
        int priorityB = MovesPriority.GetValueOrDefault(b, 0);

        return priorityB.CompareTo(priorityA);
    }

    public void MarkEnterState()
    {
        _enterStateTime = Time.GetTicksMsec();
    }

    public double GetProgress()
    {
        return (Time.GetTicksMsec() - _enterStateTime) / 1000.0;
    }

    public bool WorksLongerThan(float time) => GetProgress() >= time;
    public bool WorksLessThan(float time) => GetProgress() < time;
    public bool WorksBetween(float start, float finish)
    {
        double progress = GetProgress();
        return progress >= start && progress <= finish;
    }

    public virtual string CheckRelevance(InputPackage input)
    {
        GD.PrintErr("error, implement the check_relevance function on your state");
        return "error, implement the check_relevance function on your state";
    }

    public virtual void Update(InputPackage input, float delta)
    {
    }

    public virtual void OnEnterState()
    {
    }

    public virtual void OnExitState()
    {
    }
}
/***
```
  General New State usage guide.

> check_relevance function aims to be short and simple.
Its general structure is as follows: 
if (move is ready to transition) :
transition to the highest priority out there
else:
return "okay" to save our managing status.

Move readyness for transition is generally a simple function based on timings or statuses of the player.
If you are starting to understand that your transition readyness is a complex method, OR
if you are tempted to add third branching operator into your check_relevance function,
seriously consider if Combo can do this logic for you, you won't regret its usage I promise.
(Combo is clickable even from comments btw)

> update functions manages perframe behaviour of your Move.
There are two update types: constant change and a single dynamic update on some timing.
To implement simple constant changes, try to find some physics abstraction for them to make
engine work for you.If your constant changes are too complex, try to avoid hardcoding
the behaviour into a giant update, better shove the changes data into a backend animation or
some other data structure resource.
To implement timed changes, use a flag and work with timings via get_progress() and Co.
To roughly base your internal timings on the players behaviour, you can check skeleton
animation for reference.But for the love of god please avoid referensing skeleton and animator
in any shape way or form in the Moves code directly.This way your Move "backend" is free from
thousand different ways someone (probably you from the future) can mess up your skeleton, scene composition,
animations, names libraries etc.
```
 ***/
