using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManBehavior : PlayerBehavior {

    private BoxCollider2D boxCollider;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Action"))
        {
            stateUpdate = OnTalkEnter;
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
            spriteRenderer.sortingLayerName = "Front";

            List<GameObject> suspects = new List<GameObject>();

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].SendMessage("JudgeMode");
                suspects.Add(colliders[i].gameObject);
            }

            animator.Play("busted");
            stateUpdate = ChooseSuspect;

            GameManager.Instance.OpenEndScreen(suspects);

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
