using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMessages : MonoBehaviour {
   
    public bool isKeyboard = true;

	void Start () {
        gameObject.SetActive((Input.GetJoystickNames().Length == 0) == isKeyboard);
    }
	
	void Update () {

        
	}
}
