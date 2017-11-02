using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : BaseCharacter {

    private const float TOLERANCE = 1f;

    [SerializeField]
    private float speed;

    private float timer;
    private float stateTime = 1;
    private float duration = 0;
    private float cumulativeTime = 0;

    private Action stateUpdate;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    
    protected override void Start() {
        base.Start();
        this.stateUpdate = this.OnIdle;
    }

    #region IDLE
    private void OnIdleEnter() {
        //pLAY ANIMATION
        Debug.Log("Idle");
        this.animator.Play("idle");
        this.timer = 0;
        this.stateUpdate = this.OnIdle;
    }

    private void OnIdle() {
        this.timer += Time.deltaTime;
        if (this.timer > this.stateTime) {
            this.EndState();
        }
    }
    #endregion

    #region MOVE
    private void OnMoveEnter() {
        Debug.Log("OnMoveEnter");

        Spot nextSpot = SpotManager.Instance.FindNextPosition();
        if (nextSpot == null) {
            this.EndState();
            return;
        }

        this.animator.Play("run");
        this.targetPosition = nextSpot.transform.position;
        this.spriteRenderer.flipX = this.targetPosition.x > this.transform.position.x;
        this.startPosition = this.transform.position;
        this.duration = Vector2.Distance(this.targetPosition, this.transform.position)/ this.speed;
        this.stateUpdate = this.OnMove;
    }

    private void OnMove() {
        Debug.Log("OnMove");
        
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
        Debug.Log("End State");

        this.cumulativeTime = 0;
        float rnd = UnityEngine.Random.Range(0f, 1f);
        if (rnd < 0.5f) {
            this.stateUpdate = this.OnMoveEnter;
        } else {
            this.stateUpdate = this.OnIdleEnter;
        }
    }

    protected override void Update() {
        this.stateUpdate.Invoke();
    }



}
