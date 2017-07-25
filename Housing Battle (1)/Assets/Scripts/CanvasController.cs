using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

	public PlayerController player;
	public int powerUp = -1;

	public void OnGridSpaceClick (int idx) {
		// Debug.Log (Network.player.ToString ());
		if (powerUp == -1) {
			player.GetComponent<PlayerController> ().CmdSpawnHouse (idx);
		} else if (powerUp == 0) {
			player.GetComponent<PlayerController> ().CmdSpawnMine (idx);
		} else if (powerUp == 1) {
			player.GetComponent<PlayerController> ().CmdSpawnHouseHammer (idx);
		} else {
			player.GetComponent<PlayerController> ().CmdBomb (idx);
		}
		// Debug.Log (player.GetComponent<PlayerController> ().player + ", " + player.GetComponent<PlayerController> ().gameController.GetPlayer ());
	}

	public void OnPowerUpClick1(){
		player.GetComponent<PlayerController> ().CmdUsePowerUp (1);
	}

	public void OnPowerUpClick2(){
		player.GetComponent<PlayerController> ().CmdUsePowerUp (2);
	}
}
