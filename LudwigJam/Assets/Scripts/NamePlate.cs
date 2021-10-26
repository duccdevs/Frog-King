using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NamePlate : MonoBehaviour
{
    public string[] Names;
    public TextMeshPro text;

    public void ShowName(int i)
    {
        text.text = Names[i];
        GetComponent<Animator>().SetTrigger("Text");
    }
}
