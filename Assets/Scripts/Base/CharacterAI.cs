using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : BaseCharacter {

    private const float TOLERANCE = 0.1f;

    [SerializeField]
    private bool dealer;

    [SerializeField]
    private float speed;

    private float timer;
    private float stateTime = 1;
    private float duration = 0;
    private float cumulativeTime = 0;
    private Action stateUpdate;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Spot currentSpot;
    private Spot oldSpot;
    public bool drugged = false;
    private CharacterAI closeChar;

    protected override void Start() {
        base.Start();
        this.stateUpdate = this.OnMoveEnter;
    }

    public bool IsDealer() {
        return this.dealer;
    }

    public void BecomeDealer() {
        this.dealer = true;
    }

    public void SetCurrentSpot(Spot spot) {
        this.currentSpot = spot;
        spot.Occupy(this);
    }

    #region IDLE
    private void OnIdleEnter() {
        this.animator.Play("idle");
        this.stateTime = 1;
        this.stateUpdate = this.OnIdle;
    }

    private void OnIdle() {
        this.timer += Time.deltaTime;
        if (this.timer > this.stateTime) {
            this.EndState();
        }
    }
    #endregion

    #region DANCING
    private void OnDanceEnter() {
        //pLAY ANIMATION
        //Debug.Log("Dance");
        this.animator.Play("dance");
        this.stateTime = 2;
        this.stateUpdate = this.OnDance;
    }

    private void OnDance() {
        this.timer += Time.deltaTime;
        if (this.timer > this.stateTime) {
            this.EndState();
        }
    }
    #endregion
    
    #region TALK
    private void OnTalkEnter() {
        this.spriteRenderer.flipX = this.closeChar.transform.position.x < this.transform.position.x;
        this.closeChar.spriteRenderer.flipX = !this.spriteRenderer.flipX;

        if (this.dealer && !this.closeChar.drugged) {
            this.closeChar.OnDruggedEnter();
            this.closeChar.PlayTalkAnim();
            this.animator.Play("deal");
            this.stateTime = 2.34f;
            this.stateUpdate = this.OnTalk;
        } else {
            Debug.Log("Talk");
            this.closeChar.GoListening();
            this.animator.Play("talk");
            this.stateTime = 2.15f;
            this.stateUpdate = this.OnTalk;
        }
    }

    private void OnTalk() {
        this.timer += Time.deltaTime;
        if (this.timer > this.stateTime) {
            this.closeChar.FinishListening();
            this.EndState();
        }
    }
    #endregion

    #region MOVE
    private void OnMoveEnter() {
        //Debug.Log("OnMoveEnter");

        this.oldSpot = this.currentSpot;
        this.currentSpot = SpotManager.Instance.FindNextPosition();
        
        if (this.currentSpot == null) {
            this.EndState();
            this.currentSpot = this.oldSpot;
            return;
        }

        if (this.oldSpot != null)
            this.oldSpot.IsFree = true;

        this.currentSpot.Occupy(this);
        this.animator.Play("run");
        this.targetPosition = this.currentSpot.transform.position;
        this.spriteRenderer.flipX = this.targetPosition.x < this.transform.position.x;
        this.startPosition = this.transform.position;
        this.duration = Vector2.Distance(this.targetPosition, this.transform.position)/ this.speed;
        this.stateUpdate = this.OnMove;
    }

    private void OnMove() {
       
        this.cumulativeTime += Time.deltaTime;

        this.MoveTo(Vector3.Lerp(this.startPosition,this.targetPosition,this.cumulativeTime * this.speed/this.duration));

        if (Vector2.Distance(this.targetPosition, this.transform.position) <= TOLERANCE) {
            if (!this.drugged) {
                this.EndState();
            }
        }
    }
    #endregion

    public void StartingMove() {
        this.timer = 0;
        this.cumulativeTime = 0;

        this.stateUpdate = this.OnMoveEnter;
    }

    public void EndState() {
        if(this.drugged)
            return;

        this.timer = 0;
        this.cumulativeTime = 0;
        float rnd = UnityEngine.Random.Range(0f, 1f);

        if (this.dealer) {
            rnd += 0.4f;
        }

        if (rnd < 0.6f) {
            this.stateUpdate = this.OnMoveEnter;
        } 
        else 
        {
            rnd = UnityEngine.Random.Range(0f,1f);
            this.closeChar = null;
            
            int layer = 1 << LayerMask.NameToLayer("Suspects");
            Collider2D coll = Physics2D.OverlapCircle(this.transform.position,0.2f,layer);

            if(coll != null)
                this.closeChar = coll.GetComponent<CharacterAI>();

            if (this.currentSpot != null && this.closeChar != null) {
                this.ChooseActionOnNonEmptyGroup();
            } else {
                if (rnd < 0.51f) {
                    this.stateUpdate = this.OnDanceEnter;
                }
                else {
                    this.stateUpdate = this.OnIdleEnter;
                }
            }
        }
    }

    public void PlayTalkAnim() {
        this.animator.Play("talk");
    }

    public void OnDruggedEnter() {
        this.drugged = true;
        this.stateUpdate = null;
        Collider2D coll = this.GetComponent<Collider2D>();
        if(coll != null)
            Destroy(coll);        
    }

    public void GoListening() {
        if (this.drugged) {
            return;
        }

        this.animator.Play("talk");
        this.stateUpdate = null;
    }

    public void FinishListening() {
        if (this.drugged) {
            this.animator.Play("drugged");
            Destroy(this);
            return;
        }

        this.EndState();
    }

    private void ChooseActionOnNonEmptyGroup() {
        float rnd = UnityEngine.Random.Range(0f, 1f);

        if (this.dealer) {
            rnd -= 0.4f;
        }

        if (rnd < 0.5f) {
            this.stateUpdate = this.OnTalkEnter;
        } else if (rnd < 0.85f) {
            this.stateUpdate = this.OnDanceEnter;
        } else {
            this.stateUpdate = this.OnIdleEnter;
        }
    }

    protected override void Update() {
        if(GameManager.Instance.gamePaused)
            return;

        if(this.stateUpdate != null)
            this.stateUpdate.Invoke();
    }

    public void JudgeMode()
    {
        this.animator.Play("handsUp");
        this.spriteRenderer.sortingLayerName = "Front";

        this.stateUpdate = null;
    }
}
