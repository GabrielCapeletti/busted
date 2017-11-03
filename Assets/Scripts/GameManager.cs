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
    public GameObject scoreScreen;

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
    private GameObject dealer;
    private bool gameBegan = false;
    private bool hasPlayer2 = false;
    public bool gamePaused { get; set; }

    private void Start() {
        this.gamePaused = false;
        this.SaveSlots();
        this.SetupCharacters();

        this.logoTween = PotaTween.Create(this.logo);
        this.logoTween.SetAlpha(1f, 0f);
        this.logoTween.SetDuration(2f);

        this.postProcessing.depthOfField.enabled = true;

    }

    private void SetupCharacters()
    {
        this.characters = new List<GameObject>();

        this.totalOfCharacters = Mathf.Clamp(this.totalOfCharacters, 0, this.positionSlots.Count - 1);

        int index = 0;
        int dealerIndex = Random.Range(0, this.characterModel.Length);

        while (index < this.totalOfCharacters) {
            GameObject character = Instantiate(this.characterModel[index%this.characterModel.Length]);
            character.name += "" + index;
            Spot spot = SpotManager.Instance.FindNextPosition();

            if (spot != null) {    
                character.SendMessage("MoveTo", spot.transform.position);
            }

            character.transform.parent = transform;

            characters.Add(character);

            index++;
        }

    }

    private void SetPolice()
    {
        GameObject character = this.characters[0];

        CharacterAI ai = character.GetComponent<CharacterAI>();
        float scaleDiference = ai.GetScaleDifference();
        ai.enabled = false;
        //GameObject.Destroy(character.GetComponent<CharacterAI>());
        Destroy(character.GetComponent<BoxCollider2D>());
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
                this.dealer = ia.gameObject;
            }

            index++;
        }
            
    }

    private void EnterPlayer2() {
        CharacterAI ai = this.dealer.GetComponent<CharacterAI>();

        float scaleDiference = ai.GetScaleDifference();

        Destroy(ai);
        PlayerDealerBehaviour playerDealer = this.dealer.AddComponent<PlayerDealerBehaviour>();
        playerDealer.SetScaleDifference(scaleDiference);
        playerDealer.SetInitialScale(0.5f);
        playerDealer.player = 2;
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
        this.blackscreen.Open(suspects, police);
    }

    void Update() {
        if (this.gamePaused) {
            if (Input.GetButtonDown("Cancel1")) {
                this.blackscreen.Close();
                this.gamePaused = false;
            }
        }

        if (gameBegan && Input.GetButtonDown("Start2")) {
            Debug.Log("Start2");
            if (!this.hasPlayer2) {
                this.EnterPlayer2();
                this.hasPlayer2 = true;
            }
        }
        //if (Input.GetButtonDown("Action1")) {
        //    Debug.Log("Action1");
        //}
        //if (Input.GetButtonDown("Action2")) {
        //    Debug.Log("Action2");
        //}
        //if (Input.GetButtonDown("Cancel1")) {
        //    Debug.Log("Cancel1");
        //}
        //if (Input.GetButtonDown("Cancel2")) {
        //    Debug.Log("Cancel2");
        //}
        //if (!this.hasPlayer2) {
        //        this.EnterPlayer2();
        //        this.hasPlayer2 = true;
        //    }

        if (!this.logoTween.IsPlaying && this.logo.activeInHierarchy && Input.anyKeyDown)
        {
            this.logoTween.Play(this.StartGame);
        }
    }

    public List<GameObject> GetCharacters()
    {
        return characters;
    }

    private void StartGame()
    {
        this.logo.SetActive(false);
        this.SetDealer();
        this.SetPolice();
        this.postProcessing.depthOfField.enabled = false;
        gameBegan = true;
        this.tutorial.SetActive(true);
    }
}
