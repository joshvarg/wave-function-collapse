using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace VargheseJoshua.Lab6
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] int rows = 2;
        [SerializeField] int columns = 2;
        public Cell[,] cellgrid;
        // Start is called before the first frame update
        void Start()
        {
            cellgrid = new Cell[rows, columns];
            // Initialize left to right, row by row.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    c.transform.position = new Vector3(i, 0, j);
                    var tile = c.AddComponent<Cell>();
                    cellgrid[i, j] = tile;
                    //cellgrid[i,j] = new Cell((i,j));
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
