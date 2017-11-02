using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour {

    [Range(0,3)]
    [SerializeField]
    private float scaleDifference;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    private float maxHeight;
    private float initialScale;

    private Camera mainCamera;


    protected virtual void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.animator = this.GetComponent<Animator>();
        this.mainCamera = Camera.main;
        this.initialScale = this.transform.localScale.x;
        this.maxHeight = Camera.main.ScreenToWorldPoint(Vector3.up * Camera.main.pixelHeight).y;
    }

    protected virtual void Start() {
    }

    protected void MoveTo(Vector3 position) {
        float height = (this.mainCamera.transform.position.y + this.maxHeight * 0.5f) - position.y;
        float scale = height / this.maxHeight;

        this.spriteRenderer.sortingOrder = (int)(scale * 100);
        scale *= this.scaleDifference;
        scale += this.initialScale;
        
        this.transform.localScale = new Vector2(scale,scale);
        this.transform.position = position;
    }

    protected virtual void Update () {

	}
}
