using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] int rows = 2;
        [SerializeField] int columns = 2;
        private Cell[,] cellgrid = new Cell[2,2];
        // Start is called before the first frame update
        void Start()
        {
            // Initialize left to right, row by row.
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    cellgrid[i,j] = new Cell((i,j));
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
