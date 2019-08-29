using System;
using UnityEngine;

public class BoxRedAniCtrl : MonoBehaviour
{
    private GameObject Obj_RedNode;
    private Animator BoxAni;
    private ButtonCtrl Button_Box;

    private void Awake()
    {
        this.Obj_RedNode = base.transform.Find("Button_Box/fg/Image_RedNode").gameObject;
        this.BoxAni = base.GetComponent<Animator>();
        this.Button_Box = base.transform.Find("Button_Box").GetComponent<ButtonCtrl>();
        this.Button_Box.onClick = delegate {
            this.BoxAniPlay(false);
            WindowUI.ShowWindow(WindowID.WindowID_BoxOpen);
        };
    }

    private void BoxAniPlay(bool play)
    {
        if (this.BoxAni != null)
        {
            this.BoxAni.enabled = play;
            if (!play)
            {
                this.BoxAni.transform.localRotation = Quaternion.identity;
                this.BoxAni.transform.localScale = Vector3.one;
            }
        }
    }

    private void OnDisable()
    {
        if (this.BoxAni != null)
        {
            this.BoxAni.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (this.BoxAni != null)
        {
            this.BoxAni.enabled = true;
        }
    }

    public void UpdateBox()
    {
    }

    public void UpdateCanOpen(int count)
    {
    }
}

