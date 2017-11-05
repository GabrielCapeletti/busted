using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

    public Transform dectectivePointLeft;
    public Transform dectectivePointRight;

    public Transform shadow;
    public Transform arrow;
    public ScoreScreen scoreScreen;

    private int currentSuspect = 0;
    private bool changeArrow = false;

    private List<GameObject> suspects;
    private int selectedIndex = 0;
    private PotaTween alphaTweenIn;
    private PotaTween alphaTweenOut;
    private PoliceManBehavior police;

    void OnEnable() {
        this.alphaTweenIn.Play();
    }

    void Awake () {
        this.alphaTweenIn = PotaTween.Create(this.gameObject,0);
        this.alphaTweenIn.SetAlpha(0f, 1f);

        this.alphaTweenOut = PotaTween.Create(this.gameObject,1);
        this.alphaTweenOut.SetAlpha(1f,0f);
        this.alphaTweenOut.onComplete.AddListener(tween => ReactivateSuspects());
    }

    public void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal1");
        axis += Input.GetAxisRaw("HorizontalTeclado");
        if (Input.GetButtonDown("Action1")) {
            CharacterAI ia = this.suspects[this.selectedIndex].GetComponent<CharacterAI>();
            if (ia != null && ia.IsDealer()) {
                ScoreScreen.END_TEXT = "BOM TRABALHO! VOCÊ PRENDEU O CRIMINOSO.";
            } else if (ia == null)
            {
                ScoreScreen.END_TEXT = "BOM TRABALHO! VOCÊ PRENDEU O CRIMINOSO.";
            } else {

                ScoreScreen.END_TEXT = "VOCÊ PRENDEU A PESSOA ERRADA.";
            }

            transform.parent.gameObject.SetActive(false);
            scoreScreen.Open();
        }

        if (Mathf.Abs(axis) > 0 && !this.changeArrow) {
            this.selectedIndex = (axis > 0) ? (this.selectedIndex + 1) : (this.selectedIndex - 1);
            this.selectedIndex = (this.selectedIndex >= 0) ? this.selectedIndex : this.suspects.Count - 1;
            this.selectedIndex = this.selectedIndex % this.suspects.Count;
            this.arrow.transform.position = new Vector3(this.suspects[this.selectedIndex].transform.position.x,this.arrow.transform.position.y,this.arrow.transform.position.z);
            this.changeArrow = true;
            
        }else if (Math.Abs(axis) == 0) {
            this.changeArrow = false;
        }

    }

    private void ReallocateArrow() {
        this.arrow.transform.position = new Vector3(this.suspects[this.selectedIndex].transform.position.x,this.arrow.transform.position.y,this.arrow.transform.position.z);
    }

    private void ReactivateSuspects() {
        for (int i = 0 ; i < this.suspects.Count ; i++) {
            CharacterAI suspect = this.suspects[i].GetComponent<CharacterAI>();
            if (suspect != null) {
                suspect.spriteRenderer.sortingLayerName = "Default";
                suspect.StartingMove();
            }
            else {
                PlayerDealerBehaviour suspectPlayer = this.suspects[i].GetComponent<PlayerDealerBehaviour>();
                suspectPlayer.spriteRenderer.sortingLayerName = "Default";
                suspectPlayer.StartingMove();
            }
        }

        this.police.spriteRenderer.sortingLayerName = "Default";
        this.police.stopWalking = false;
        this.police.EndState();

        this.gameObject.SetActive(false);
    }

    public void Close() {
        this.alphaTweenOut.Play();
    }

    public void Open(List<GameObject> suspects, PoliceManBehavior police) {

        GameManager.Instance.tutorial.SetActive(false);

        this.police = police;
        this.suspects = suspects;
        this.selectedIndex = 0;
        this.gameObject.SetActive(true);

        float distanceX = 2.2f;

        this.shadow.localScale = new Vector3(0.3f, 0.1f) * (suspects.Count * 0.6F);

        for (int i = 0; i < suspects.Count; i++)
        {
            float posX = 1f + (distanceX * i) - ((suspects.Count * distanceX) * 0.5f);

            PotaTween policeTween = PotaTween.Create(police.gameObject);
            policeTween.SetPosition(police.transform.position, this.dectectivePointRight.position);

            if (!police.GetComponent<SpriteRenderer>().flipX)
            {
                policeTween.SetPosition(police.transform.position, this.dectectivePointLeft.position);
            }

            policeTween.SetDuration(0.5f);
            policeTween.onComplete.AddListener(arg0 => this.ReallocateArrow());
            this.PositionArrow(posX);
            this.arrow.gameObject.SetActive(true);

            PotaTween tween = PotaTween.Create(suspects[i].gameObject);
            tween.SetPosition(suspects[i].transform.position, new Vector3(posX, -2));
            tween.SetScale(suspects[i].transform.localScale, Vector3.one * 0.8f);
            tween.SetDuration(0.5f);
            tween.Play();
            policeTween.Play();
        }

    }

    private void PositionArrow(float posX)
    {
        this.arrow.transform.position = new Vector3(posX, 2.2f, 0);
    }
}
