using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScreen : MonoBehaviour {

    public static string END_TEXT = "";
    public TextMesh text;
    public Transform score;

	void Start () {
		
	}

    public void Open()
    {
        gameObject.SetActive(true);
        text.text = END_TEXT;

        List<GameObject> characters = GameManager.Instance.GetCharacters();

        int totalLine = 15;

        for (int i=0; i < characters.Count; i++)
        {
            GameObject characterIcon = new GameObject();
            characterIcon.transform.parent = transform;

            SpriteRenderer spriteRenderer = characterIcon.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = characters[i].GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.sortingLayerName = "Front";

            PotaTween tweenAlpha = PotaTween.Create(characterIcon);
            tweenAlpha.SetAlpha(0f, 1f);
            tweenAlpha.SetDuration(0.3f);
            tweenAlpha.SetDelay(0.1f * i);

            characterIcon.transform.localScale = characterIcon.transform.localScale * 0.5f;
            Vector3 newPos = Vector3.right * (i % totalLine);
            newPos.y = (Vector3.down * (int)(i / totalLine) * 2f).y;
            newPos.z = (Vector3.back * (int)(i / totalLine)).z;

            characterIcon.transform.position = score.position + newPos;

            tweenAlpha.Play();
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Action1"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
