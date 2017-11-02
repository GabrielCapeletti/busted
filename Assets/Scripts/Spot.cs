using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    [SerializeField]
    private int groupId;
    public int GroupId {
        get { return this.groupId; }
    }

    [SerializeField]
    private bool isGroup;
    public bool IsGroup {
        get { return this.isGroup; }
    }

    private bool isFree = true;
    public bool IsFree {
        get { return this.isFree; }
        set { this.isFree = value; }
    }

    private CharacterAI lastOccupiedBy;
    public CharacterAI LastOccupiedBy {
        get { return this.lastOccupiedBy; }
        set { this.lastOccupiedBy = value; }
    }

    void Start() {
        if (!this.isGroup) {
            this.groupId = -1;
        }
    }

    public void Occupy(CharacterAI character) {
        this.isFree = false;
        this.lastOccupiedBy = character;
    }

    

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position,0.2f);
    }
}
