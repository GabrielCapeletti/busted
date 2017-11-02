using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public EndScreen blackscreen;

    [SerializeField]
    private Transform slotContainer;

    [SerializeField]
    private GameObject characterModel;

    [SerializeField]
    private int totalOfCharacters;

    private List<Vector3> positionSlots;

    private void Start()
    {
        this.SaveSlots();
        this.SetupCharacters();
    }

    private void SetupCharacters()
    {
        this.totalOfCharacters = Mathf.Clamp(this.totalOfCharacters, 0, this.positionSlots.Count - 1);

        int index = 0;

        while (index < this.totalOfCharacters)
        {
            GameObject character = Instantiate(this.characterModel);
            character.name += "" + index;
            Spot spot = SpotManager.Instance.FindNextPosition();
            if(spot != null)
            {
                character.GetComponent<CharacterAI>().SetCurrentSpot(spot);
                character.SendMessage("MoveTo", spot.transform.position);
            }
            index++;
        }

    }

    private void SaveSlots()
    {
        this.positionSlots = new List<Vector3>();
        int index = 0;

        while (index < this.slotContainer.childCount)
        {
            this.positionSlots.Add(this.slotContainer.GetChild(index).position);
            index++;
        }
    }

    public void OpenEndScreen(List<GameObject> suspects, PoliceManBehavior police)
    {
        blackscreen.Open(suspects, police);
    }

}
