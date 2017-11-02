using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : BaseCharacter {

    [SerializeField]
    private float speed = 2;

    private Vector3 direction;
    private GameManager manager;

    protected override void Start()
    {
        manager = GameManager.Instance;
        base.Start();
    }

    protected override void Update()
    {
        Vector3 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (direction.magnitude > 0)
        {
            Vector3 nextPos = transform.position + (direction * speed * Time.deltaTime);

            if (Physics2D.OverlapCircle(nextPos, 0.1f) == null)
            {
                MoveTo(nextPos);
            }
        }
    }

}
