using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject> // generics, grid will be created with any variables
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged; // fired whenever cell value changes
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }


    public bool showDebug = true;
    
    public int width; // first dimension of array
    public int height; // second dimension of array
    private float cellSize; // cell size
    private Vector3 originPosition; // grid offset

    private TGridObject[,] gridArray; // main 2D array


    // CONSTRUCTOR  
    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width,height]; // create array with the number of elements of width, each of which has the number of elements of height


        // DEBUG ARRAY
        if (showDebug)
        {
            TextMesh[,] debugArray = new TextMesh[width, height]; // create debug array

            // draw grid
            for (int x = 0; x < gridArray.GetLength(0); x++) {     // cycle through 1st dim of the array (width elements)
                for (int z = 0; z < gridArray.GetLength(1); z++) { // cycle through 2nd dim of the array (lenght elements)
                    debugArray[x, z] = CreateWorldText(gridArray[x, z].ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter); // write cell value
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f); // draw cells
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f); //
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f); // draw top and left side of the grid
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);  //

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                debugArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z].ToString();};
        }
    }

    // converet grid pos to world pos
    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0 ,z) * cellSize + originPosition;
    }

    // convert world pos to grid pos
    private void GetGridPosition(Vector3 worlPosition, out int x, out int z) // why not a Vector2Int?
    {
        x = Mathf.FloorToInt((worlPosition - originPosition).x / cellSize); // round int down to cell size for ex.: 10 -> cell 0, 11 -> cell 1
        z = Mathf.FloorToInt((worlPosition - originPosition).z / cellSize);
    }

    // set cell value
    public void SetValue(int x, int z , TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height) // ignoring invalid values when looking for cell
        {
            gridArray[x, z] = value; // set value of cell
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }
    }

    // set value from world pos to grid pos
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, z;
        GetGridPosition(worldPosition, out x, out z); //get cell pos
        SetValue(x, z, value); // set cell value
    }

    // get value from cell
    public TGridObject GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height) // ignoring invalid values when looking for cell
        {
            return gridArray[x, z]; // return grid pos
        }
        else
        {
            return default(TGridObject); // return invalid value
        }
    }

    // get value from cell from world pos
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetGridPosition(worldPosition, out x, out z); // world pos to grid pos
        return GetValue(x, z); // return value from cell
    }


    // code monkey utils
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
