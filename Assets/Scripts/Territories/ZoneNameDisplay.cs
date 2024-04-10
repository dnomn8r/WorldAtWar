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

	public void SetHoverState(bool hover, bool isAdjacency, bool isHazard) {

		if(originalString == null) {
			originalString = textMesh.text;
		}

		if (isAdjacency) {
            textMesh.text = "<color=" + (isHazard ? "red" : "green") + ">" + originalString + ">";
        } else if (hover) {
			textMesh.text = "<color=yellow>" + originalString + ">";
		} else {
			textMesh.text = originalString;
		}
    }


}
