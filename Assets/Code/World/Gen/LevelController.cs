using Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace World.Gen
{
    public class LevelController : MonoBehaviour
    {
        GameObject parent;

        public Texture2D dungeonTexture;

        public RawImage img;
        public RawImage playerIcon;
        List<BSPNode> leaves;
        BSPNode tree;
        float worldScale = 3;

        public static LevelController ctrl;

        [SerializeField] Transform player;

        [Header("Prefabs")]

        [SerializeField] GameObject wallpiece;
        [SerializeField] GameObject enemy;
        [SerializeField] GameObject trapdoor;
        [SerializeField] GameObject chest;
        [SerializeField] GameObject[] props;

        private void Awake()
        {
            Generate();
            ctrl = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G)) Generate();

            playerIcon.rectTransform.anchoredPosition = (Vector2.right * player.position.x + Vector2.up * player.position.z);

        }

        private void Generate()
        {
            tree = new(new RectInt(0, 0, 47, 47), 0);

            tree.TrySplitLeaf(0.5f);

            leaves = tree.GetLeaves();


            List<Corridor> corridors = tree.GetCorridors();

            print("Length = " + corridors.Count);

            dungeonTexture = DrawDungeon(leaves, corridors, tree);

            dungeonTexture.Apply();


            img.texture = dungeonTexture;

            BuildLevel(dungeonTexture);


        }

        private Vector3 MapSpaceToWorldSpace(Vector2 spawnpoint)
        {

            Vector2Int worldPos = MathHelper.RoundVector2ToInt(spawnpoint) - tree.rect.size / 2;

            return ((Vector3.right * worldPos.x + Vector3.forward * worldPos.y) - new Vector3(2,0,2)) * worldScale;
        }

        private Texture2D DrawDungeon(List<BSPNode> leaves, List<Corridor> corridors, BSPNode root)
        {
            Texture2D dungeonTexture = new Texture2D(50, 50)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 0,
                wrapMode = TextureWrapMode.Repeat,
            };

            DrawRooms(leaves, dungeonTexture);
            DrawCorridors(corridors, dungeonTexture);
            PopulateRooms(leaves);

            return dungeonTexture;
        }

        private void PopulateRooms(List<BSPNode> leaves)
        {
            List<BSPNode> intersections = new();
            List<BSPNode> deadEnds = new();

            foreach (BSPNode leaf in leaves)
            {
                if (leaf.corridors.Count == 1)
                {
                    deadEnds.Add(leaf);
                }

                else
                {
                    intersections.Add(leaf);
                }
            }


            BSPNode startroom = deadEnds[0];
            BSPNode endroom = deadEnds[deadEnds.Count - 1];

            deadEnds.Remove(startroom);
            deadEnds.Remove(endroom);

            SpawnPlayer(startroom);
            SpawnProps(intersections);
            SpawnTreasure(deadEnds);
            SpawnEndRoom(endroom);
            SpawnEnemies(intersections);
        }

        private void SpawnEnemies(List<BSPNode> intersections)
        {
            foreach (BSPNode room in intersections)
            {
                int enemyCount = Random.Range(-2, 3);

                for (int i = 0; i < enemyCount; i++)
                {
                    Instantiate(enemy, MapSpaceToWorldSpace(room.room.center), Quaternion.identity);
                }
            }
        }

        private void SpawnEndRoom(BSPNode endroom)
        {
            Instantiate(trapdoor, MapSpaceToWorldSpace(endroom.room.center), Quaternion.identity);
        }

        private void SpawnProps(List<BSPNode> rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {

                Instantiate(props[Random.Range(0, props.Length)], MapSpaceToWorldSpace(rooms[i].room.center), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));

            }
        }

        private void SpawnTreasure(List<BSPNode> rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {

                Quaternion rotation = Quaternion.LookRotation(MapSpaceToWorldSpace(rooms[i].corridors[0].center));
                Instantiate(chest, MapSpaceToWorldSpace(rooms[i].room.center), rotation);

            }
        }

        private void SpawnPlayer(BSPNode node)
        {
            RectInt spawnroom = node.room;

            Vector3 playerpos = MapSpaceToWorldSpace(spawnroom.center);

            player.position = playerpos;
        }

        private static void DrawCorridors(List<Corridor> corridors, Texture2D dungeonTexture)
        {
            foreach (var corridor in corridors)
            {
                corridor.FindClosest();

                if (corridor.connected.Length > 0)
                {

                    DrawCorridor(dungeonTexture, corridor.connected[0].room.center, corridor.connected[1].room.center, Color.red);

                }
            }
        }

        private static void DrawRooms(List<BSPNode> leaves, Texture2D dungeonTexture)
        {
            for (int x = -1; x < dungeonTexture.width; x++)
            {
                for (int y = -1; y < dungeonTexture.height; y++)
                {
                    dungeonTexture.SetPixel(x, y, Color.black);

                    foreach (BSPNode leaf in leaves)
                    {
                        if (leaf.room.Contains(new Vector2Int(x, y)))
                        {
                            dungeonTexture.SetPixel(x, y, Color.white);
                        }
                    }
                }
            }
        }

        private static Texture2D DrawCorridor(Texture2D dungeonTexture, Vector2 start, Vector2 end, Color color)
        {
            var line = AI.Pathfinding.PlanCorridor(dungeonTexture, start, end, color);

            foreach (Vector2 key in line.Keys)
            {
                Vector2Int keyInt = MathHelper.RoundVector2ToInt(key);

                if (dungeonTexture.GetPixel(keyInt.x, keyInt.y) == Color.black)
                    dungeonTexture.SetPixel(Mathf.RoundToInt(key.x), Mathf.RoundToInt(key.y), line[key]);
            }

            return dungeonTexture;
        }

        void BuildLevel(Texture2D tex)
        {
            if (parent != null) Destroy(parent);

            parent = new GameObject();

            for (int x = 0; x < tex.width; x++)
            {

                for (int z = 0; z < tex.height; z++)
                {

                    if (tex.GetPixel(x, z) == Color.black)
                    {

                        GameObject wallpieceInstance = Instantiate(wallpiece, new Vector3(x - tex.width / 2, 0, z - tex.height / 2) * worldScale, Quaternion.identity);

                        wallpieceInstance.transform.SetParent(parent.transform);

                    }
                }
            }
        }
    }
}