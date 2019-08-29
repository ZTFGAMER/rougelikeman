using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Transform player;
    public Transform destPos;
    private NodeItem[,] grid;
    private int w;
    private int h;
    private GameObject WallRange;
    private GameObject PathRange;
    private List<GameObject> pathObj = new List<GameObject>();

    public NodeItem getItem(Vector3 position)
    {
        if ((GameLogic.Release != null) && (GameLogic.Release.MapCreatorCtrl != null))
        {
            Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(position);
            if ((this.grid != null) && ((this.grid.GetLength(0) > roomXY.x) && (this.grid.GetLength(1) > roomXY.y)))
            {
                return this.grid[roomXY.x, roomXY.y];
            }
            return new NodeItem(false, 5, 0);
        }
        if (((this.grid != null) && (this.grid.GetLength(0) > 5)) && (this.grid.GetLength(1) > 0))
        {
            return this.grid[5, 0];
        }
        return new NodeItem(false, 5, 0);
    }

    public List<NodeItem> getNeibourhood(NodeItem node)
    {
        List<NodeItem> list = new List<NodeItem>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (((i != 0) || (j != 0)) && ((i == 0) || (j == 0)))
                {
                    int num3 = node.x + i;
                    int num4 = node.y + j;
                    if (((num3 < this.w) && (num3 >= 0)) && ((num4 < this.h) && (num4 >= 0)))
                    {
                        list.Add(this.grid[num3, num4]);
                    }
                }
            }
        }
        return list;
    }

    public void Init(int[,] list)
    {
        this.w = list.GetLength(0);
        this.h = list.GetLength(1);
        this.grid = new NodeItem[this.w, this.h];
        for (int i = 0; i < this.w; i++)
        {
            for (int j = 0; j < this.h; j++)
            {
                bool isWall = list[i, j] > 0;
                this.grid[i, j] = new NodeItem(isWall, i, j);
            }
        }
    }

    public void updatePath(List<NodeItem> lines)
    {
    }

    public class NodeItem
    {
        public bool isWall;
        public int x;
        public int y;
        public int gCost;
        public int hCost;
        public Grid.NodeItem parent;

        public NodeItem(bool isWall, int x, int y)
        {
            this.isWall = isWall;
            this.x = x;
            this.y = y;
        }

        public int fCost =>
            (this.gCost + this.hCost);
    }
}

