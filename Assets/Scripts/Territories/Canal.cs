using System.Collections.Generic;
using UnityEngine;

public class Canal : MonoBehaviour{

	[SerializeField] private List<LandZone> owners;
	public List<LandZone> Owners { get {  return owners; } }

	[SerializeField] private SeaZone firstSeaZone;
	public SeaZone FirstSeaZone { get {  return firstSeaZone; } }

	[SerializeField] private SeaZone secondSeaZone;
	public SeaZone SecondSeaZone { get { return secondSeaZone; } }



}

