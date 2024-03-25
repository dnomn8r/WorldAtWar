using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialGameState", menuName = "InitialStates/InitialGameState", order = 1)]
public class InitialGameState : ScriptableObject{

	[SerializeField] private InitialOwnershipState initialLandTerritoryOwnership;

	[SerializeField] private InitialLandTerritoryState initialLandTerritoryState;

}

