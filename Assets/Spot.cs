using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {


    [SerializeField]
    private bool isGroup;
    public bool IsGroup {
        get { return this.isGroup; }
    }


    private bool isFree;
    public bool IsFree {
        get { return this.isFree; }
    }

    void OnGUI() {
        
    }
}
