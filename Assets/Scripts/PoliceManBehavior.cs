using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManBehavior : PlayerBehavior {

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

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f, layerMask);

        for(int i = 0; i < colliders.Length; i++)
        {

            colliders[i].SendMessage("StopOnIdle");
        }

        if (colliders.Length > 0)
        {
            stateUpdate = ChooseSuspect;
        }
    }

    private void ChooseSuspect()
    {

    }

}
