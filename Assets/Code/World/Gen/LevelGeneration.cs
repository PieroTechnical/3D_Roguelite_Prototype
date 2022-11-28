using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World.Gen
{
    public class BSPNode
    {
        readonly float maxRatio = 1.1f;
        readonly float cutRandomness = 0.05f;
        readonly int maxDepth = 4;
        public readonly int border = 3;

        public int depth = 0;
        public int entrances = 0;

        public RectInt rect;
        public RectInt room;

        public BSPNode[] children;
        public BSPNode parent;

        public List<Corridor> corridors = new();

        public BSPNode(RectInt rect, int depth)
        {
            this.rect = rect;
            this.depth = depth;
            children = new BSPNode[0];
            room = new RectInt(0, 0, 0, 0);
        }

        public void TrySplitLeaf(float cutPoint)
        {
            if (depth >= maxDepth || Random.Range(1, 10)-depth <= 0)
            {
                BuildRoom(border);
                return;
            }

            bool cutH = Random.Range(0, 2) == 1;

            // Room too wide, force cut along y axis
            if (rect.width / rect.height > maxRatio) cutH = false;

            // Too tall, Force x axis cut
            if (rect.height / rect.width > maxRatio) cutH = true;

            SplitLeaf(cutPoint, cutH);
        }

        void SplitLeaf(float cutPoint, bool cutH)
        {
            RectInt child1Rect = rect;
            RectInt child2Rect = rect;

            if (cutH)
            {
                int localCutPoint = Mathf.RoundToInt(rect.y + rect.height * cutPoint);

                child1Rect.yMax = localCutPoint;
                child2Rect.yMin = localCutPoint;
            }

            else
            {
                int localCutPoint = Mathf.RoundToInt(child2Rect.x + child2Rect.width * cutPoint);

                child1Rect.xMax = localCutPoint;
                child2Rect.xMin = localCutPoint;
            }


            children = new BSPNode[2]
            {
            new BSPNode(child1Rect, depth + 1),
            new BSPNode(child2Rect, depth + 1)
            };

            foreach (BSPNode child in children)
            {
                child.parent = this;
                float nextCutPoint = Random.Range(0.5f - cutRandomness, 0.5f + cutRandomness);
                child.TrySplitLeaf(nextCutPoint);
            }
        }


        void BuildRoom(int border = 3)
        {
            room = rect;


            if (room.width > room.height + 4)
            {

                room.width = room.height;

            }

            else if (room.height > room.width + 4)
            {

                room.height = room.width;

            }

            int xOffset = Random.Range(0, rect.width - room.width);
            int yOffset = Random.Range(0, rect.height - room.height);

            room.x += xOffset + border;
            room.y += yOffset + border;
            room.width -= border;
            room.height -= border;


        }


        public List<BSPNode> GetLeaves()
        {
            if (children.Length == 0)
            {
                return new List<BSPNode> { this };
            }

            else
            {
                List<BSPNode> grandchildren = new();

                grandchildren.AddRange(children[0].GetLeaves());
                grandchildren.AddRange(children[1].GetLeaves());

                return grandchildren;
            }
        }

        public List<Corridor> GetCorridors()
        {
            if (children.Length == 0)
            {
                return new List<Corridor>() { new() };
            }

            else
            {
                List<Corridor> childCorridors = new() { };

                childCorridors.Add(new(children));

                childCorridors.AddRange(children[0].GetCorridors());
                childCorridors.AddRange(children[1].GetCorridors());


                return childCorridors;

            }
        }
    }

    public class Corridor
    {
        public BSPNode[] connected = new BSPNode[] { };
        public Vector2 center = Vector2.zero;

        public Corridor(BSPNode[] connected)
        {
            this.connected = connected;

            center = (connected[0].rect.center + connected[1].rect.center) / 2;
        }

        public void FindClosest()
        {
            if (connected.Length == 0) return;

            for (int i = 0; i < 2; i++)
            {
                while (connected[i].children.Length > 0)
                {
                    var closest = connected[i].children[0];

                    if (Vector2.Distance(center, connected[i].children[1].rect.center) < Vector2.Distance(center, closest.rect.center))
                    {
                        closest = connected[i].children[1];
                    }

                    connected[i] = closest;
                    center = (connected[0].rect.center + connected[1].rect.center) / 2;
                }
            }

            connected[0].entrances++;
            connected[1].entrances++;

            connected[0].corridors.Add(this);
            connected[1].corridors.Add(this);
        }

        public Corridor() { }
    }
}