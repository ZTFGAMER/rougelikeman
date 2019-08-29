using Dxx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FindPath
{
    private Grid grid = new Grid();
    private Queue<GameObject> mCacheList = new Queue<GameObject>();
    private List<GameObject> mUseList = new List<GameObject>();

    public FindPath()
    {
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        this.InitData();
    }

    private void CacheSphere(GameObject o)
    {
        GameObjectPool.Release("Sphere", o);
    }

    public void DeInit()
    {
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }

    public List<Grid.NodeItem> FindingPath(Vector3 s, Vector3 e)
    {
        Grid.NodeItem item = this.grid.getItem(s);
        Grid.NodeItem endNode = this.grid.getItem(e);
        List<Grid.NodeItem> toRelease = ListPool<Grid.NodeItem>.Get();
        HashSet<Grid.NodeItem> set = HashSetPool<Grid.NodeItem>.Get();
        toRelease.Add(item);
        List<Grid.NodeItem> list2 = new List<Grid.NodeItem>();
        while (toRelease.Count > 0)
        {
            Grid.NodeItem item3 = toRelease[0];
            int num = 0;
            int count = toRelease.Count;
            while (num < count)
            {
                if ((toRelease[num].fCost <= item3.fCost) && (toRelease[num].hCost < item3.hCost))
                {
                    item3 = toRelease[num];
                }
                num++;
            }
            toRelease.Remove(item3);
            set.Add(item3);
            if (item3 == endNode)
            {
                list2 = this.generatePath(item, endNode);
                break;
            }
            List<Grid.NodeItem> list3 = this.grid.getNeibourhood(item3);
            for (int i = 0; i < list3.Count; i++)
            {
                Grid.NodeItem item4 = list3[i];
                if (!item4.isWall && !set.Contains(item4))
                {
                    int num4 = item3.gCost + this.getDistanceNodes(item3, item4);
                    if ((num4 < item4.gCost) || !toRelease.Contains(item4))
                    {
                        item4.gCost = num4;
                        item4.hCost = this.getDistanceNodes(item4, endNode);
                        item4.parent = item3;
                        if (!toRelease.Contains(item4))
                        {
                            toRelease.Add(item4);
                        }
                    }
                }
            }
        }
        if (list2.Count == 0)
        {
            list2 = this.generatePath(item, null);
        }
        ListPool<Grid.NodeItem>.Release(toRelease);
        HashSetPool<Grid.NodeItem>.Release(set);
        return list2;
    }

    private List<Grid.NodeItem> generatePath(Grid.NodeItem startNode, Grid.NodeItem endNode)
    {
        List<Grid.NodeItem> lines = new List<Grid.NodeItem>();
        if (endNode != null)
        {
            for (Grid.NodeItem item = endNode; item != startNode; item = item.parent)
            {
                lines.Add(item);
            }
            lines.Reverse();
        }
        this.grid.updatePath(lines);
        return lines;
    }

    private int getDistanceNodes(Grid.NodeItem a, Grid.NodeItem b)
    {
        int num = Mathf.Abs((int) (a.x - b.x));
        int num2 = Mathf.Abs((int) (a.y - b.y));
        return ((num + num2) * 10);
    }

    private GameObject GetSphere() => 
        GameObjectPool.Instantiate("Sphere");

    public void Init(int[,] rects)
    {
        this.grid.Init(rects);
    }

    private void InitData()
    {
        this.Init(GameLogic.Release.MapCreatorCtrl.GetFindPathRect());
    }

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        this.InitData();
    }
}

