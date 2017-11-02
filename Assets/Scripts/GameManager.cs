using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region SINGLETON PATTERN
    public static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null) {
                    GameObject container = new GameObject();
                    _instance = container.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private PolygonCollider2D walkAreaCollider;
    public PolygonCollider2D WalkAreaCollider {
        get { return this.walkAreaCollider; }
        set { this.walkAreaCollider = value; }
    }




}
