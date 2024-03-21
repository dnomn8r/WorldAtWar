using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Country", menuName = "InitialStates/Country", order = 1)]
public class Country : ScriptableObject{

	[SerializeField] private Color ownershipColor;

    [SerializeField] protected List<LandTerritory> landTerritories;

}

