using System;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[ExecuteInEditMode]
public class ZoneNameDisplay : MonoBehaviour {

	private string originalString = null;

	[SerializeField] private TextMeshPro textMesh;

#if UNITY_EDITOR
	private void OnTransformParentChanged() {

		Zone zone = GetComponentInParent<Zone>();

		textMesh.text = zone.name;

		textMesh.fontStyle = zone.FontStyle;

		EditorUtility.SetDirty(textMesh);
	}
#endif

	public void SetNameColor(string colorName) {

		if(originalString == null) {
			originalString = textMesh.text;
		}

		if (!string.IsNullOrEmpty(colorName)) {

            textMesh.text = "<color=" + colorName + ">" + originalString + "</color>";
        } else { 
			textMesh.text = originalString;
		}
    }


}
