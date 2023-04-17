
using System.Collections.Generic;
using Unity.Collections;
using UnityEditorInternal;

namespace VargheseJoshua.Lab6
{
    public static class Globals
    {
        public static Dictionary<superpositions, (superpositions L, superpositions R)> Rules = new Dictionary<superpositions, (superpositions L, superpositions R)>()
        {
            {superpositions.forest, (superpositions.land, superpositions.land) },
            {superpositions.land, (superpositions.forest, superpositions.sand) },
            {superpositions.sand, (superpositions.land, superpositions.water) },
            {superpositions.water, (superpositions.sand, superpositions.ocean) },
            {superpositions.ocean, (superpositions.water, superpositions.water) },
        };
        /*public static string[] Super = new string[]
        {
            "forest",
            "land",
            "sand",
            "water",
            "ocean"
        };*/
        public enum superpositions
        {
            forest,
            land,
            sand,
            water,
            ocean,
            super
        };

        public static System.Random RNG = new System.Random();
    }
}
