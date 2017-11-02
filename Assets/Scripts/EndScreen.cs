﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

    public Transform dectectivePointLeft;
    public Transform dectectivePointRight;

    public Transform shadow;
    public Transform arrow;

    private int currentSuspect = 0;
    private bool changeArrow = false;

    private List<GameObject> suspects;

    void Start () {
		
	}

    public void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(axis) > 0 && !changeArrow)
        {

            changeArrow = true;
            return;
        }

        changeArrow = false;
    }

    public void Open(List<GameObject> suspects, PoliceManBehavior police)
    {
        this.suspects = suspects;


        gameObject.SetActive(true);

        float distanceX = 2.2f;
        
        shadow.localScale = new Vector3(0.3f, 0.1f) * (suspects.Count * 0.6F);

        for (int i = 0; i < suspects.Count; i++)
        {
            float posX = 1f + (distanceX * i) - ((suspects.Count * distanceX) * 0.5f);

            PotaTween policeTween = PotaTween.Create(police.gameObject);
            policeTween.SetPosition(police.transform.position, dectectivePointRight.position);

            if (!police.GetComponent<SpriteRenderer>().flipX)
            {
                policeTween.SetPosition(police.transform.position, dectectivePointLeft.position);
            }

            policeTween.SetDuration(0.5f);

            PositionArrow(posX);
            arrow.gameObject.SetActive(true);

            PotaTween tween = PotaTween.Create(suspects[i].gameObject);
            tween.SetPosition(suspects[i].transform.position, new Vector3(posX, -2));
            tween.SetScale(suspects[i].transform.localScale, Vector3.one * 0.8f);
            tween.SetDuration(0.5f);

            tween.Play();
            policeTween.Play();
        }

    }

    private void PositionArrow(float posX)
    {
        arrow.transform.position = new Vector3(posX, 2.2f, 0);
    }
}