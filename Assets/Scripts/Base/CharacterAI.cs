using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : BaseCharacter {

    private const float TOLERANCE = 0.1f;

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

    protected override void Start() {
        base.Start();
        this.stateUpdate = this.OnMoveEnter;
    }

    public void SetCurrentSpot(Spot spot) {
        this.currentSpot = spot;
        spot.IsFree = false;
    }

    #region IDLE
    private void OnIdleEnter() {
        //pLAY ANIMATION
        //Debug.Log("Idle");
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
        this.animator.Play("jump");
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
        //pLAY ANIMATION
        //Debug.Log("Talk");
        this.animator.Play("idle");
        this.stateTime = 1;
        this.stateUpdate = this.OnTalk;
    }

    private void OnTalk() {
        this.timer += Time.deltaTime;
        if (this.timer > this.stateTime) {
            this.EndState();
        }
    }
    #endregion

    #region MOVE
    private void OnMoveEnter() {
        Debug.Log("OnMoveEnter");

        this.oldSpot = this.currentSpot;
        this.currentSpot = SpotManager.Instance.FindNextPosition();
        
        if (this.currentSpot == null) {
            this.EndState();
            this.currentSpot = this.oldSpot;
            return;
        }

        if (this.oldSpot != null)
            this.oldSpot.IsFree = true;

        this.currentSpot.IsFree = false;
        this.animator.Play("run");
        this.targetPosition = this.currentSpot.transform.position;
        this.spriteRenderer.flipX = this.targetPosition.x > this.transform.position.x;
        this.startPosition = this.transform.position;
        this.duration = Vector2.Distance(this.targetPosition, this.transform.position)/ this.speed;
        this.stateUpdate = this.OnMove;
    }

    private void OnMove() {
        //Debug.Log("OnMove");
        
        //float newPosX = Ease.Linear(this.cumulativeTime, this.startPosition.x, this.targetPosition.x, this.duration);
        //float newPosY = Ease.Linear(this.cumulativeTime,this.startPosition.y,this.targetPosition.y,this.duration);
        this.cumulativeTime += Time.deltaTime;

        //this.transform.position = new Vector2(newPosX,newPosY);
        this.MoveTo(Vector3.Lerp(this.startPosition,this.targetPosition,this.cumulativeTime * this.speed/this.duration));

        if (Vector2.Distance(this.targetPosition, this.transform.position) <= TOLERANCE) {
            this.EndState();
        }
    }
    #endregion

    private void EndState() {
        this.timer = 0;
        this.cumulativeTime = 0;
        float rnd = UnityEngine.Random.Range(0f, 1f);
        if (rnd < 0.6f) {
            this.stateUpdate = this.OnMoveEnter;
        } 
        else 
        {
            rnd = UnityEngine.Random.Range(0f,1f);

            if (this.currentSpot != null && this.currentSpot.GroupId >= 0 && SpotManager.Instance.hasSomeoneOnGroup(this.currentSpot)) {
                this.ChooseActionOnNonEmptyGroup();
            } else 
            {
                if (rnd < 0.51f) {
                    this.stateUpdate = this.OnDanceEnter;
                }
                else {
                    this.stateUpdate = this.OnIdleEnter;
                }
            }
        }
    }

    private void ChooseActionOnNonEmptyGroup() {
        float rnd = UnityEngine.Random.Range(0f, 1f);
        if (rnd < 0.5f) {
            this.stateUpdate = this.OnTalkEnter;
        } else if (rnd < 0.85f) {
            this.stateUpdate = this.OnDanceEnter;
        } else {
            this.stateUpdate = this.OnIdleEnter;
        }
    }

    

    protected override void Update() {
        this.stateUpdate.Invoke();
    }



}
