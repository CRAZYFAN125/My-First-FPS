using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [System.Serializable]
    public class SpawnRate
    {
        public GameObject @object;
        [Range(0, 1)] public float Szansa;
        [HideInInspector] public float SzansaMin;
        [HideInInspector] public float SzansaMax;
    }
    float chance;
    Mesh mesh;

    public NavMeshSurface surface;

    [HideInInspector] public Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    [HideInInspector] public int xSize = 20;
    [HideInInspector] public int zSize = 20;
    public GameObject ObjectToSpawn;
    public SpawnRate[] Spawns;
    Transform MCamera;
    public Quaternion q;
    [HideInInspector] public static bool Ready = false;
    public float secondsTimeWait = .1f;
    public int EnemyCount = 0;

    [SerializeField] GameObject Shooter;
    int fala = 5;

    // Start is called before the first frame update
    void Start()
    {
        chance = 100/Spawns.Length;

        foreach (SpawnRate item in Spawns)
        {
            item.SzansaMin = item.Szansa*100;
            item.SzansaMax = (item.Szansa*chance)*100;
        }

        mesh = new Mesh();
        mesh.name = "CustomMesh";
        GetComponent<MeshFilter>().mesh = mesh;

        int x = PlayerPrefs.GetInt("CONF");
        if (x != 0)
        {
            xSize = PlayerPrefs.GetInt("Gen");
            zSize = xSize;
        }

        MCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Vector3 vector = new Vector3(-(xSize / 2), 0, -(zSize / 2));
        gameObject.transform.position = vector;
        MCamera.position = vector + new Vector3(-3, 10, -3);
        MCamera.rotation = q;



        StartCoroutine(CreateShape());

    }
    private void FixedUpdate()
    {
        if (!Ready)
        {
            UpdateMesh(); return;
        }
        int enCount = 0;
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject item in enemys)
        {
            enCount += 1;
        }
        if (enCount < EnemyCount)
        {
            Spawn();
        }
        //GameObject.FindGameObjectWithTag("Player").transform.rotation = new Quaternion(0.0001f, Mathf.LerpAngle(0, 360, 600f), 0, 0);
    }
    void Spawn()
    {
        float r = Random.Range(1, 100);
        float xxx = 1000;
        GameObject obj = ObjectToSpawn;

        foreach (SpawnRate item in Spawns)
        {
            print(item.SzansaMin + " | " + item.SzansaMax);
            if (r>=item.SzansaMin&&r<=item.SzansaMax)
            {
                obj = item.@object;
            }
        }

        for (int i = 0; i < Mathf.FloorToInt(/*vertices.Length*/ (xSize / 10f)); i++)
        {
            int C = Mathf.FloorToInt(Random.Range(1, vertices.Length));
            GameObject g = Instantiate(obj, vertices[C] - new Vector3(xSize / 2, -2f, zSize / 2), Quaternion.identity, gameObject.transform);
            g.name += " " + C;
            g.SetActive(true);
            Debug.Log(r + " " + xxx);
        }

        int cache = fala % 5;
        if (cache==0)
        {
            int C = Mathf.FloorToInt(Random.Range(1, vertices.Length));
            GameObject g = Instantiate(Shooter, vertices[C] - new Vector3(xSize / 2, -2f, zSize / 2), Quaternion.identity, gameObject.transform);
            g.name += " " + C;
            g.SetActive(true);
            Debug.Log("Special");
        }
        fala++;

        print(fala + "-Fala  Reszta po podzieleniu na 5-" + /*fala % 5*/cache);
    }
    IEnumerator CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float rX = Random.Range(0.5f, 5);
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * rX, z * rX) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        #region Defining UVs

        uvs = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }

        #endregion

        int vert = 0, tris = 0;
        triangles = new int[xSize * zSize * 6];
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
                yield return new WaitForSeconds(secondsTimeWait);
            }
            vert++;
        }

        


        surface.BuildNavMesh();
        List<int> Data = new List<int>();
        for (int i = 0; i < Mathf.FloorToInt(/*vertices.Length*/ (xSize / 10f)); i++)
        {
            int C = Mathf.FloorToInt(Random.Range(1, vertices.Length));
            int[] datas = Data.ToArray();
            bool CanBeBuild = true;
            foreach (int item in datas)
            {
                if (C == item)
                {
                    CanBeBuild = false;
                    break;
                }
            }
            if (CanBeBuild)
            {
                try
                {
                    GameObject g = Instantiate(ObjectToSpawn, vertices[C] - new Vector3(xSize / 2, -2f, zSize / 2), Quaternion.identity, gameObject.transform);
                    g.name += " " + C;
                    g.SetActive(true);
                    Data.Add(C);
                }
                catch
                {
                    Debug.Log(C + " / " + vertices.Length);
                }
            }
            yield return new WaitForSeconds(.5f);
        }
        MCamera.position = new Vector3(0, 5.7f, 0);
        MCamera.rotation = Quaternion.identity;
        Ready = true;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        EnemyCount = Data.Count;

    }

    public void StartTest()
    {
        mesh = new Mesh();
        mesh.name = "CustomMesh";
        GetComponent<MeshFilter>().mesh = mesh;


        #region Create Shape

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float rX = Random.Range(0.5f, 5);
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * rX, z * rX) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        int vert = 0, tris = 0;
        triangles = new int[xSize * zSize * 6];
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
        List<int> Data = new List<int>();
        for (int i = 0; i < Mathf.FloorToInt(vertices.Length / (xSize * 1.5f)); i++)
        {
            int C = Mathf.FloorToInt(Random.Range(xSize + 1, vertices.Length - 6));
            int[] datas = Data.ToArray();
            bool CanBeBuild = true;
            foreach (int item in datas)
            {
                if (C == item)
                {
                    CanBeBuild = false;
                    break;
                }
            }
            if (CanBeBuild)
            {
                Instantiate(ObjectToSpawn, vertices[C], Quaternion.identity, gameObject.transform).SetActive(true);
                Data.Add(C);
            }
        }
        #endregion
        UpdateMesh();
    }
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }

    public Vector3[] GetFirstAndLastPoint()
    {
        Vector3[] v = new Vector3[2];

        v[0] = vertices[0];
        v[1] = vertices[vertices.Length - 1];

        return v;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        foreach (Vector3 item in vertices)
        {
            Gizmos.DrawSphere(item, 0.1f);
        }
    }
}
