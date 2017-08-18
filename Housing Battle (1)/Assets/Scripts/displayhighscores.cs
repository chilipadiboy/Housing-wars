using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayhighscores : MonoBehaviour {

    public Text[] leaderBoardText;
    Highscore leaderBoardFunctions;
	// Use this for initialization
	void Start () {
		for (int i=0; i < leaderBoardText.Length; i++)
        {
            leaderBoardText[i].text = i + 1 + ". Loading score...";
        }

        leaderBoardFunctions = GetComponent<Highscore>();
        StartCoroutine("RefreshScores");
    }
	
    public void OnScoresDownloaded(leaderScore[] highscoreList)
    {
        for (int i=0;i<leaderBoardText.Length; i++)
        {
            leaderBoardText[i].text = i + 1 + ". ";
            if (highscoreList.Length > i)
            {
                leaderBoardText[i].text += highscoreList[i].username + " - " + highscoreList[i].score;
            }
        }
    }
	IEnumerator RefreshScores()
    {
        while (true)
        {
            leaderBoardFunctions.downloadHighscores();
            yield return new WaitForSeconds(20);
        }
    }
}
