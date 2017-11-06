using UnityEngine;
using System.Collections;

public class StringReplacer : MonoBehaviour
{
    public void OnEnable()
    {
        StringTable.ReplaceTextInObject(this.gameObject);
    }

    void Start()
    {
        if (!StringTable.inited)
        {
            StringTable.LoadLanguage();
        }

        StringTable.ReplaceTextInObject(this.gameObject);
    }
}
