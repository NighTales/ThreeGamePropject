using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Threading;
using System.Runtime;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "newScoreData", menuName = "newScore Data")]
public class NewScoreData : ScriptableObject
{
    public string filepath;
    public List<ScoreField> scores;

    public void Save()
    {
        SetDirty();
        scores = (from c in scores
                  orderby c.score
                  select c).Reverse().ToList();
        DataManager.Save(this, Application.dataPath + filepath);
    }
    public void Load()
    {
        scores = DataManager.Load(Application.dataPath + filepath);
    }

    public NewScoreData()
    {
        scores = new List<ScoreField>();
    }
}
