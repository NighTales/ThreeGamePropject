using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ScoreData", menuName = "Score Data"), Serializable]
public class ScoreData : ScriptableObject
{
    [SerializeField]
    public List<ScoreField>[] scores;

    public void Save()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void Load()
    {

    }

    public ScoreData()
    {
        scores = new List<ScoreField>[LoadLevel.countChallenge];
        for (int i = 0; i < LoadLevel.countChallenge; i++)
        {
            scores[i] = new List<ScoreField>();
        }
    }
}
