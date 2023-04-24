using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace VargheseJoshua.Lab6
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] public int rows = 3;
        [SerializeField] public int columns = 2;
        private Transform gridobject;
        public Cell[,] cellgrid;
        // Start is called before the first frame update
        //void Start()
        public void Init(Transform gO)
        {
            gridobject = gO;
            cellgrid = new Cell[columns, rows];
            // Initialize left to right, row by row.
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    c.name = "x:"+i + ", " + "z:"+j;
                    c.transform.SetParent(gridobject);
                    c.transform.position = new Vector3(i, 0, j);
                    var tile = c.AddComponent<Cell>();
                    tile.position = (i, j);
                    tile.Init();
                    cellgrid[i, j] = tile;
                }
            }
        }
    }
}
