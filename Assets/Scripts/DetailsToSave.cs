using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DetailsToSave
{
    public int rowCount;
    public int columnCount;
    public int TurnCounter;
    public int Score;
    public int GameEndCounter;
    public List<string> ImageNameList = new List<string>();
}
