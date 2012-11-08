using UnityEngine;
using System.Collections;


/// <summary>
/// A class that intercepts messages from child objects
/// and determines what actions to take for the RedBlueTurrertPlatformer
/// </summary>
public class RedBlueLevelEventMonitor : MonoBehaviour {
	
	// Constants from Unity
	public const string PROJECTILE_TAG = "Projectile";
	
	public const string RED_NAME = "RedProjectile";
	public const string BLUE_NAME = "BlueProjectile";
	
	// Constants from XML
	//  - Is there a way to get these automatically?
	public const string RED_OUTCOME = "chooseRed";
	public const string BLUE_OUTCOME = "chooseBlue";
	
	
	
	/// <summary>
	/// 
	/// </summary>
	public void ObserveEvent(GameObject obj){	
	
		// Honey Badger don't care unless it's
		// a projectile.
		if (obj.tag != PROJECTILE_TAG)
			return;
	
		// Otherwise determine which color of projectile 
		// was passed and take the edge in the story that
		// represents that.
		
		if (obj.name == RED_NAME)
			MiniGameController.endMiniGame(RED_OUTCOME);
		
		if (obj.name == BLUE_NAME)
			MiniGameController.endMiniGame(BLUE_OUTCOME);
	}
	
	
}
