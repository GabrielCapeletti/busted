using UnityEngine;
using System.Collections;
using System.Text;
using SimpleJSON;
using UnityEngine.UI;
using System.Collections.Generic;

public static class StringTable
{
    private static JSONNode strTable = new JSONNode();
    private static TextAsset table = new TextAsset();
    private static StringBuilder strBuilder;

    private static Dictionary<SystemLanguage, string> languages;

    public static string BASE_FOLDER = "Localization/";

    public static SystemLanguage deviceLanguage = SystemLanguage.English;
    public static bool inited = false;

    public static void Initialize()
    {
        inited = true;

        languages = new Dictionary<SystemLanguage, string>();
        languages.Add(SystemLanguage.Portuguese, "pt_br");
        languages.Add(SystemLanguage.English, "en_us");
        languages.Add(SystemLanguage.Spanish, "es_ar");
        languages.Add(SystemLanguage.Russian, "rus_ru");

    }

    public static void ReplaceTexts()
    {
        Text[] labels = (Text[])GameObject.FindObjectsOfType<Text>();

        foreach (Text lbl in labels)
        {
            if (lbl.name.IndexOf("dyn_") == 0)
            {
                lbl.text = GetText(lbl.name.Replace("dyn_", ""));
            }
        }
    }

    public static void ReplaceTextsInChildren(GameObject gameObject)
    {
        Text[] labels = (Text[])gameObject.GetComponentsInChildren<Text>();

        foreach (Text lbl in labels)
        {
            if (lbl.name.IndexOf("dyn_") == 0)
            {
                lbl.text = GetText(lbl.name.Replace("dyn_", ""));
            }
        }
    }

    public static void ReplaceTextInObject(GameObject gameObject)
    {
        Text lbl = gameObject.GetComponent<Text>();
        if (lbl)
        {
            lbl.text = GetText(lbl.name.Replace("dyn_", ""));
            return;
        }

        TextMesh mesh = gameObject.GetComponent<TextMesh>();

        if (mesh)
        {

            if (StringTable.deviceLanguage == SystemLanguage.Russian)
            {
                Font russianFont = Resources.Load("DejaVuSerifCondensed") as Font;
                mesh.font = russianFont;
                MeshRenderer rend = mesh.GetComponentInChildren<MeshRenderer>();
                rend.material = russianFont.material;
            }

            mesh.text = GetText(mesh.name.Replace("dyn_", ""));
        }
    }

    public static void LoadLanguage()
    {
        if (!inited)
        {
            Initialize();
        }

        table = ((TextAsset)Resources.Load(BASE_FOLDER + languages[deviceLanguage]));
        SetupLanguageTable();
    }

    public static void SetLanguage(SystemLanguage language = SystemLanguage.Unknown, bool loadLanguage = false)
    {
        if (!inited)
        {
            Initialize();
        }

        if (languages.ContainsKey(language))
        {
            deviceLanguage = language;
        }

        //PlayerStatus.SetLanguage(deviceLanguage);
        if (loadLanguage)
        {
            LoadLanguage();
        }
    }

    private static void SetupLanguageTable()
    {
        string jsonText = table.text;//.Replace("\\n", "\n");

        strTable = JSONNode.Parse(jsonText);

        foreach (string key in strTable.Keys)
        {
            string fieldName = key;
            JSONNode obj = strTable[fieldName];
            strTable[fieldName] = obj;
        }
    }

    public static string GetText(string textId)
    {
        if (strTable[textId] != null)
        {
            string ret = strTable[textId];
            return ret;
        }
        return "NOT FOUND";
    }
}
