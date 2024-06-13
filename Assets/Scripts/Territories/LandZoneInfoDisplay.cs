
public class LandZoneInfoDisplay : ZoneInfoDisplay {

	public override void Refresh() {

		// for now, don't do anything special here

		base.Refresh();

		LandZone landZone = zone as LandZone;

		if(zone == null || landZone.CurrentOwner == null) {
			return;
		}

    }


}
