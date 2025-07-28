using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Country", menuName = "InitialStates/Country", order = 1)]
public class Country : ScriptableObject{

	[SerializeField] protected Sprite flag;

	public Sprite Flag {
		get {
			return flag;
		}

		set { flag = value;}
	}


	[SerializeField] private Color ownershipColor = Color.white;
	public Color OwnershipColor { get { return ownershipColor; } }

    [SerializeField] protected List<LandTerritory> landTerritories;

	public List<LandTerritory> LandTerritories {
		get { return landTerritories; }
	}
}

