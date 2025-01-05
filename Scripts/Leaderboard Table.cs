using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardTable : MonoBehaviour
{
    public static LeaderboardTable instance;
    public Transform leaderboardContainer;
    public Transform leaderboardTemplate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     private void Awake() {
        leaderboardTemplate.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateLeaderBoard(List<string> finishingOrder){
       
       foreach(Transform dataEntry in leaderboardContainer){
        Destroy(dataEntry.gameObject);
       }

       for(int i=0; i<finishingOrder.Count;i++){
        Transform entryTransform = Instantiate(leaderboardTemplate, leaderboardContainer);
            entryTransform.gameObject.SetActive(true);
            LeaderboardUI entry= entryTransform.GetComponent<LeaderboardUI>();
            if(entry!=null){
                entry.SetEntryData(finishingOrder[i],$"{i+1}");
            }
       }
    }
    
}
