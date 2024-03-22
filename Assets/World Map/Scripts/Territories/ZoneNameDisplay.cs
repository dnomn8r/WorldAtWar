using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[ExecuteInEditMode]
public class ZoneNameDisplay : MonoBehaviour {

	[SerializeField] private TextMeshPro textMesh;

#if UNITY_EDITOR
	private void OnTransformParentChanged() {

		Zone zone = GetComponentInParent<Zone>();

		textMesh.text = zone.name;

		textMesh.fontStyle = zone.FontStyle;

		EditorUtility.SetDirty(textMesh);
	}
#endif



}
