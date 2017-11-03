using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.PostProcessing;

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
    public PostProcessingProfile postProcessing;
    public GameObject tutorial;

    [SerializeField]
    private GameObject logo;

    [SerializeField]
    private Transform slotContainer;

    [SerializeField]
    private GameObject[] characterModel;

    [SerializeField]
    private int totalOfCharacters;

    private List<Vector3> positionSlots;
    private List<GameObject> characters;

    private PotaTween logoTween;

    public bool gamePaused { get; set; }

    private void Start() {
        this.gamePaused = false;
        this.SaveSlots();
        this.SetupCharacters();

        logoTween = PotaTween.Create(logo);
        logoTween.SetAlpha(1f, 0f);
        logoTween.SetDuration(2f);

        postProcessing.depthOfField.enabled = true;

    }

    private void SetupCharacters()
    {
        characters = new List<GameObject>();

        this.totalOfCharacters = Mathf.Clamp(this.totalOfCharacters, 0, this.positionSlots.Count - 1);

        int index = 0;
        int dealerIndex = Random.Range(0, this.characterModel.Length);

        while (index < this.totalOfCharacters) {

            int rand = Random.Range(0, this.characterModel.Length);
            GameObject character = Instantiate(this.characterModel[rand]);
            character.name += "" + index;
            Spot spot = SpotManager.Instance.FindNextPosition();

            if (spot != null) {    
                character.SendMessage("MoveTo", spot.transform.position);
            }

            characters.Add(character);

            index++;
        }

    }

    private void SetPolice()
    {
        GameObject character = characters[0];

        CharacterAI ai = character.GetComponent<CharacterAI>();
        float scaleDiference = ai.GetScaleDifference();
        GameObject.Destroy(character.GetComponent<CharacterAI>());
        PoliceManBehavior police = character.AddComponent<PoliceManBehavior>();
        police.SetScaleDifference(scaleDiference);
        police.SetInitialScale(0.5f);
        police.name = "Player";
    }

    private void SetDealer()
    {
        int index = 0;
        int dealerIndex = Random.Range(1, this.characters.Count - 1);

        while (index < this.totalOfCharacters)
        {
            GameObject character = this.characters[index];
            CharacterAI ia = character.GetComponent<CharacterAI>();

            if (dealerIndex == index)
            {
                ia.name = "Dealer";
                ia.BecomeDealer();
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

    public void OpenEndScreen(List<GameObject> suspects, PoliceManBehavior police) {
        this.gamePaused = true;
        blackscreen.Open(suspects, police);
    }

    void Update() {
        if (gamePaused) {
            if (Input.GetButtonDown("Cancel")) {
                this.blackscreen.Close();
                this.gamePaused = false;
            }
        }

        if (!logoTween.IsPlaying && logo.activeInHierarchy && Input.anyKeyDown)
        {
            logoTween.Play(StartGame);
        }
    }

    private void StartGame()
    {
        logo.SetActive(false);
        SetDealer();
        SetPolice();
        postProcessing.depthOfField.enabled = false;

        tutorial.SetActive(true);
    }
}
