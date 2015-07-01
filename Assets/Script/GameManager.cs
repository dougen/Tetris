using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    private string path;
    private SoundManager sm;

    public GameObject spawner;
    public Text scoreLable;
    public Text gameOverLable;
    public GameObject startPanel;

    public static int SCORE = 0;
    public enum GameState { START, GAMING, GAMEOVER };
    public static GameState state;


    private struct RankInfo
    {
        public int score;
        public float time;

        public RankInfo(int s, float t)
        {
            score = s;
            time = t;
        }
    }

    // 在所有脚本激活之前赋值
    private void Awake()
    {
        state = GameState.START;
    }

	void Start () 
	{
        sm = FindObjectOfType<SoundManager>();
	    path = Application.dataPath + "/rank.save";
	}
	
	void Update () 
	{
        scoreLable.text = GameManager.SCORE.ToString();
        if (state == GameState.GAMEOVER)
        {
            if (Input.anyKey)
            {
                GameStart();
            }
        }
	}

    public void GameOver()
    {
        for (int i = 0; i < Grid.h; ++i)
        {
            Grid.DeleteRow(i);
        }
        state = GameState.GAMEOVER;
        spawner.SetActive(false);
        gameOverLable.enabled = true;
        WriteRankInfo(new RankInfo(SCORE, Time.time), path);
        sm.a.Stop();
        sm.PlayGameOver();
    }

    private void WriteRankInfo(RankInfo rank, string path)
    {
        FileInfo rankInfo = new FileInfo(path);
        StreamWriter sw = null;
        if (rankInfo.Exists)
        {
            sw = rankInfo.CreateText();
        }
        else
        {
            sw = rankInfo.AppendText();
        }

        string myRank = rank.score + "," + rank.time;
        sw.WriteLine(myRank);
        sw.Close();
        sw.Dispose();
    }

    private List<RankInfo> ReadRankInfo(string path)
    {
        FileInfo rankInfo = new FileInfo(path);
        if (!rankInfo.Exists)
        {
            return null;
        }

        StreamReader sr = null;
        sr = rankInfo.OpenText();
        List<RankInfo> ranks = new List<RankInfo>();
        string line = sr.ReadLine();
        while (line != null)
        {
            string[] words = line.Split(',');
            RankInfo rank = new RankInfo(int.Parse(words[0]), float.Parse(words[1]));
            ranks.Add(rank);
            line = sr.ReadLine();
        }
        sr.Close();
        sr.Dispose();
        return ranks;
    }

    public void GameStart()
    {
        for (int i = 0; i < Grid.h; ++i)
        {
            Grid.DeleteRow(i);
        }
        state = GameState.GAMING;
        startPanel.SetActive(false);
        spawner.SetActive(true);
        SCORE = 0;
        spawner.GetComponent<Spawner>().SpawnNext();
        sm.a.Play();
        gameOverLable.enabled = false;
    }
}
