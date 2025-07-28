using UnityEngine;

public abstract class AirUnit : Unit{

    public override MoveType MovementType { get { return MoveType.AIR; } }

}

