using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[ExecuteInEditMode]
public class ZoneValueDisplay : MonoBehaviour {

#if UNITY_EDITOR
	private void OnTransformParentChanged() {

		LandZone zone = GetComponentInParent<LandZone>();

		SetValue(zone.Value);

		EditorUtility.SetDirty(textMesh);
	}
#endif

	[SerializeField] private TextMeshPro textMesh;

	public void SetValue(int value) {

		textMesh.text = value.ToString();

		Vector3 scale = Vector3.one;

		if (value < 3) {
			scale = new Vector3(0.75f, 0.75f, 1.0f);
		} else if (value < 8) {
			scale = new Vector3(0.9f, 0.9f, 1.0f);
		}

		textMesh.transform.localScale = scale;
	}


}
