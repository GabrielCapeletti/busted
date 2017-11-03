using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public List<GameObject> steps;
    public float timeStep = 2;

    private int currentStep = 0;

	void Start () {
        steps[currentStep].SetActive(true);
        Invoke("NextStep", timeStep);
    }

    public void NextStep()
    {
        currentStep++;

        if(currentStep < steps.Count)
        {
            steps[currentStep].SetActive(true);
            Invoke("NextStep", timeStep);
            return;
        }

        gameObject.SetActive(false);
    }

    void Update () {
		
	}
}
