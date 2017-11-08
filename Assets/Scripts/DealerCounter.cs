using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerCounter : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public Transform mask;
    public Material spriteMaterial;

    private List<SpriteRenderer> junks;
    private int totalOfJunks = 0;

    private int currentJunk = 0;

    void Awake () {
        float percentX = 1f - (float)(GameManager.CROWDED - GameManager.PEOPLE_ON_PARTY) / (float)(GameManager.CROWDED - GameManager.MIN_PEOPLE);
        
        mask.localScale = new Vector3(0.6f + (0.4f * percentX), 1, 1);

        totalOfJunks = GameManager.PEOPLE_ON_PARTY / 3;
        
        junks = new List<SpriteRenderer>();

        for (int i = 0; i < totalOfJunks; i++)
        {
            GameObject junk = new GameObject();
            junk.transform.parent = transform;

            SpriteRenderer spriteRenderer = junk.AddComponent<SpriteRenderer>();
            spriteRenderer.material = spriteMaterial;
            junks.Add(spriteRenderer);

        }

    }
	
    public void AddJunk(Sprite sprite)
    {
        if(currentJunk < junks.Count)
        {
            junks[currentJunk].sprite = sprite;

            float percentX = 1f - (float)(GameManager.CROWDED - GameManager.PEOPLE_ON_PARTY) / (float)(GameManager.CROWDED - GameManager.MIN_PEOPLE);

            float distanceX = spriteRenderer.bounds.size.x * (0.6f + (0.3f * percentX));

            float posX = (distanceX * -0.5f) + (currentJunk * ((float)distanceX / ((float)totalOfJunks - 1)));

            junks[currentJunk].transform.localScale = Vector3.one * 0.8f;

            Vector3 newPosition = junks[currentJunk].transform.localPosition;
            newPosition.x = 0.2f + posX;
            newPosition.z = -1;
            newPosition.y = -3f;

            junks[currentJunk].transform.localPosition = newPosition;

            PotaTween entryTween = PotaTween.Create(junks[currentJunk].gameObject);
            entryTween.SetPosition(TweenAxis.Y, newPosition.y - 5, newPosition.y, true);
            entryTween.SetEaseEquation(Ease.Equation.OutBack);

            entryTween.Play();

            currentJunk++;
        }
    }

}
