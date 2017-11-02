using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

    public Transform dectectivePoint;

	void Start () {
		
	}

    public void Open(List<GameObject> suspects)
    {
        gameObject.SetActive(true);

        float distanceX = 2;

        for(int i = 0; i < suspects.Count; i++)
        {
            float posX = (distanceX * i) - ((suspects.Count * distanceX) * 0.5f);

            PotaTween tween = PotaTween.Create(suspects[i].gameObject);
            tween.SetPosition(suspects[i].transform.position, new Vector3(posX, -2));
            tween.SetDuration(0.5f);

            tween.Play();
        }

        
    }
}
