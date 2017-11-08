using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour {

    public List<Toggle> itens;

    private bool canChangeToggle = true;
    private int currentItem = 0;

    private PotaTween outTween;

    void Start ()
    {
        outTween = PotaTween.Create(gameObject);
        outTween.SetPosition(TweenAxis.Y, 0, 30, false, false);
    }

	void Update ()
    {
        if (!NavMenu((int)Input.GetAxisRaw("VerticalTeclado")))
        {
            NavMenu((int)Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetButtonDown("Action1") && !outTween.IsPlaying)
        {
            outTween.Play(ChangeScene);
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }

    private bool NavMenu(int dirY)
    {

        if (Mathf.Abs(dirY) > 0 && canChangeToggle)
        {
            currentItem = (int)Mathf.Clamp(currentItem - dirY, 0, itens.Count - 1);
            itens[currentItem].Select();
            canChangeToggle = false;
            return true;

        }

        canChangeToggle = true;

        return false;
    }

    public void SetLanguage(int language)
    {

        StringTable.SetLanguage(SystemLanguage.English);
        if (language > 0)
        {
            StringTable.SetLanguage(SystemLanguage.Portuguese);
        }
        StringTable.LoadLanguage();
    }
}
