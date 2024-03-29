using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public abstract class Zone : MonoBehaviour {

	[MenuItem("Zones/Clear Adjacencies")]
	static void ClearAdjacencies() {

		Debug.Log("clear all zone adjacencies");

		Zone[] zones = FindObjectsByType<Zone>(FindObjectsSortMode.None);

		foreach (Zone zone in zones) {
			zone.Adjacencies.Clear();
		}
	}


	[MenuItem("Zones/Make Scriptables")]
	static void MakeScriptables() {

		LandZone[] landZones = FindObjectsByType<LandZone>(FindObjectsSortMode.None);

		foreach (LandZone zone in landZones) {

			LandTerritory territory = ScriptableObject.CreateInstance<LandTerritory>();

			territory.Value = zone.Value;

			string path = "Assets/Definitions/LandTerritories/" + zone.name + ".asset";

			AssetDatabase.CreateAsset(territory, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			zone.LandTerritory = territory;
		}

		SeaZone[] seaZones = FindObjectsByType<SeaZone>(FindObjectsSortMode.None);

		foreach (SeaZone zone in seaZones) {

			WaterTerritory territory = ScriptableObject.CreateInstance<WaterTerritory>();

			string path = "Assets/Definitions/WaterTerritories/" + zone.name + ".asset";

			AssetDatabase.CreateAsset(territory, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			zone.WaterTerritory = territory;
		}
	}


	[SerializeField] protected List<Zone> adjacencies = new List<Zone>();

	public abstract Color BaseColor {get;}
	public abstract FontStyles FontStyle { get;}


	public List<Zone> Adjacencies {
		get { return adjacencies; }
	}

	public void ToggleSelection(bool toggle) {

		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer ren in renderers) {
			ren.color = toggle ? Color.green : BaseColor;
		}

		ToggleAdjacencyHighlights(toggle);
	}

	protected void ToggleHighlight(bool toggle, bool isHazard = false) {

		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer ren in renderers) {
			ren.color = toggle ? (isHazard ? Color.red : Color.yellow) : BaseColor;
		}
	}

	protected virtual void ToggleAdjacencyHighlights(bool toggle) {

		foreach (Zone zone in adjacencies) {
	
			if (zone != null) {

				zone.ToggleHighlight(toggle);
			} else {

				Debug.LogError("zone is null in " + gameObject.name + " this should not happen");
			}
		}
	}

}
