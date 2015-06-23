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

    public static int SCORE = 0;

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

	void Start () 
	{
        sm = FindObjectOfType<SoundManager>();
	    path = Application.dataPath + "/rank.save";
	}
	
	void Update () 
	{
        scoreLable.text = GameManager.SCORE.ToString();
	}

    public void GameOver()
    {
        spawner.SetActive(false);
        gameOverLable.enabled = true;
        WriteRankInfo(new RankInfo(SCORE, Time.time), path);
        for(int i=0; i<Grid.h; ++i)
        {
            Grid.DeleteRow(i);
        }
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
}
