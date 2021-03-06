using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CanvasController : NetworkBehaviour {

	private GameController gameController;

	[SerializeField]
	private PlayerController player;

	[SerializeField]
	private PlayerController player1;

	[SerializeField]
	private PlayerController player2;

	public int powerUp = -1;

	public void SetGameControllerReference(GameController controller){
		gameController = controller;
	}

	public void OnGridSpaceClick (int idx) {
		if (!player.isLocalPlayer) {
			return;
		}

		if (powerUp == -1) {
			player.GetComponent<PlayerController> ().CmdSpawnHouse (idx);
		} else if (powerUp == 0) {
			player.GetComponent<PlayerController> ().CmdSpawnMine (idx);
		} else if (powerUp == 1) {
			player.GetComponent<PlayerController> ().CmdSpawnHouseHammer (idx);
		} else {
			player.GetComponent<PlayerController> ().CmdBomb (idx);
		}
	}

	public void OnPowerUpClick1(){
		player.GetComponent<PlayerController> ().CmdUsePowerUp (1);
	}

	public void OnPowerUpClick2(){
		player.GetComponent<PlayerController> ().CmdUsePowerUp (2);
	}

	public void ChangePlayer(PlayerController nextPlayer){
		player = nextPlayer;
	}

	public void SetPlayer1(PlayerController newPlayer1){
		player1 = newPlayer1;
	}

	public void SetPlayer2(PlayerController newPlayer2){
		player2 = newPlayer2;
	}

	void Update(){
		if (gameController.GetPlayer () == 1) {
			player = player1;
		} else {
			player = player2;
		}
	}
}
