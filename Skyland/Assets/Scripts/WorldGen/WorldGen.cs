using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("World Options")]
    public int RADIUS = 30;
    public Vector3 startPos;
    bool[,,] square;
    bool[,] grass;
    public GameObject prefab;
    public GameObject dirt;
    public GameObject obelisk;
    public GameObject parent;
    public int divs = 10;
    float[,] HeightMap;
    public float noiseFactor;
    public float heightChange;
    public float noiseOffset;
    GameObject sup_parent;
    GameObject[] grassDivs;
    public int divisions; // how many different parents to use
    float step = .01f;
    [Header("Spawnables")]     //ALL ABOUT OBJECTS
    static List<Vector3> positions;
    static List<float> positionsRadii;
    public GameObject treeRespawnUI_Prefab;
    public GameObject rockRespawnUI_Prefab;
    GameObject treeRespawnUI;
    GameObject rockRespawnUI;
    public GameObject[] tree;
    public GameObject[] rock;
    public GameObject[] flowers;
    public float noSpawnRadius;
    public int TreeAmount;
    public int RockAmount;
    public int FlowerAmount;
    public float baseTreeRespawnTime;
    public float baseRockRespawnTime;
    public float baseFlowerRespawnTime;
    public float treeRadius;
    public float RockRadius;
    public float flowerRadius;
    List<float> treeRespawnTimes;
    List<float> rockRespawnTimes;
    List<float> flowerRespawnTimes;
    [Header("Player Creation")]
    public GameObject playerPrefab;
    GameObject player;
    [Header("Inventory Manager")]
    public InventoryTemplate InvTemplate;


    public void setUIState(bool state)
    {
        treeRespawnUI.SetActive(state);
        rockRespawnUI.SetActive(state);
    }

    void HotMessCreation()
    {
        HeightMap = new float[RADIUS * 2 + 1, RADIUS * 2 + 1];
        square = new bool[RADIUS * 2 + 1, RADIUS * 2 + 1, RADIUS * 2 + 1];
        grass = new bool[RADIUS * 2 + 1, RADIUS * 2 + 1];
        grassDivs = new GameObject[divs];
        sup_parent = Instantiate(parent);
        sup_parent.name = "World Objects";
        for (int i = 0; i < RADIUS * 2 + 1; i++)
        {
            for (int j = 0; j < RADIUS * 2 + 1; j++)
            {
                float n = Mathf.PerlinNoise(i * noiseFactor + noiseOffset, (j * noiseFactor));
                float distance = Mathf.Sqrt((i - RADIUS) * (i - RADIUS) + (j - RADIUS) * (j - RADIUS));
                float o = (distance < noSpawnRadius) ? 0 : (distance - noSpawnRadius < 8) ? (n * heightChange) * ((distance - noSpawnRadius) / 8) : (n * heightChange);
                float p = (int)o + (int)(((o - (int)o) * 100) / 50);
                HeightMap[i, j] = p + 1;

            }
        }
        for (int i = 0; i < divs; i++)
        {
            grassDivs[i] = Instantiate(parent, sup_parent.transform);
        }
        for (float i = 0; i < 6.3; i += step)
        {
            for (float j = 0; j <= RADIUS * 2; j += step)
            {
                int a, b, c;
                float layerRad = Mathf.Sin((j / (RADIUS * 2)) * Mathf.PI / 2) * RADIUS;
                a = (int)(Mathf.Sin(i) * layerRad);
                b = (int)(Mathf.Cos(i) * layerRad);
                c = (int)j;
                square[a + RADIUS, c, b + RADIUS] = true;
                grass[a + RADIUS, b + RADIUS] = true;
            }
        }
        for (int i = 0; i < RADIUS * 2 + 1; i++)
        {
            for (int j = 0; j < RADIUS * 2 + 1; j++)
            {
                if (grass[i, j] == true)
                {
                    Vector3 pos = new Vector3(i - RADIUS, 0, j - RADIUS);
                    int index = (int)(((float)i / ((float)RADIUS * 2 + 1)) * grassDivs.Length);
                    GameObject d = Instantiate(dirt, pos, Quaternion.identity, grassDivs[index].transform);
                    d.transform.localScale = new Vector3(1f, HeightMap[i, j], 1f);
                }

                for (int k = 0; k < RADIUS * 2 + 1; k++)
                {
                    if (square[i, j, k])
                    {
                        /*
                        count++;
                        Vector3 pos = new Vector3(i - RADIUS, j - RADIUS * 2, k - RADIUS);
                        int index = (int)(((float)k / ((float)RADIUS * 2 + 1)) * (divisions - 1));
                        Instantiate(prefab, pos, Quaternion.identity, divs[index].transform);
                        */
                    }

                }
            }
        }

        // Create Grass

        foreach (GameObject d in grassDivs)
        {
            d.GetComponent<CombineMeshes>().Combine(dirt.GetComponent<MeshRenderer>().sharedMaterial);
            d.AddComponent<MeshCollider>();
            d.layer = 13;
        }
        /*
        foreach (GameObject d in divs)
        {
            d.GetComponent<CombineMeshes>().Combine();
        }
        */
        sup_parent.transform.position = startPos;
    }
    void CreatePlayer()
    {
        player = Instantiate(playerPrefab, startPos + Vector3.up * 2 + Vector3.forward * 4, Quaternion.identity);
    }
    void SpawnObelisk()
    {
        Instantiate(obelisk, startPos, Quaternion.identity);
    }
    void ScatterObjects(GameObject[] g, int amount, float radius, string name)
    {
        GameObject p = Instantiate(parent);
        p.name = name;
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = FindPosition(radius);
            int r = Random.Range(0, g.Length - 1);
            int rrotation = Random.Range(0, 3);
            Quaternion q = Quaternion.identity;
            q.eulerAngles += new Vector3(0, rrotation * 90, 0);
            IEnumerator c = DelayedInstantiate(g[r], pos, q, p.transform);
            StartCoroutine(c);
        }
    }
    void CreateInvTemplate()
    {
        Inventory.template = InvTemplate;
    }
    void Awake()
    {
        WorldGeneration(startPos);
        //WorldGeneration(startPos + Vector3.forward * -200);
        CreatePlayer();
        CreateInvTemplate();
        CreateRespawnUI();
        treeRespawnTimes = new List<float>();
        rockRespawnTimes = new List<float>();
        flowerRespawnTimes = new List<float>();
        // Physics.gravity *= .8f;
    }
    void WorldGeneration(Vector3 pos)
    {
        startPos = pos;
        HotMessCreation();
        ScatterObjects(tree, TreeAmount, treeRadius, "trees");
        ScatterObjects(rock, RockAmount, treeRadius, "rocks");
        ScatterObjects(flowers, FlowerAmount, treeRadius, "flowers");
        SpawnObelisk();
    }
    // Update is called once per frame
    void Update()
    {
        updateRespawns();
    }
    void updateRespawns()
    {
        if (treeRespawnTimes.Count > 0)
        {
            if (treeRespawnTimes[0] < Time.time)
            {
                ScatterObjects(tree, 1, treeRadius, "RespawnedTree");
                treeRespawnTimes.RemoveAt(0);
            }
        }
        if (rockRespawnTimes.Count > 0)
        {
            if (rockRespawnTimes[0] < Time.time)
            {
                ScatterObjects(rock, 1, RockRadius, "RespawnedRock");
                rockRespawnTimes.RemoveAt(0);
            }
        }
        if (flowerRespawnTimes.Count > 0)
        {
            if (flowerRespawnTimes[0] < Time.time)
            {
                ScatterObjects(flowers, 1, flowerRadius, "RespawnedFlower");
            }
        }
    }
    public void OnObjectDeath(Vector3 position, ObjectType type)
    {
        RemovePosition(position);
        switch (type)
        {
            case ObjectType.Rock:
                rockRespawnTimes.Add(Time.time + baseRockRespawnTime);
                rockRespawnUI.GetComponent<ObjectRespawnUI>().AddSlider(baseRockRespawnTime);
                break;
            case ObjectType.Tree:
                treeRespawnTimes.Add(Time.time + baseTreeRespawnTime);
                treeRespawnUI.GetComponent<ObjectRespawnUI>().AddSlider(baseTreeRespawnTime);
                break;
            case ObjectType.Flower:
                flowerRespawnTimes.Add(Time.time + baseFlowerRespawnTime);
                break;
        }
    }
    Vector3 FindPosition(float radius)
    {
        if (positions == null)
        {
            positions = new List<Vector3>();
            positionsRadii = new List<float>();
        }
        Vector3 pos;
        pos = Vector3.zero;
        int i = 0;
        while (i < 200)
        {
            bool valid = true;
            float randomRadius = Random.Range(noSpawnRadius, RADIUS - 1);
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            int x, z;
            x = (int)(randomRadius * Mathf.Cos(randomAngle) + startPos.x);
            z = (int)(randomRadius * Mathf.Sin(randomAngle) + startPos.z);
            int j = x + (RADIUS);
            int k = z + (RADIUS);
            x += (int)startPos.x;
            z += (int)startPos.z;
            pos = new Vector3(x, (HeightMap[j, k] / 2) + startPos.y - .5f, z);
            int ij = 0;
            foreach (Vector3 v in positions)
            {
                if ((v - pos).magnitude < radius + positionsRadii[ij])
                {
                    valid = false;
                }
                ij++;
            }
            if (valid)
            {
                break;
            }
            i++;
        }
        positions.Add(pos);
        positionsRadii.Add(radius);
        return pos;
    }
    public static void AddPosition(Vector3 pos, float radius)
    {
        positions.Add(pos);
        positionsRadii.Add(radius);
    }
    static public void RemovePosition(Vector3 position)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i] == position)
            {
                positions.RemoveAt(i);
                positionsRadii.RemoveAt(i);
                break;
            }
        }
    }

    static public bool CheckPosition(Vector3 pos, float radius)
    {
        int i = 0;
        foreach (Vector3 v in positions)
        {
            if ((v - pos).magnitude < radius + positionsRadii[i])
            {
                return false;
            }
            i++;
        }
        return true;
    }
    static public IEnumerator DelayedInstantiate(GameObject g, Vector3 pos, Quaternion q, Transform parent)
    {
        float r = Random.Range(0f, 2f);
        yield return new WaitForSeconds(r);
        Instantiate(g, pos, q, parent);
    }
    void CreateRespawnUI()
    {
        treeRespawnUI = Instantiate(treeRespawnUI_Prefab);
        rockRespawnUI = Instantiate(rockRespawnUI_Prefab);
    }
}
public enum ObjectType
{
    Tree, Rock, Flower, Watchtower, Cannon, Player
}