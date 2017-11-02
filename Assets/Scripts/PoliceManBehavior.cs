using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManBehavior : PlayerBehavior {

    private BoxCollider2D boxCollider;
    private bool stopWalking = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (stopWalking)
        {
            base.Update();
        }

        if (Input.GetButtonDown("Action"))
        {
            stateUpdate = OnTalkEnter;
            stopWalking = true;
        }

    }

    protected override void OnTalkEnter()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Suspects");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(spriteRenderer.bounds.center, 0.8f, layerMask);

        int flipDir = 1;
        if (spriteRenderer.flipX)
        {
            flipDir = -1;
        }

        if (colliders.Length > 0)
        {
            int maxSuspects = 3;
            spriteRenderer.sortingLayerName = "Front";

            List<GameObject> suspects = new List<GameObject>();

            for (int i = 0; i < Mathf.Clamp(colliders.Length, 0, maxSuspects); i++)
            {
                colliders[i].SendMessage("JudgeMode");
                suspects.Add(colliders[i].gameObject);
            }

            animator.Play("busted");
            stateUpdate = ChooseSuspect;

            GameManager.Instance.OpenEndScreen(suspects, this);

        }
        else
        {
            stateUpdate = OnIdleEnter;
        }
    }

    private void ChooseSuspect()
    {

    }

}
