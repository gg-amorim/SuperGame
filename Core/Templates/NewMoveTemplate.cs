using Godot;

// Step 0: redefine your namespace
namespace MMO.Core.Templates.Node;

// Step 1: redefine your ClassName
public partial class NewState : State
{
    // Step 2: redefine your overriden parameters 

    public override void _Ready()
    {
        AnimationStr = "sprint";
    }

    // Step 3: navigate to PlayerModel and add a new state to moves dictionary
    // Step 4: navigate to base State and add this new state to priority dictionary

    // Step 5: implement a check_relevance function to manage transitions, return action name or "okay"
    public override string CheckRelevance(InputPackage input)
    {
        return "implement the CheckRelevance function on your state";
    }

    // Step 6: implement an update function
    public override void Update(InputPackage input, float delta)
    {
    }

    // Step 7: use these bros if needed
    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
    }

    // Step 8: delete annoying comments telling you what to do
}
