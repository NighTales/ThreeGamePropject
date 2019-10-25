using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class ScoreViewScript : MonoBehaviour
{
    public Button[] buttons = new Button[LoadLevel.countChallenge];

    public Color normalColor;
    public Color selectColor;

    //public ScoreData score;

    public NewScoreData[] nscore;

    public GameObject contentListView;
    public GameObject scoreField;

    public int currtentScore = -1;
    public Text DebugText;

    [HideInInspector]
    public List<GameObject> gameObjects = new List<GameObject>();

    Image[] images = new Image[LoadLevel.countChallenge];

    private void Awake()
    {
        for (int i = 0; i < LoadLevel.countChallenge; i++)
            images[i] = buttons[i].image;

        //for (int i = 0; i < nscore.Length; i++)
        //{
        //    var d = Resources.Load(dataFilename[i]);
        //    Debug.Log(d.name + "  " + d.GetType() + " " + assets.ToString());
        //}

        foreach (var s in nscore)
            s.Load();

        //Get the path of the Game data folder
        string m_Path = Application.dataPath;

        //Output the Game data path to the console
        DebugText.text = ("Path : " + m_Path);
    }

    private void Start()
    {
        buttons[0].onClick.AddListener(Button1_Click);
        buttons[1].onClick.AddListener(Button2_Click);
        buttons[2].onClick.AddListener(Button3_Click);


    }

    private void OnEnable()
    {
        Button_Click(0);
    }

    void DrowImage(int index)
    {
        for (int i = 0; i < LoadLevel.countChallenge; i++)
            if (i != index)
                images[i].color = normalColor;
            else
                images[i].color = selectColor;
    }
    public void Button1_Click() => Button_Click(0);
    public void Button2_Click() => Button_Click(1);
    public void Button3_Click() => Button_Click(2);

    public void Button_Click(int index)
    {
        DrowImage(index);
        if (currtentScore != -1)
        {
            foreach (var g in gameObjects)
            {
                Destroy(g);
            }

            gameObjects.Clear();
        }

      

        for (int i = 0; i < nscore[index].scores.Count; i++)
        {
            ScoreField f = nscore[index].scores[i];
            var go = Instantiate(scoreField, contentListView.transform);
            gameObjects.Add(go);
            var sfs = go.GetComponent<ScoreFieldScript>();
            sfs.Name = f.myname;
            sfs.Score = f.score.ToString();
            sfs.Number = (i + 1).ToString();
        }


        currtentScore = index;
    }
}

public static class DataManager
{
    
    public static void Save(NewScoreData data, string filepath)
    {
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        using (var stream = File.CreateText(filepath))
        {
            foreach (var sc in data.scores)
            {
                string s = JsonUtility.ToJson(sc);
                stream.WriteLine(s);
            }
        }
        Debug.Log("Save filepath: " + filepath);
    }

    public static List<ScoreField> Load(string filepath)
    {
        List<ScoreField> scores = new List<ScoreField>();

        string[] sa = File.ReadAllLines(filepath);

        foreach (var s in sa)
            scores.Add(JsonUtility.FromJson<ScoreField>(s));

        Debug.Log("Load filepath: " + filepath);

        return scores;
    }
}
