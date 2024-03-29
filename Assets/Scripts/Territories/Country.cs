using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Country", menuName = "InitialStates/Country", order = 1)]
public class Country : ScriptableObject{

	[SerializeField] private Color ownershipColor;
	public Color OwnershipColor { get { return ownershipColor; } }

    [SerializeField] protected List<LandTerritory> landTerritories;

	public List<LandTerritory> LandTerritories {
		get { return landTerritories; }
	}
}

