using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeaZone : Zone{

	[SerializeField] private WaterTerritory waterTerritory;

	public WaterTerritory WaterTerritory {

		get { return waterTerritory; }
#if UNITY_EDITOR

		set { waterTerritory = value; }
#endif 
	}

    public override Territory Territory {
        get {
            return waterTerritory;
        }
    }


    public override Color BaseColor {
		get {
			return new Color(0.5f, 0.75f, 1.0f);
		}
	}
	public override FontStyles FontStyle {
		get {
			return FontStyles.Italic;
		}
	}

	protected override void Awake() {

		base.Awake();

		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer ren in renderers) {

			if (ren.GetComponentInParent<ZoneInfoDisplay>() == null) {
				ren.color = BaseColor;
			}
		}

	}


}
