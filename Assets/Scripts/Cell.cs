using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Cell : MonoBehaviour
    {
        public List<Constraint> Superpositions = new List<Constraint>(Globals.Super.Length);
        public (float x, float z) position;
        // Start is called before the first frame update
        public Cell((float x, float z) position)
        {
            for (int i = 0; i < Superpositions.Count; i++)
            {
                Superpositions[i].isPossible = true;
                Superpositions[i].Name = Globals.Super[i];
            }

            this.position = position;
            var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
            c.transform.position = new Vector3(position.x, 0, position.z);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
