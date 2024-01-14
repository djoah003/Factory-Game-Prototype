using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]


public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 gridOffset;

    [Header("Terrain Settings")]
    [SerializeField] Material terrainMaterial;
    [SerializeField] Material edgeMaterial;
    [SerializeField] float waterLevel = .3f;
    [SerializeField] float scale = .1f; // noise scale

    private Grid<bool> grid; // when creating a grid define <variable type> !!!

    [Header("Debug Tools")]
    [SerializeField] bool spawnCollider = false;
    [SerializeField] GameObject gridCollider;

    private void Start()
    {
        // NOISE MAP
        float[,] noiseMap = new float[gridWidth, gridHeight]; // create array
        float xOffset = UnityEngine.Random.Range(-10000f, 10000f); // generate random offset
        float zOffset = UnityEngine.Random.Range(-10000f, 10000f);

        for (int x = 0; x < gridWidth; x++) { // cycle through to randomize noise
            for (int z = 0; z < gridHeight; z++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, z * scale + zOffset); // randomize noise value
                noiseMap[x, z] = noiseValue; // set noise value
            }
        }

        // noise falloff yipeee
        float[,] falloffMap = new float[gridWidth, gridHeight]; // create array
        for (int x = 0; x < gridWidth; x++) { // cycle through to noise falloff
            for (int z = 0; z < gridHeight; z++)
            {
                float xv = x / (float)gridWidth * 2 - 1; // I have no idea what's going on in here
                float zv = z / (float)gridHeight * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(zv));
                falloffMap[x, z] = Mathf.Pow(v, 5f) / (Mathf.Pow(v, 5f) + Mathf.Pow(2.2f - 2.2f * v, 5f));
            }
        }

        // CONSTRUCT GRID
        grid = new Grid<bool>(gridWidth, gridHeight, cellSize, gridOffset); // when creating a grid define <variable type> !!!

        // Set WATER
        for (int x = 0; x < gridWidth; x++) { // cycle through to set water
            for (int z = 0; z < gridHeight; z++)
            {
                float noiseValue = noiseMap[x, z]; // get noise value from noise array
                noiseValue -= falloffMap[x, z]; // check for the falloff
                if (noiseValue < waterLevel)
                {
                    grid.SetValue(x, z, false); // if noise value is below water level it is water
                }
                else
                {
                    grid.SetValue(x, z, true); // if noise value is above water level it is ground
                }
            }
        }

        DrawTerrainMesh();
        DrawEdgeMesh();
        DrawTextures();

        // Debug collider
        if (spawnCollider)
        {
            GameObject newObject = Instantiate(gridCollider, gridOffset, Quaternion.identity);
            newObject.transform.localScale = new Vector3(gridWidth, 1, gridHeight);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GetMouseWorldPosition(), true);
        }
    }

    void DrawTerrainMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        // draw terrain where water is false
        for (int x = 0; x < gridWidth; x++) {
            for(int z = 0; z < gridHeight; z++) {
                if (grid.GetValue(x, z) == true) // draw land where there is no water
                {
                    // create cell vertices
                    Vector3 a = new Vector3(x       * cellSize, 0, (z + 1) * cellSize);
                    Vector3 b = new Vector3((x + 1) * cellSize, 0, (z + 1) * cellSize);
                    Vector3 c = new Vector3(x       * cellSize, 0,  z      * cellSize);
                    Vector3 d = new Vector3((x + 1) * cellSize, 0,  z      * cellSize);

                    // create UVs
                    Vector2 uvA = new Vector2( x      / (float)gridWidth,  z      / (float)gridHeight);
                    Vector2 uvB = new Vector2((x + 1) / (float)gridWidth,  z      / (float)gridHeight);
                    Vector2 uvC = new Vector2( x      / (float)gridWidth, (z + 1) / (float)gridHeight);
                    Vector2 uvD = new Vector2((x + 1) / (float)gridWidth, (z + 1) / (float)gridHeight);

                    Vector3[] v = new Vector3[] { a, b, c, b, d, c }; // get vertecies for square (two triangles -> 1:abc 2:bcd)
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC }; // get uvs
                    for (int i = 0; i < 6; i++)
                    {
                        vertices.Add(v[i]); // create verticies
                        triangles.Add(triangles.Count); // create triangles
                        uvs.Add(uv[i]); // create UVs
                    }
                }
            }
        }

        // assign and calculate normals
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        // mesh filter data and renderer data
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    void DrawEdgeMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for(int x = 0; x < gridWidth; x++) {
            for (int z = 0; z < gridHeight; z++)
            {
                if (grid.GetValue(x, z) == false) { // ignore water
                    if(x > 0 && grid.GetValue(x - 1, z) == true) // left
                    {
                        Vector3 a = new Vector3( x      * cellSize,  0, (z + 1) * cellSize);
                        Vector3 b = new Vector3( x      * cellSize,  0,  z      * cellSize);
                        Vector3 c = new Vector3( x      * cellSize, cellSize / -2, (z + 1) * cellSize);
                        Vector3 d = new Vector3( x      * cellSize, cellSize / -2,  z      * cellSize);
                        Vector3[] v = new Vector3[]{a, c, b, b, c, d}; // switched abc to acb because the tri was drawn counter clockwise

                        for(int i = 0; i < 6; i++)
                        {
                                vertices.Add(v[i]);
                                triangles.Add(triangles.Count);
                        }
                    }

                    if (x > 0 && grid.GetValue(x + 1, z) == true) // right
                    {
                        Vector3 a = new Vector3((x + 1) * cellSize,  0,  z      * cellSize);
                        Vector3 b = new Vector3((x + 1) * cellSize,  0, (z + 1) * cellSize);
                        Vector3 c = new Vector3((x + 1) * cellSize, cellSize / -2,  z      * cellSize);
                        Vector3 d = new Vector3((x + 1) * cellSize, cellSize / -2, (z + 1) * cellSize);
                        Vector3[] v = new Vector3[] { a, c, b, b, c, d }; // switched abc to acb because the tri was drawn counter clockwise

                        for (int i = 0; i < 6; i++)
                        {
                            vertices.Add(v[i]);
                            triangles.Add(triangles.Count);
                        }
                    }

                    if (z > 0 && grid.GetValue(x, z - 1) == true) // down
                    {
                        Vector3 a = new Vector3( x      * cellSize,  0,  z      * cellSize);
                        Vector3 b = new Vector3((x + 1) * cellSize,  0,  z      * cellSize);
                        Vector3 c = new Vector3( x      * cellSize, cellSize / -2,  z      * cellSize);
                        Vector3 d = new Vector3((x + 1) * cellSize, cellSize / -2,  z      * cellSize);
                        Vector3[] v = new Vector3[] { a, c, b, b, c, d }; // switched abc to acb because the tri was drawn counter clockwise

                        for (int i = 0; i < 6; i++)
                        {
                            vertices.Add(v[i]);
                            triangles.Add(triangles.Count);
                        }
                    }

                    if (z > 0 && grid.GetValue(x, z + 1) == true) // up
                    {
                        Vector3 a = new Vector3((x + 1) * cellSize,  0, (z + 1) * cellSize);
                        Vector3 b = new Vector3( x      * cellSize,  0, (z + 1) * cellSize);
                        Vector3 c = new Vector3((x +1 ) * cellSize, cellSize / -2, (z + 1) * cellSize);
                        Vector3 d = new Vector3( x      * cellSize, cellSize / -2, (z + 1) * cellSize);
                        Vector3[] v = new Vector3[] { a, c, b, b, c, d }; // switched abc to acb because the tri was drawn counter clockwise

                        for (int i = 0; i < 6; i++)
                        {
                            vertices.Add(v[i]);
                            triangles.Add(triangles.Count);
                        }
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObject = new GameObject("Edge");
        edgeObject.transform.SetParent(transform);

        MeshFilter meshFilter = edgeObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObject.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;
    }

    void DrawTextures()
    {
        Texture2D texture = new Texture2D(gridWidth, gridHeight);
        Color[] colorMap = new Color[gridWidth * gridHeight];

        for(int x = 0; x < gridWidth; x++) {
            for(int y = 0; y < gridHeight; y++)
            {
                if(grid.GetValue(x, y) == false)
                {
                    colorMap[y * gridWidth + x] = Color.blue;
                }
                else
                {
                    colorMap[y * gridHeight + x] = new Color(153 / 255f, 191 / 255f, 115 / 255f);
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(colorMap);
            texture.Apply();

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = terrainMaterial;
            meshRenderer.material.mainTexture = texture;
        }
    }

    private static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Collider")))
        {
            return hit.point;
        }
        return new Vector3(-1f,0,-1f);
    }
}