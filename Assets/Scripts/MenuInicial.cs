using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInicial : MonoBehaviour {

    public List<Transform> points;

    public GameObject arrow;
    public GameObject credits;

    public GameObject logo;

    private int selectedIndex = 0;
    private bool changeArrow = false;

    private bool turnOffMenu = false;


    void Start () {
        selectedIndex = 1;
    }

    public void Update() {

        if (this.turnOffMenu) return;

        float axis = Input.GetAxisRaw("Horizontal1");
        axis += Input.GetAxisRaw("HorizontalTeclado");

        if (Mathf.Abs(axis) > 0 && !this.changeArrow) {
            this.selectedIndex = (axis > 0) ? (this.selectedIndex + 1) : (this.selectedIndex - 1);
            this.selectedIndex = (this.selectedIndex >= 0) ? this.selectedIndex : this.points.Count - 1;
            this.selectedIndex = this.selectedIndex % this.points.Count;
            this.arrow.transform.position = new Vector3(this.points[this.selectedIndex].transform.position.x,this.arrow.transform.position.y,this.arrow.transform.position.z);
            this.changeArrow = true;

            credits.SetActive(selectedIndex == this.points.Count - 1);
            logo.SetActive(!(selectedIndex == this.points.Count - 1));

        } else if (Math.Abs(axis) == 0) {
            this.changeArrow = false;
        }

        if (Input.GetButtonDown("Action1")) {
            switch (this.selectedIndex) {
                case 0:
                    GameManager.PEOPLE_ON_PARTY = 11;
                    GameManager.Instance.StartGame();
                    this.turnOffMenu = true;
                    break;
                case 1:
                    GameManager.PEOPLE_ON_PARTY = 15;
                    GameManager.Instance.StartGame();
                    this.turnOffMenu = true;
                    break;
                case 2:
                    GameManager.PEOPLE_ON_PARTY = 21;
                    GameManager.Instance.StartGame();
                    this.turnOffMenu = true;
                    break;
                case 3:
                    GameManager.PEOPLE_ON_PARTY = 30;
                    GameManager.Instance.StartGame();
                    this.turnOffMenu = true;
                    break;
            }

        }
    }
}
