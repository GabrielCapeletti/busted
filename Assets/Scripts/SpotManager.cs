using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotManager : MonoBehaviour {

    #region SINGLETON PATTERN
    public static SpotManager _instance;
    public static SpotManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<SpotManager>();

                if (_instance == null) {
                    GameObject container = new GameObject();
                    _instance = container.AddComponent<SpotManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    private Spot[] spots;
    private List<Spot> groupList = new List<Spot>();
    private List<Spot> singleList= new List<Spot>();
    private int recursiveCounter = 0;
    private Dictionary<int, List<Spot>> groupMap = new Dictionary<int, List<Spot>>();

    void Awake () {
        this.spots = this.GetComponentsInChildren<Spot>();

        foreach (Spot spot in this.spots) {
            if (spot.IsGroup) {
                if (!this.groupMap.ContainsKey(spot.GroupId))
                    this.groupMap[spot.GroupId] = new List<Spot>();

                this.groupMap[spot.GroupId].Add(spot);

                this.groupList.Add(spot);
            } else {
                this.singleList.Add(spot);
            }
        }
    }

    private Spot GetSpotFromList(List<Spot> list) {
        if (list.Count == 0)
            return null;

        int i = Random.Range(0, list.Count);
        if (list[i].IsFree) {
            return list[i];
        }

        this.recursiveCounter++;
        if (this.recursiveCounter > 1000) {
            this.recursiveCounter = 0;
            return null;
        }

        return this.FindNextPosition();
    }

    public Spot FindNextPosition() {
        float rnd = Random.Range(0f, 1f);
        if (rnd < 0.5f) {
            return this.GetSpotFromList(this.groupList);
        } else {
            return this.GetSpotFromList(this.singleList);
        }
    }

    public CharacterAI hasSomeoneOnGroup(Spot exception) {
        foreach (Spot spot in this.groupMap[exception.GroupId]) {
            if (exception.gameObject != spot && !spot.IsFree)
                return spot.LastOccupiedBy;
        }

        return null;
    }
	
}
