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

        Collider2D[] colliders = Physics2D.OverlapCircleAll(spriteRenderer.bounds.center, 0.5f, layerMask);

        int flipDir = 1;
        if (spriteRenderer.flipX)
        {
            flipDir = -1;
        }

       // RaycastHit2D[] colliders = Physics2D.RaycastAll(spriteRenderer.bounds.center, Vector3.right * flipDir, 2,layerMask);

        //Collider2D[] colliders = Physics2D.OverlapBoxAll(spriteRenderer.bounds.center + (Vector3.right * flipDir), new Vector2(1f, 0.5f), 0, layerMask);
        
        if (colliders.Length > 0)
        {
            spriteRenderer.sortingLayerName = "Front";
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].SendMessage("JudgeMode");
            }

            animator.Play("busted");
            stateUpdate = ChooseSuspect;
            GameManager.Instance.blackscreen.SetActive(true);
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
