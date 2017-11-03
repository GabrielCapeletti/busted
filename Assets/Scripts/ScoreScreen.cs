using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour {

    public static string END_TEXT = "";

    public TextMesh text;

	void Start () {
		
	}

    public void Open()
    {
        gameObject.SetActive(true);
        text.text = END_TEXT;
    }
}
