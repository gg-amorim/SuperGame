using Godot;
using MMO.Scripts.Players;
using MMO.Scripts.Players.States.Combos.Sword;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace MMO.Core;

public partial class State : Node
{
    public Player Player { get; set; }
    public ResourcesComponent Resources { get; set; }
    public MovesData MovesData { get; set; }
    [Export] public string AnimationStr { get; set; }
    [Export] public string BackendAnimation { get; set; }
    [Export] public AnimationPlayer Animator { get; set; }
    [Export] public float StaminaCost { get; set; } = 0f;

    public string StateName { get; set; }

    public bool HasQueuedMove { get; set; } = false;
    public string QueuedMove { get; set; } = "none, drop error please";

    public bool HasForcedMove { get; set; } = false;
    public string ForcedMove { get; set; } = "none, drop error please";

    private List<Combo> _combos = new List<Combo>();

    private ulong _enterStateTime { get; set; }

    public static readonly Dictionary<string, int> MovesPriority = new()
    {
        { "idle", 1 },
        { "run", 2 },
        { "sprint", 3 },
        { "jump_run", 10 },
        { "midair", 10 },
        { "landing_run", 10 },
        { "jump_sprint", 10 },
        { "landing_sprint", 10 },
        { "slash_1", 15 },
        { "slash_2", 15 },
        { "slash_3", 15 },
        { "parry", 20 },
        { "riposte", 25 },
        { "parried", 100 },
        { "staggered", 100 },
        { "death", 200 }
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

    public float GetProgress()
    {
        return (float)((Time.GetTicksMsec() - _enterStateTime) / 1000.0);
    }

    public bool WorksLongerThan(float time) => GetProgress() >= time;
    public bool WorksLessThan(float time) => GetProgress() < time;
    public bool WorksBetween(float start, float finish)
    {
        float progress = GetProgress();
        return progress >= start && progress <= finish;
    }
    public void AssignCombos()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Combo combo)
            {
                _combos.Add(combo);
                combo.State = this; 
            }
        }
    }
    public void CheckCombos(InputPackage input)
    {
        foreach (Combo combo in _combos)
        {
            // Nota: Confirme o caminho de acesso ao dicionário de moves do seu modelo de jogador.
            State triggeredState = Player.Model.States[combo.TriggeredMove];

            if (combo.IsTriggered(input) && Resources.CanBePaid(triggeredState))
            {
                HasQueuedMove = true;
                QueuedMove = combo.TriggeredMove;
            }
        }
    }

    public virtual string CheckRelevance(InputPackage input)
    {
        if (HasForcedMove)
        {
            HasForcedMove = false;
            return ForcedMove;
        }

        CheckCombos(input);
        return DefaultLifecycle(input);
    }

    public string BestInputThatCanBePaid(InputPackage input)
    {
        // O Sort customizado substitui o lambda do GDScript
        input.Actions.Sort(MovesPrioritySort);

        foreach (string action in input.Actions)
        {
            State move = Player.Model.States[action];
            if (Resources.CanBePaid(move))
            {
                if (move == this)
                    return "okay";
                else
                    return action;
            }
        }
        return "throwing because for some reason input.actions doesn't contain even idle";
    }
    public void UpdateResources(float delta)
    {
        Resources.Update(delta);
    }
    public bool IsVulnerable() => MovesData.GetVulnerable(BackendAnimation, GetProgress());

    public bool IsInterruptable() => MovesData.GetInterruptable(BackendAnimation, GetProgress());

    public bool IsParryable() => MovesData.GetParryable(BackendAnimation, GetProgress());


    public virtual string DefaultLifecycle(InputPackage input)
    {
        return "implement default lifecycle pepega " + AnimationStr;
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

    public virtual HitData FormHitData(Weapon weapon)
    {
        GD.Print("someone tries to get hit by default Move");
        return HitData.Blank(); // Supondo que você crie o método estático Blank() em HitData
    }

    public virtual void ReactOnHit(HitData hit)
    {
        if (IsVulnerable())
        {
            Resources.LoseHealth(hit.Damage);
        }
        if (IsInterruptable())
        {
            TryForceMove("staggered");
        }
    }

    public virtual void ReactOnParry(HitData hit)
    {
        TryForceMove("parried");
    }

    public void TryForceMove(string newForcedMove)
    {
        if (!HasForcedMove)
        {
            HasForcedMove = true;
            ForcedMove = newForcedMove;
        }
        else if (MovesPriority.GetValueOrDefault(newForcedMove, 0) >= MovesPriority.GetValueOrDefault(ForcedMove, 0))
        {
            ForcedMove = newForcedMove;
        }
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
