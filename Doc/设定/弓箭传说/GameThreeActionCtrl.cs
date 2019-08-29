using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameThreeActionCtrl : ActionBasic
{
    public void DoAction(List<Transform> list, List<Transform> shadowlist, GameObject sieve, Action callback)
    {
        base.ActionClear();
        ActionBasic.ActionShowMaskUI action = new ActionBasic.ActionShowMaskUI {
            show = true
        };
        base.AddAction(action);
        ActionShowSieve sieve2 = new ActionShowSieve {
            sieve = sieve,
            show = true
        };
        base.AddAction(sieve2);
        ActionBasic.ActionWaitIgnoreTime time = new ActionBasic.ActionWaitIgnoreTime {
            waitTime = 0.5f
        };
        base.AddAction(time);
        ActionUp up = new ActionUp {
            list = list
        };
        base.AddAction(up);
        time = new ActionBasic.ActionWaitIgnoreTime {
            waitTime = 0.5f
        };
        base.AddAction(time);
        ActionDown down = new ActionDown {
            list = list
        };
        base.AddAction(down);
        sieve2 = new ActionShowSieve {
            sieve = sieve,
            show = false
        };
        base.AddAction(sieve2);
        ActionRandomSieve sieve3 = new ActionRandomSieve {
            list = list,
            shadowlist = shadowlist,
            sieve = sieve.transform
        };
        base.AddAction(sieve3);
        sieve2 = new ActionShowSieve {
            sieve = sieve,
            show = true
        };
        base.AddAction(sieve2);
        action = new ActionBasic.ActionShowMaskUI {
            show = false
        };
        base.AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            action = callback
        };
        base.AddAction(delegate2);
    }

    public void OnClickOne(Transform transform, Transform sieve, Action<bool> callback)
    {
        base.ActionClear();
        ActionUp action = new ActionUp();
        List<Transform> list = new List<Transform> {
            transform
        };
        action.list = list;
        base.AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            actionbool = callback,
            resultbool = transform.localPosition.x == sieve.localPosition.x
        };
        base.AddAction(delegate2);
    }

    protected override void OnUpdate(float delta)
    {
        if (base.actionList.Count > 0)
        {
            ActionBasic.ActionBase base2 = base.actionList[0];
            base2.Init();
            base2.Update();
            if (base2.IsEnd)
            {
                base.actionList.RemoveAt(0);
            }
        }
    }

    public class ActionDown : ActionBasic.ActionUIBase
    {
        public List<Transform> list;
        private int frame = 10;
        private int currentframe;
        private float Speed = 50f;

        private void DoPosition(float speed)
        {
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                float y = this.list[num].localPosition.y + speed;
                this.list[num].localPosition = new Vector3(this.list[num].localPosition.x, y, this.list[num].localPosition.z);
                num++;
            }
        }

        protected override void OnInit()
        {
            this.currentframe = 0;
        }

        protected override void OnUpdate()
        {
            if (this.currentframe < this.frame)
            {
                float speed = -(((float) this.currentframe) / ((float) this.frame)) * this.Speed;
                this.DoPosition(speed);
                this.currentframe++;
            }
            else
            {
                base.End();
            }
        }
    }

    public class ActionRandomSieve : ActionBasic.ActionUIBase
    {
        public List<Transform> list;
        public List<Transform> shadowlist;
        public Transform sieve;
        private Transform transform1;
        private Transform transform2;
        private Transform shadow1;
        private Transform shadow2;
        private int moveCount = 8;
        private int currentCount;
        private int moveframe = 12;
        private int currentframe;
        private float movestartx;
        private float moveendx;
        private int currentstate;
        private float y;
        private float yValue = 5f;

        protected override void OnInit()
        {
        }

        protected override void OnUpdate()
        {
            if (this.currentCount < this.moveCount)
            {
                if (this.currentstate == 0)
                {
                    this.currentframe = 0;
                    this.RandomTransform(out this.transform1, out this.transform2, out this.shadow1, out this.shadow2);
                    this.movestartx = this.transform1.localPosition.x;
                    this.moveendx = this.transform2.localPosition.x;
                    this.currentstate = 1;
                }
                else if (this.currentstate == 1)
                {
                    if (this.currentframe < (this.moveframe / 2))
                    {
                        this.y = -this.currentframe * this.yValue;
                    }
                    else
                    {
                        this.y = -((this.moveframe - this.currentframe) - 1) * this.yValue;
                    }
                    this.transform1.localPosition = new Vector3(this.movestartx + (((this.moveendx - this.movestartx) * (this.currentframe + 1f)) / ((float) this.moveframe)), this.y, this.transform1.localPosition.z);
                    this.transform2.localPosition = new Vector3(this.moveendx + (((this.movestartx - this.moveendx) * (this.currentframe + 1f)) / ((float) this.moveframe)), -this.y, this.transform2.localPosition.z);
                    this.shadow1.localPosition = new Vector3(this.transform1.localPosition.x, this.shadow1.localPosition.y, this.shadow1.localPosition.z);
                    this.shadow2.localPosition = new Vector3(this.transform2.localPosition.x, this.shadow2.localPosition.y, this.shadow2.localPosition.z);
                    this.currentframe++;
                    if (this.currentframe == this.moveframe)
                    {
                        this.currentstate = 2;
                    }
                }
                else if (this.currentstate == 2)
                {
                    this.sieve.localPosition = this.list[1].localPosition;
                    this.currentCount++;
                    this.currentstate = 0;
                }
            }
            else
            {
                base.End();
            }
        }

        private void RandomTransform(out Transform transform1, out Transform transform2, out Transform shadow1, out Transform shadow2)
        {
            int num = GameLogic.Random(0, this.list.Count);
            transform1 = this.list[num];
            shadow1 = this.shadowlist[num];
            int num2 = GameLogic.Random(0, this.list.Count);
            while (num2 == num)
            {
                num2 = GameLogic.Random(0, this.list.Count);
            }
            transform2 = this.list[num2];
            shadow2 = this.shadowlist[num2];
        }
    }

    public class ActionShowSieve : ActionBasic.ActionUIBase
    {
        public GameObject sieve;
        public bool show;

        protected override void OnInit()
        {
            this.sieve.SetActive(this.show);
            base.End();
        }
    }

    public class ActionUp : ActionBasic.ActionUIBase
    {
        public List<Transform> list;
        private int frame = 10;
        private int currentframe;
        private float Speed = 50f;

        private void DoPosition(float speed)
        {
            int num = 0;
            int count = this.list.Count;
            while (num < count)
            {
                float y = this.list[num].localPosition.y + speed;
                this.list[num].localPosition = new Vector3(this.list[num].localPosition.x, y, this.list[num].localPosition.z);
                num++;
            }
        }

        protected override void OnInit()
        {
            this.currentframe = 0;
        }

        protected override void OnUpdate()
        {
            if (this.currentframe < this.frame)
            {
                float speed = (1f - ((this.currentframe + 1f) / ((float) this.frame))) * this.Speed;
                this.DoPosition(speed);
                this.currentframe++;
            }
            else
            {
                base.End();
            }
        }
    }
}

