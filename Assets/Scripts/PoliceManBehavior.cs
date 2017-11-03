using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManBehavior : PlayerBehavior {

    private BoxCollider2D boxCollider;
    public bool stopWalking = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!this.stopWalking)
        {
            base.Update();
        }

        if (Input.GetButtonDown("Action" + this.player))
        {
            this.stateUpdate = this.OnTalkEnter;
        }

    }

    protected override void OnTalkEnter()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Suspects");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.spriteRenderer.bounds.center, 0.8f, layerMask);

        int flipDir = 1;
        if (this.spriteRenderer.flipX)
        {
            flipDir = -1;
        }

        if (colliders.Length > 0)
        {
            this.stopWalking = true;
            int maxSuspects = 3;
            this.spriteRenderer.sortingLayerName = "Front";

            List<GameObject> suspects = new List<GameObject>();

            for (int i = 0; i < Mathf.Clamp(colliders.Length, 0, maxSuspects); i++)
            {
                if (colliders[i].name == this.name) continue;

                colliders[i].SendMessage("JudgeMode");
                suspects.Add(colliders[i].gameObject);
            }

            this.animator.Play("busted");
            this.stateUpdate = this.ChooseSuspect;

            GameManager.Instance.OpenEndScreen(suspects, this);

        }
        else
        {
            this.stateUpdate = this.OnIdleEnter;
        }
    }

    private void ChooseSuspect()
    {

    }

}
