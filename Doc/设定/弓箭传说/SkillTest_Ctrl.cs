using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillTest_Ctrl : MonoBehaviour
{
    private InputField mInput;
    private ButtonCtrl Button_Sure;
    private int skillid;

    private void Awake()
    {
        this.mInput = base.transform.Find("InputField").GetComponent<InputField>();
        this.Button_Sure = base.transform.Find("Button_Sure").GetComponent<ButtonCtrl>();
        this.Button_Sure.onClick = new Action(this.OnButtonClick);
        this.mInput.onValueChanged.AddListener(value => int.TryParse(value, out this.skillid));
    }

    private void OnButtonClick()
    {
        GameLogic.Self.AddSkillTest(this.skillid);
        this.mInput.text = string.Empty;
        this.mInput.ActivateInputField();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            this.OnButtonClick();
        }
    }
}

