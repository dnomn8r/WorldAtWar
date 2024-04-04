using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialGameState", menuName = "InitialStates/InitialGameState", order = 1)]
public class InitialGameState : ScriptableObject{

	[SerializeField] private InitialOwnershipState initialLandTerritoryOwnership;
	public InitialOwnershipState InitialLandTerritoryOwnership { get { return initialLandTerritoryOwnership; } }

	[SerializeField] private InitialLandTerritoryState initialNeutralState;
	public InitialLandTerritoryState InitialNeutralState { get { return initialNeutralState; } }

	[SerializeField] private List<InitialCountryState> initialCountryStates;

	public List<InitialCountryState> InitialCountryStates { get { return initialCountryStates; } }

}

