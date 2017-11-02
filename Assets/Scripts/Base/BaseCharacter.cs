using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour {

    [Range(0,3)]
    [SerializeField]
    private float scaleDifference;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected PolygonCollider2D walkArea;
    private float maxHeight;
    private float initialScale;

    protected virtual void Start() {
	    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
	    this.animator = this.GetComponent<Animator>();
        this.walkArea = GameManager.Instance.WalkAreaCollider;
        this.maxHeight = (this.walkArea.bounds.max.y - this.walkArea.bounds.min.y);
        this.initialScale = this.transform.localScale.x;
        //new Vector2(bounds.min.x + (xPox * (bounds.max.x - bounds.min.x)),);
    }

    protected void MoveTo(Vector3 position) {
        float height = this.walkArea.bounds.max.y - position.y;
        float scale = height / this.maxHeight;
        this.spriteRenderer.sortingOrder = (int)(scale * 100);

        scale *= this.scaleDifference;
        scale += this.initialScale;
        
        this.transform.localScale = new Vector2(scale,scale);

        this.transform.position = position;

    }

    protected virtual void Update () {
		//
	}
}
