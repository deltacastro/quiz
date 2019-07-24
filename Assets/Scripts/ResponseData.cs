using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResponseData
{
    public int code;
    public string status;

    public RoundData[] allRoundData;
}
