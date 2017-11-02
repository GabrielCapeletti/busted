using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : BaseCharacter {

    [SerializeField]
    private float speed = 2;

    private Vector3 direction;
    private GameManager manager;
    private Action stateUpdate;
    private float timer;
    private double stateTime;

    protected override void Start()
    {
        base.Start();
        this.manager = GameManager.Instance;
        this.stateUpdate = this.OnIdleEnter;
        this.MoveTo(this.transform.position);
    }

    private void OnWaitAnim() {
        if (direction.magnitude > 0) {
            this.stateUpdate = this.OnMoveEnter;
            return;
        }

        this.timer += Time.deltaTime;
        if (this.timer > this.stateTime) {
            this.EndState();
        }
    }

    private void OnIdleEnter() {
        this.animator.Play("idle");
        this.stateUpdate = this.OnWaitAnim;
    }

    private void OnDanceEnter() {
        this.animator.Play("dance");
        this.stateTime = 1;
        this.stateUpdate = this.OnWaitAnim;
    }

    private void OnTalkEnter() {
        this.animator.Play("idle");
        this.stateTime = 1;
        this.stateUpdate = this.OnWaitAnim;
    }
    
    #region MOVE
    private void OnMoveEnter() {
        this.animator.Play("run");
        this.stateUpdate = this.OnMove;
    }

    private void OnMove() {
        if (direction.magnitude <= 0) {
            this.stateUpdate = this.EndState;
            return;
        }

        Vector3 nextPos = this.transform.position + (direction * this.speed * Time.deltaTime);
        this.spriteRenderer.flipX = nextPos.x < this.transform.position.x;

        int layerMask = 1 << LayerMask.NameToLayer("Colliders");

        if (Physics2D.OverlapCircle(nextPos,0.1f, layerMask) == null) {
            this.MoveTo(nextPos);
        }
    }
    #endregion

    private void EndState() {
        this.timer = 0;
        float rnd = UnityEngine.Random.Range(0f, 1f);
        if (rnd < 0.51f) {
            this.stateUpdate = this.OnDanceEnter;
        } else {
            this.stateUpdate = this.OnIdleEnter;
        }
    }

    protected override void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        this.stateUpdate.Invoke();
    }

}
