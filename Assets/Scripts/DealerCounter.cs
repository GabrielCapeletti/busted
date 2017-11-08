using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerCounter : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public Transform mask;

    private List<GameObject> junks;

	void Start () {
        float percentX = 1f - (float)(GameManager.CROWDED - GameManager.PEOPLE_ON_PARTY) / (float)(GameManager.CROWDED - GameManager.MIN_PEOPLE);
        mask.localScale = new Vector3(0.6f + (0.4f * percentX), 1, 1);
        Debug.Log(percentX);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
