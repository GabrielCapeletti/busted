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

    void Start () {
        this.spots = this.GetComponents<Spot>();

        foreach (Spot spot in this.spots) {
            if (spot.IsGroup) {
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
        } else {
            this.recursiveCounter++;
            if (this.recursiveCounter > 100) {
                this.recursiveCounter = 0;
                return null;
            }
            else {
                this.FindNextPosition();
            }
        }

        return null;
    }

    public Spot FindNextPosition() {
        float rnd = Random.Range(0f, 1f);
        if (rnd < 0.5f) {
            return this.GetSpotFromList(this.groupList);
        } else {
            return this.GetSpotFromList(this.singleList);
        }
    }
	
}
