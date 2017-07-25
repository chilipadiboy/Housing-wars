using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public int buildNumber = 0;
	public int player;
	public GameController gameController;
	public CanvasController canvasController;
	public GameObject house1;

	[HideInInspector]
	public PlayerController otherPlayer;

	public const string Started = "PlayerController.Start";
	public const string StartedLocal = "PlayerController.StartedLocal";
	public const string Destroyed = "PlayerController.Destroyed";
	public const string CoinToss = "PlayerController.CoinToss";
	public const string AddHouse = "PlayerController.AddHouse";

	void Awake(){
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}

	void Start(){
		GameObject.Find ("Canvas").GetComponent<CanvasController> ().player = this;
		gameController.playerController = this;
	}

	public override void OnStartClient(){
		base.OnStartClient();
		this.PostNotification (Started);
	}

	public override void OnStartLocalPlayer(){
		base.OnStartLocalPlayer ();
		this.PostNotification (StartedLocal);
	}

	void OnDestroy(){
		this.PostNotification (Destroyed);
	}


	[Command]
	public void CmdCoinToss (){
		bool coinToss = Random.value < 0.5;
		RpcCoinToss(coinToss);
	}

	[ClientRpc]
	void RpcCoinToss (bool coinToss){
		this.PostNotification(CoinToss, coinToss);
	}

	/*
	[Command]
	public void CmdMarkGridSpace(int index){
		int row = index / 5;
		int col = index % 5;
		GameObject newHouse = gameController.AddHouse (row, col);
		NetworkServer.Spawn (newHouse);
		// RpcMarkGridSpace (index);
	}

	[ClientRpc]
	void RpcMarkGridSpace(int index){
		int row = index / 5;
		int col = index % 5;
		gameController.AddHouse (row, col);
	}

	*/

	[Command]
	public void CmdSpawnHouse(int idx){
		RpcSpawnHouse (idx);
	}

	[ClientRpc]
	void RpcSpawnHouse(int idx){
		int row = idx / 5;
		int col = idx % 5;
		GameObject newHouse = gameController.AddHouse(row, col);
		NetworkServer.SpawnWithClientAuthority (newHouse, this.gameObject);
		gameController.SetReference (newHouse, row, col);
		gameController.CmdToggleButtonInteractable (row, col, false);
		RpcEndTurn ();
	}


	[Command]
	public void CmdSpawnHouseHammer(int idx){
		RpcSpawnHouseHammer (idx);
	}

	[ClientRpc]
	void RpcSpawnHouseHammer(int idx){
		int row = idx / 5;
		int col = idx % 5;
		if (buildNumber == 0) {
			GameObject newHouse = gameController.AddHouse(row, col);
			NetworkServer.SpawnWithClientAuthority (newHouse, this.gameObject);
			gameController.SetReference (newHouse, row, col);
			gameController.CmdToggleButtonInteractable (row, col, false);
			buildNumber++;
		} else {
			GameObject newHouse = gameController.AddHouse(row, col);
			NetworkServer.SpawnWithClientAuthority (newHouse, this.gameObject);
			gameController.SetReference (newHouse, row, col);
			gameController.CmdToggleButtonInteractable (row, col, false);
			RpcEndTurn ();
			buildNumber = 0;
		}
	}

	[Command]
	public void CmdUsePowerUp(int player) {
		RpcUsePowerUp (player);
	}

	[ClientRpc]
	void RpcUsePowerUp(int player){
		if (player == 1) {
			// Debug.Log ("success 1");
			gameController.UsePowerUp (1);
		} else {
			// Debug.Log ("success 2");
			gameController.UsePowerUp (2);
		}
	}

	[Command]
	public void CmdSpawnMine(int idx){
		RpcSpawnMine (idx);
	}

	[ClientRpc]
	public void RpcSpawnMine(int idx){
		GameObject newMine = null;
		int row = idx / 5;
		int col = idx % 5;
		if (isLocalPlayer) {
			newMine = gameController.AddMine (row, col);
		}
		// NetworkServer.SpawnWithClientAuthority (newMine, this.gameObject);
		gameController.SetReferenceMine (newMine, row, col);
		gameController.canvasController.powerUp = -1;
	}

	[Command]
	public void CmdBomb(int idx){
		RpcBomb (idx);
	}

	[ClientRpc]
	void RpcBomb(int idx){
		int row = idx / 5;
		int col = idx % 5;
		gameController.CmdSetBuildingsInteractable (true);
		gameController.Bomb (row, col);
		RpcEndTurn ();
	}

	/*
	[Command]
	public void CmdDestroyObject(int row, int col){
		gameController.DestroyObject (row, col);
	}
	*/

	void EndTurn(){
		RpcEndTurn();
	}

	[ClientRpc]
	void RpcEndTurn(){
		gameController.EndTurn ();
		// gameController.playerController = otherPlayer;
		// canvasController.player = otherPlayer;
	}
}
