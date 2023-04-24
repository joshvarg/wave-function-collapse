using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] public string splist;
        [SerializeField] private ScriptManager sc;
        public List<Constraint> sp;

        public bool stable = false;
        public bool atEdge = true;

        public int entropy;

        private GameObject tile;

        public (int x, int z) position;

        public Cell north;
        public Cell south;
        public Cell west;
        public Cell east;

        [SerializeField]
        public Globals.superpositions currentState;
        //void Awake()
        public void Init()
        {
            entropy = Globals.MAX_STATES;
            stable = false;
            sp = new List<Constraint>(Globals.MAX_STATES);
            currentState = Globals.superpositions.super;
            InitializeConstraints();
            //Debug.Log("spcount"+ sp.Count);
            tile = this.gameObject;
            tile.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            sc = GameObject.Find("Manager").GetComponent<ScriptManager>();
        }

        public void Collapse()
        {
            int rand = Globals.RNG.Next(0, sp.Count);
            //Debug.Log(rand);
            //Debug.Log(sp.ToCommaSeparatedString());
            currentState = sp[rand].state;
            var x = sp[rand];
            //entropy = 6;
            sp.Clear();
            sp.Add(x);

            stable = true;

            tile.GetComponent<Renderer>().material.SetColor("_Color", Globals.Colors[currentState]);
            sc.UpdateConstraints(this);
        }


        private void InitializeConstraints()
        {
            for(int i = 0; i < Globals.MAX_STATES; i++)
            {
                sp.Add(new Constraint((Globals.superpositions)i));
            }
        }
        public void ResetNeighbors()
        {
            if (!north.IsUnityNull())
            {
                north.Init();
                if (!north.north.IsUnityNull())
                {
                    north.north.Init();
                }
            }
            if (!south.IsUnityNull())
            {
                south.Init();
                if (!south.south.IsUnityNull())
                {
                    south.south.Init();
                }
            }
            if (!east.IsUnityNull())
            {
                east.Init();
                if (!east.east.IsUnityNull())
                {
                    east.east.Init();
                }
            }
            if (!west.IsUnityNull())
            {
                west.Init();
                if (!west.west.IsUnityNull())
                {
                    west.west.Init();
                }
            }
        }
        public int StableNeighbors()
        {
            int num = 0;
            if (!north.IsUnityNull() && north.stable)
            {
                num++;
            }
            if (!south.IsUnityNull() && south.stable)
            {
                num++;
            }
            if (!east.IsUnityNull() && east.stable)
            {
                num++;
            }
            if (!west.IsUnityNull() && west.stable)
            {
                num++;
            }
            return num;
        }
        private void UpdateEdge()
        {
            bool temp = true;

            if (!north.IsUnityNull())
            {
                temp = temp && (north.sp.Count < Globals.MAX_STATES);
            }
            if (!south.IsUnityNull())
            {
                temp = temp && (south.sp.Count < Globals.MAX_STATES);
            }
            if (!east.IsUnityNull())
            {
                temp = temp && (east.sp.Count < Globals.MAX_STATES);
            }
            if (!west.IsUnityNull())
            {
                temp = temp && (west.sp.Count < Globals.MAX_STATES);
            }


            atEdge = temp;
        }

        /*void Update()
        {
            UpdateEdge();
            Debug.Log(position + "at edge?: "+ atEdge);
            if (atEdge)
            {
                Debug.Log("here");
                sc.UpdateConstraints(this);
                sc.AddToPool(this);
            }
        }*/
    }
}
