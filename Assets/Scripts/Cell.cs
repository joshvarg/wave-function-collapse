using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] public string splist;
        public List<Constraint> sp = new List<Constraint>(Globals.MAX_STATES);

        public bool stable = false;

        public int entropy = Globals.MAX_STATES;

        private GameObject tile;

        public (int x, int z) position;

        public Cell north;
        public Cell south;
        public Cell west;
        public Cell east;

        [SerializeField]
        public Globals.superpositions currentState = Globals.superpositions.super;
        //void Awake()
        public void Init()
        {
            InitializeConstraints();
            //Debug.Log("spcount"+ sp.Count);
            tile = this.gameObject;
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
        }


        private void InitializeConstraints()
        {
            for(int i = 0; i < Globals.MAX_STATES; i++)
            {
                sp.Add(new Constraint((Globals.superpositions)i));
            }
        }
    }
}
