using System;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text positionText;

    public void SetEntryData(string name, int position)
    {
        if (nameText != null)
            nameText.text = name;
        else
            Debug.LogError("nameText is not assigned!");

        if (positionText != null)
            positionText.text = position.ToString();
        else
            Debug.LogError("positionText is not assigned!");


           Debug.Log($"{name} {position}");

    }

    
}
