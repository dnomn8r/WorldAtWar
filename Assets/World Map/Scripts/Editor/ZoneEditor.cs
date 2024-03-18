using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Zone))]
public class InteractableItemsEditor : Editor {

	//[InitializeOnLoad]
	public class Startup {

		[UnityEditor.Callbacks.DidReloadScripts]
		private static void ScriptsHasBeenReloaded() {
			SceneView.duringSceneGui += DuringSceneGui;
		}
		[UnityEditor.Callbacks.DidReloadScripts]
		static Startup() {

			Selection.selectionChanged += OnSelectionChanged;
		}

		private static void DuringSceneGui(SceneView sceneView) {
			Event e = Event.current;

			if (e.type == EventType.KeyDown) {

				if (Selection.gameObjects.Length > 0) {

					Zone zone = Selection.gameObjects[0].GetComponent<Zone>();

					if (zone != null) {

						if (e.keyCode == KeyCode.Backspace) {

							zone.Adjacencies.Clear();

							zone.ToggleSelection(true);

							//Debug.Log("clearing selection from: " + zone.name);
						
						}else if(e.keyCode == KeyCode.Return) {

							for(int i = 1; i < Selection.gameObjects.Length; i++) { 
							
								Zone adjacentZone = Selection.gameObjects[i].GetComponent<Zone>();

								if(adjacentZone == null) { continue; }

								if (!zone.Adjacencies.Contains(adjacentZone)) {
									zone.Adjacencies.Add(adjacentZone);
								}

								if(!adjacentZone.Adjacencies.Contains(zone)) {
									adjacentZone.Adjacencies.Add(zone);	
								}
							}

							zone.ToggleSelection(true);

							//Debug.Log("setting selection to: " + zone.name);

						} else if (e.keyCode == KeyCode.P && zone is LandZone) {

							LandZone landZone = (LandZone)zone;

							for (int i = 1; i < Selection.gameObjects.Length; i++) {

								LandZone adjacentZone = Selection.gameObjects[i].GetComponent<LandZone>();

								if (adjacentZone == null) { continue; }

								if (!landZone.HazardousAdjacencies.Contains(adjacentZone)) {
									landZone.HazardousAdjacencies.Add(adjacentZone);
								}

								if (!adjacentZone.HazardousAdjacencies.Contains(landZone)) {
									adjacentZone.HazardousAdjacencies.Add(landZone);
								}
							}

							landZone.ToggleSelection(true);

							//Debug.Log("setting selection to: " + zone.name);
						}
					}
				}
			}
		}

		private static Zone currentZone;
		private static void OnSelectionChanged() {

			for (int i = 0; i < Selection.gameObjects.Length; i++) {

				Zone selectedZone = Selection.gameObjects[0].GetComponent<Zone>();
				
				if (selectedZone != currentZone && currentZone != null) {

					//Debug.Log("zone: " + currentZone.name + " deselected");
					currentZone.ToggleSelection(false);
				}

				currentZone = selectedZone;

				if (selectedZone != null) {

					//Debug.Log("we've selected: " + selectedZone.name);

					currentZone.ToggleSelection(true);
				}
			}

			if(Selection.gameObjects.Length == 0 && currentZone != null) {
				currentZone.ToggleSelection(false);
				currentZone = null;
			}
		}
	}

}