using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Misc;

namespace AI
{
    public static class Pathfinding
    {
        public static Dictionary<Vector2, Color> PlanCorridor(Texture2D tex, Vector2 start, Vector2 target, Color color)
        {
            Dictionary<Vector2, Color> line = new();

            Vector2 currentPos = MathHelper.RoundVector2ToInt(start);

            while (currentPos != target)
            {
                line.Add(currentPos, color);
                Vector2 moveDir = CalculateNextMove(tex, target, currentPos);

                currentPos += moveDir;
            }

            line.Add(currentPos, color);

            return line;
        }

        private static Vector2 CalculateNextMove(Texture2D tex, Vector2 target, Vector2 currentPos)
        {
            Vector2 moveDir = Vector2.zero;
            Vector2 delta = target - currentPos;

            if (Mathf.Abs(delta.x) > 0) moveDir.x += Mathf.Clamp(delta.x, -1, 1);
            if (Mathf.Abs(delta.y) > 0) moveDir.y += Mathf.Clamp(delta.y, -1, 1);

            if (moveDir.magnitude > 1)
            {

                var xMove = Vector2.right * moveDir.x;
                var yMove = Vector2.up * moveDir.y;

                if(CalculateUndesirability(tex, currentPos+xMove, target) < CalculateUndesirability(tex, currentPos + yMove,  target))
                {
                    moveDir = xMove;
                }

                else
                {
                    moveDir = yMove;
                }

            }


            return moveDir;
        }

        private static float CalculateUndesirability(Texture2D tex, Vector2 cell, Vector2 target)
        {
            float undesirability = Mathf.Abs(cell.x - target.x) + Mathf.Abs(cell.y - target.y);

            if (tex.GetPixel(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y)) == Color.black)
            {
                for(int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {

                        if (tex.GetPixel(Mathf.RoundToInt(cell.x + x), Mathf.RoundToInt(cell.y + y)) != Color.black) undesirability += 1;

                    }
                }
            }

            else
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (tex.GetPixel(Mathf.RoundToInt(cell.x + x), Mathf.RoundToInt(cell.y + y)) == Color.black) undesirability += 1;
                    }
                }
            }


            return undesirability;
        }
    }


    [System.Serializable]
    public class AStarNode
    {
        public Vector2 position;
        public Vector2 previousPos;
        public float hCost; // Distance to the target node
        public float gCost; // Number of steps from the start to reach the current point
        public float fCost; // Sum of the other two costs, lower costs meaning this is a desirable path to take
        public bool closed = false;

        public AStarNode(Vector2 pos, Vector2 target, float cost)
        {
            position = pos;

            gCost = cost;
            hCost = Mathf.Abs(pos.x - target.x) + Mathf.Abs(pos.y - target.y);
            fCost = gCost + hCost * 1.0001f;
        }
    }
}

