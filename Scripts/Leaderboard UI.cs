using UnityEngine;
using TMPro;

    public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text positionText;

    public void SetEntryData(string name, string position)
    {
        nameText.text = name; //initialising name of car data
        positionText.text = position;// initialising position the car finished
    }
}

