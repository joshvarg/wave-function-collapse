
using System.Collections.Generic;
using Unity.Collections;
using UnityEditorInternal;
using UnityEngine;

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

        public static Dictionary<superpositions, Color> Colors = new Dictionary<superpositions, Color>()
        {
            {superpositions.forest, new Color(55f/255f, 79f/255f, 47f/255f) }, //dark green
            {superpositions.land, new Color(173f/255f, 223f/255f, 179f/255f) }, //light green
            {superpositions.sand, new Color(252f / 255f, 210f / 255f, 153f / 255f) }, //light orange
            {superpositions.water, new Color(5f / 255f, 195f / 255f, 221f / 255f) }, //light blue
            {superpositions.ocean, new Color(0f / 255f, 94f / 255f, 184f / 255f) } //dark blue
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
