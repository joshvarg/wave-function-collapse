using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Constraint
    {
        public Globals.superpositions state;
        public Constraint(Globals.superpositions n) {
            state = n;
        }

        public override string ToString()
        {
            return state.ToString();
        }
    }
}
