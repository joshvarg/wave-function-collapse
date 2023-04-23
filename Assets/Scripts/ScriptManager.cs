using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace VargheseJoshua.Lab6
{
    public class ScriptManager : MonoBehaviour
    {
        [SerializeField] Grid grid;
        [SerializeField] Transform cam;
        [SerializeField] Transform gridObject;
        public Cell mostRecentCollapse;
        public (int, int) contraposition = (0,0);
        public bool restart = false;
        private bool done = false;
        // Start is called before the first frame update
        private void Start()
        {
            Initialize(contraposition);
        }
        private void Initialize((int,int) initialcollapse)
        {
            grid.Init(gridObject);
            cam.position = new Vector3(grid.columns / 2, (grid.columns+grid.rows)/2, grid.rows / 2);
            UpdateNeighbors(grid);
            grid.cellgrid[initialcollapse.Item1, initialcollapse.Item2].Collapse();
            mostRecentCollapse = grid.cellgrid[0,0];
        }

        private void Update()
        {
            //Debug.Log(done);
            if (!done)
            {

                foreach (var c in grid.cellgrid)
                {
                    /*if(mostRecentCollapse.position == (30, 5) && c.position==(30,5)) { 
                        Debug.Log("STOP"); 
                    }*/
                    //Debug.Log(c.position);
                    UpdateConstraints(c);
                }
                if (restart)
                {
                    Destroy(gridObject.gameObject);
                    gridObject = new GameObject("GridObject").transform;
                    Initialize(contraposition);
                    restart = false;
                }
                else
                {
                    done = CollapseLowestEntropy();

                }
            }
        }

        private void UpdateNeighbors(Grid g)
        {
            foreach (Cell c in g.cellgrid)
            {
                if (c.position.x + 1 < grid.columns)
                {
                    c.east = grid.cellgrid[c.position.x + 1, c.position.z];
                }
                if (c.position.x - 1 >= 0)
                {
                    c.west = grid.cellgrid[c.position.x - 1, c.position.z];
                }
                if (c.position.z + 1 < grid.rows)
                {
                    c.north = grid.cellgrid[c.position.x, c.position.z + 1];
                }
                if (c.position.z - 1 >= 0)
                {
                    c.south = grid.cellgrid[c.position.x, c.position.z - 1];
                }
            }
        }

        private List<Constraint> FindAllowedConstraints(Cell host, Cell neighbor)
        {
            var update = new List<Constraint>();
            foreach (Constraint con in host.sp)
            {
                foreach (Constraint ncon in neighbor.sp)
                {
                    if (Globals.Rules[con.state].L == ncon.state //equals left
                        || Globals.Rules[con.state].R == ncon.state// OR right
                        || con.state == ncon.state)// OR the same
                    {
                        //Debug.Log(Globals.Rules[con.state]);
                        //Debug.Log("cell state: "+con.state+"  north state: "+ncon.state);
                        update.Add(ncon);
                    }
                }
            }
            if(update.Count <= 0)
            {
                Debug.Log("CONTRADICTION FOUND at "+neighbor.position+" restarting!");
                contraposition = neighbor.position;
                restart = true;
            }
            return update.Distinct().ToList();
        }
        private void UpdateConstraints(Cell c)
        {
            if (c.north != null)
            {
                c.north.sp = FindAllowedConstraints(c, c.north);
                
                //Debug.Log("n"+c.north.sp.ToCommaSeparatedString());
            }
            if (c.south != null)
            {
                c.south.sp = FindAllowedConstraints(c, c.south);
                //Debug.Log("s"+c.south.sp.ToCommaSeparatedString());
            }
            if (c.east != null)
            {
                c.east.sp = FindAllowedConstraints(c, c.east);
                //Debug.Log("e"+c.east.sp.ToCommaSeparatedString());
            }
            if (c.west != null)
            {
                c.west.sp = FindAllowedConstraints(c, c.west);
                //Debug.Log("w"+c.west.sp.ToCommaSeparatedString());
            }
            c.splist = c.sp.ToCommaSeparatedString();
            c.entropy = c.sp.Count;
            //Debug.Log("entropy: " + c.entropy);

            //Debug.Log(north.sp.ToCommaSeparatedString());
        }

        private bool CollapseLowestEntropy()
        {
            //Cell lowest = grid.cellgrid[Globals.RNG.Next(0, grid.rows), Globals.RNG.Next(0, grid.columns)];
            Cell lowest = grid.cellgrid[0, 0];
            int min = Globals.MAX_STATES;
            //int iteration = 0;
            bool finished = true;
            foreach(Cell c in grid.cellgrid)
            {
                if (c.stable)
                {
                    //Debug.Log(iteration +" " + finished);
                    //iteration++;
                    continue;
                } else if(c.entropy < min) {
                    finished = finished && c.stable;
                    min = c.entropy;
                    lowest = c;
                }
            }
            lowest.Collapse();
            mostRecentCollapse= lowest;
            //Debug.Log("end of collapse run" + finished);
            return finished;
        }
    }
}
