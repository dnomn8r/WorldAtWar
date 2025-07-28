using UnityEngine;

public abstract class SeaUnit : Unit{

    public override MoveType MovementType { get { return MoveType.SEA; } }
}

