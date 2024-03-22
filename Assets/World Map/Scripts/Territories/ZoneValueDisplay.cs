using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[ExecuteInEditMode]
public class ZoneValueDisplay : MonoBehaviour {

	[SerializeField] private TextMeshPro textMesh;

#if UNITY_EDITOR
	private void OnTransformParentChanged() {

		LandZone zone = GetComponentInParent<LandZone>();

		textMesh.text = zone.Value.ToString();

		Vector3 scale = Vector3.one;

		if(zone.Value < 3) {
			scale = new Vector3(0.75f, 0.75f, 1.0f);
		}else if(zone.Value < 8) {
			scale = new Vector3(0.9f, 0.9f, 1.0f);
		}

		textMesh.transform.localScale = scale;

		EditorUtility.SetDirty(textMesh);
	}
#endif



}
