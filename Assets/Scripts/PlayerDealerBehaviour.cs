using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDealerBehaviour : PlayerBehavior {
    private CharacterAI closeChar;
    private BoxCollider2D collider;
    protected override void Awake() {
        base.Awake();
        collider = this.GetComponent<BoxCollider2D>();
    }

    protected override void OnWaitAnim() {
        base.OnWaitAnim();

        if (Input.GetButtonDown("Action"+this.player)) {
            this.TrySellDrug();
        }
    }

    protected void OnDrugging() {
        this.timer += Time.deltaTime;
        if (this.timer > 1f) {
            this.closeChar.PlayTalkAnim();
        }
        if (this.timer > this.stateTime) {
            this.closeChar.FinishListening();
            this.EndState();
        }
    }

    protected override void OnMove() {
        base.OnMove();
        if (Input.GetButtonDown("Action" + this.player)) {
            this.TrySellDrug();
        }
    }

    public void StartingMove() {
        this.timer = 0;
        this.stateUpdate = this.OnMoveEnter;
    }


    public void JudgeMode() {
        this.animator.Play("handsUp");
        this.spriteRenderer.sortingLayerName = "Front";

        this.stateUpdate = null;
    }

    private void TrySellDrug() {
        int layer = 1 << LayerMask.NameToLayer("Suspects");
        Collider2D coll = Physics2D.OverlapCircle(this.transform.position +new Vector3(0,1.68f,0),1.2f,layer);

        if (coll != null && coll.name != this.name) {
            Debug.Log("Try selling drug");

            this.closeChar = coll.GetComponent<CharacterAI>();
            this.spriteRenderer.flipX = this.closeChar.transform.position.x < this.transform.position.x;
            this.closeChar.spriteRenderer.flipX = !this.spriteRenderer.flipX;

            if (!this.closeChar.drugged) {
                this.animator.Play("deal");
                this.closeChar.OnDruggedEnter();
                this.stateTime = 2.34f;
                this.stateUpdate = this.OnDrugging;
            } 
        }
    }
}
