using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VargheseJoshua.Lab6
{
    public class ScriptManager : MonoBehaviour
    {
        [SerializeField] Grid grid;
        private Cell mostRecentCollapse;
        // Start is called before the first frame update
        void Start()
        {
            grid.Init();
            grid.cellgrid[0, 0].Collapse();
            mostRecentCollapse = grid.cellgrid[0, 0];
            UpdateNeighbors(mostRecentCollapse);

        }

        private void UpdateNeighbors(Cell c)
        {
            if (c.position.x + 1 < grid.columns)
            {
                c.east = grid.cellgrid[c.position.x + 1, c.position.z];
            }
            if (c.position.x - 1 > 0)
            {
                c.west = grid.cellgrid[c.position.x - 1, c.position.z];
            }
            if (c.position.z + 1 < grid.rows)
            {
                c.north = grid.cellgrid[c.position.x, c.position.z + 1];
            }
            if (c.position.z - 1 > 0)
            {
                c.south = grid.cellgrid[c.position.x, c.position.z - 1];
            }
        }

        private void UpdateConstraints(Cell c)
        {
            foreach(Constraint con in c.sp)
            {

            }
        }
    }
}
