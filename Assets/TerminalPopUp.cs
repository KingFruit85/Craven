using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalPopUp : MonoBehaviour
{
    [SerializeField] Button _button1;
    [SerializeField] Button _button2;
    [SerializeField] Button _button3;
    [SerializeField] Text _button1Text;
    [SerializeField] Text _button2Text;
    [SerializeField] Text _button3Text;
    
    // I want this to spawn a terminal on screen that you can navigate through learning backstory and 
    // unlocking linked 

    public void PopUpTerminal(GameObject loadSource, Transform canvas, string btn1txt, string btn2txt, string btn3txt,Action action1, Action action2)
    {
        _button1Text.text = btn1txt;
        _button2Text.text = btn2txt;
        _button3Text.text = btn3txt;


        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        _button1.onClick.AddListener(() => {
            action1();
            loadSource.GetComponent<Computer>().inUse = false;
            GameObject.Destroy(this.gameObject);
        });

        _button2.onClick.AddListener(() => {
            action2();
            loadSource.GetComponent<Computer>().inUse = false;
            GameObject.Destroy(this.gameObject);
        });


        // CLose Terminal
        _button3.onClick.AddListener(() => {
            loadSource.GetComponent<Computer>().inUse = false;
            GameObject.Destroy(this.gameObject);
        });

    }
}
