using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
	
	const string privateCode = "vj4FUe6bEEKrwnG6cHUkcAPdkDXYKeWE6wHB0vVQHmYg";
	const string publicCode = "599276e3ef12d81294044226";
	const string webURL = "http://dreamlo.com/lb/";

	public string test1;
	public string test2;

	[SerializeField]
	private int currScore1;

	[SerializeField]
	private int currScore2;

	[SerializeField]
	private int currGames1;

	[SerializeField]
	private int currGames2;

	void Start () {
		
	}

	public void TestSetScore(){
		GameOver (test1, test2, false);
	}

	public void GameOver(string winner, string loser, bool isDraw){
		StartCoroutine(GameOverCoroutine(winner, loser, isDraw));
	}

	public void AddNewPlayer(string user){
		StartCoroutine (AddNew (user));
	}

	IEnumerator AddNew(string user){
		WWW downloadURL = new WWW(webURL + publicCode + "/pipe-get/"+ user);
		Debug.Log(webURL + publicCode + "/pipe-get/" + user);
		yield return downloadURL;

		if (string.IsNullOrEmpty (downloadURL.error)) {
			// do nothing, name already exists
		} else {
			yield return addHighscore (user, 0, 0);
			Debug.Log ("Adding new player " + user);
		}
	}

	IEnumerator GameOverCoroutine(string winner, string loser, bool isDraw){
		// Get winner score, games and delete their score
		yield return getDoubleScore (winner, loser);

		int winnerScore = currScore1;
		int winnerGames = currGames1;
		yield return delHighscore (winner);
		Debug.Log ("Winner score is " + winnerScore + " and winner games is " + winnerGames);

		// Get loser score, games and delete their score
		int loserScore = currScore2;
		int loserGames = currGames2;
		Debug.Log ("Loser score is " + loserScore + " and loser games is " + loserGames);
		yield return delHighscore (loser);

		if (isDraw) {
			// calculate new elo
			int newWinnerScore = (winnerScore * winnerGames + loserScore)/ (winnerGames + 1);
			int newLoserScore = (loserScore * loserGames + winnerScore) / (loserGames + 1);

			// update new elo
			yield return addHighscore (winner, newWinnerScore, winnerGames + 1);
			yield return addHighscore (loser, newLoserScore, loserGames + 1);
		} else {
			// calculate new elo
			int newWinnerScore = (winnerScore * winnerGames + loserScore + 400)/ (winnerGames + 1);
			int newLoserScore = Mathf.Max(0, (loserScore * loserGames + winnerScore - 400) / (loserGames + 1));

			// update new elo
			yield return addHighscore (winner, newWinnerScore, winnerGames + 1);
			yield return addHighscore (loser, newLoserScore, loserGames + 1);
		}

	}

	public void getTwoPlayersScore(string user1, string user2){
		StartCoroutine (getDoubleScore (user1, user2));
	}


	public int getSingleHighScore(string username) ///get a single persons highscore
	{
		StartCoroutine(getSingleScore(username, 1));
		return currScore1;
	}

	public int getSingleGames(string username) ///get a single persons games
	{
		StartCoroutine(getSingleScore(username, 2));
		return currGames1;
	}


	IEnumerator getSingleScore(string username, int field)
	{
		WWW downloadURL = new WWW(webURL + publicCode + "/pipe-get/"+ username);
		Debug.Log(webURL + publicCode + "/pipe-get/" + username);
		yield return downloadURL;

		if (string.IsNullOrEmpty(downloadURL.error))
		{
			Debug.Log(downloadURL.text);
			string[] entry = downloadURL.text.Split(new char[] { '|' });
			if (field == 1) {
				currScore1 = int.Parse(entry [field]);
			} else if (field == 2) {
				currGames1 = int.Parse(entry [field]);
			}
		}
		else
		{
			Debug.Log("Error downloading score " + downloadURL.error);
		}
	}


	IEnumerator getDoubleScore(string user1, string user2)
	{
		WWW downloadURL1 = new WWW(webURL + publicCode + "/pipe-get/"+ user1);
		WWW downloadURL2 = new WWW(webURL + publicCode + "/pipe-get/"+ user2);
		Debug.Log(webURL + publicCode + "/pipe-get/" + user1);
		Debug.Log(webURL + publicCode + "/pipe-get/" + user2);
		yield return downloadURL1;
		yield return downloadURL2;

		if (string.IsNullOrEmpty(downloadURL1.error))
		{
			Debug.Log(downloadURL1.text);
			string[] entry1 = downloadURL1.text.Split(new char[] { '|' });
			currScore1 = int.Parse(entry1 [1]);
			currGames1 = int.Parse(entry1 [2]);
		}
		else
		{
			Debug.Log("Error downloading score " + downloadURL1.error);
		}

		if (string.IsNullOrEmpty(downloadURL2.error))
		{
			Debug.Log(downloadURL2.text);
			string[] entry2 = downloadURL2.text.Split(new char[] { '|' });
			currScore2 = int.Parse(entry2 [1]);
			currGames2 = int.Parse(entry2 [2]);
		}
		else
		{
			Debug.Log("Error downloading score " + downloadURL2.error);
		}
	}

	IEnumerator delHighscore(string username)
	{
		WWW uploadURL = new WWW(webURL + privateCode + "/delete/" + WWW.EscapeURL(username));
		yield return uploadURL;

		if (string.IsNullOrEmpty(uploadURL.error))
		{
			Debug.Log("Delete Successful");
		}
		else
		{
			Debug.Log("Delete failed " + uploadURL.error);
		}
	}

	public void AddNewHighscore(string username, int score, int games) //add high score
	{
		StartCoroutine(addHighscore(username, score, games));
	}

	IEnumerator addHighscore(string username, int score, int games)
	{
		WWW uploadURL = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score + "/" + games);
		yield return uploadURL;

		if (string.IsNullOrEmpty(uploadURL.error))
		{
			Debug.Log("Upload Successful");
		}
		else
		{
			Debug.Log("Error uploading score " + uploadURL.error);
		}
	}


}