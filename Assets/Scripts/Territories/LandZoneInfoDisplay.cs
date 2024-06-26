using System.Collections.Generic;
using static WorldMapManager;

public class LandZoneInfoDisplay : ZoneInfoDisplay {

	public override void Refresh() {

		// for now, don't do anything special here

		base.Refresh();

		LandZone landZone = zone as LandZone;

		if (zone == null || landZone.CurrentOwner == null) {
			return;
		}

		List<General> generalsPresent = WorldMapManager.Instance.GetGeneralsAtTerritory(landZone.LandTerritory);


		for (int i = 0; i < zoneInfoEntries.Count; i++) {
	
			for(int j = 0; j < zoneInfoEntries[i].generals.Count; j++) {

				// disable general displays not used
				zoneInfoEntries[i].generals[j].enabled = j < generalsPresent.Count;
				
				if(j < generalsPresent.Count) {
					zoneInfoEntries[i].generals[j].color = generalsPresent[j].owner.OwnershipColor;
                }
			}
			
		}
	}


}
