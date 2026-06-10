using Godot;


namespace MMO.Core;

public class HitData
{
    public bool IsParryable { get; set; }
    public float Damage { get; set; }
    public string HitMoveAnimation { get; set; }

    public Weapon Weapon { get; set; }

    // O método estático equivalente ao func blank()
    public static HitData Blank()
    {
        return new HitData();
    }
}