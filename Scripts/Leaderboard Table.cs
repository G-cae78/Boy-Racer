using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardTable : MonoBehaviour
{
    public static LeaderboardTable instance;
    public GameObject leaderboardContainer;
    public Transform leaderboardTemplate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     private void Awake() {
       // leaderboardTemplate.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateLeaderBoard(List<string> finishingOrder){
    // Clear previous entries
    foreach (Transform child in leaderboardTemplate)
    {
        Destroy(child.gameObject);
    }
   int pos=0;
   Vector3 positionOffset = Vector3.zero;
    // Populate leaderboard with new entries
    foreach (var player in finishingOrder)
    {
        pos=pos+1;
        GameObject entry = Instantiate(leaderboardContainer, leaderboardTemplate);
       // Set the position of the new entry, adjusting its vertical position by 'verticalSpacing'
            positionOffset.y = -pos * 3f;
            entry.transform.localPosition = positionOffset;
        LeaderboardUI leaderboardUI= entry.GetComponent<LeaderboardUI>();
        if(leaderboardUI!=null){
          leaderboardUI.SetEntryData(player,pos);
          Debug.Log("Entry added: " + player + " " + pos);
        }else {
           Debug.LogError("LeaderboardUI component is missing from the instantiated prefab.");
        }
        
    }
}
    
}
