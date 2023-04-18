
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditorInternal;
using UnityEngine;

namespace VargheseJoshua.Lab6
{
    public static class Globals
    {
        public static Dictionary<superpositions, (superpositions L, superpositions R)> Rules = new Dictionary<superpositions, (superpositions L, superpositions R)>()
        {
            {superpositions.forest, (superpositions.grass, superpositions.grass) },
            {superpositions.grass, (superpositions.forest, superpositions.land) },
            {superpositions.land, (superpositions.grass, superpositions.sand) },
            {superpositions.sand, (superpositions.land, superpositions.water) },
            {superpositions.water, (superpositions.sand, superpositions.ocean) },
            {superpositions.ocean, (superpositions.water, superpositions.deepocean) },
            {superpositions.deepocean, (superpositions.ocean, superpositions.ocean) },
            //{superpositions.stone, (superpositions.stone, superpositions.stone) },
        };

        public static Dictionary<superpositions, Color32> Colors = new Dictionary<superpositions, Color32>()
        {
            {superpositions.forest, new Color32(0x06, 0x27, 0x26, 0xff) }, //dark green
            {superpositions.grass, new Color32(0x05, 0x43, 0x23, 0xff) },
            {superpositions.land, new Color32(0x04, 0x5f, 0x35, 0xff) }, //light green
            {superpositions.sand, new Color32(0xcd, 0xc9, 0xaa, 0xff) }, //light orange
            {superpositions.water, new Color32(0x4a, 0x6f, 0xa5, 0xff) }, //light blue
            {superpositions.deepocean, new Color32(0x28, 0x4b, 0x66, 0xff) }, //dark blue
            {superpositions.ocean, new Color32(0x39, 0x5d, 0x86, 0xff) },
            //{superpositions.stone, new Color32(0xdd, 0xdd, 0xdd, 0xff) },
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
            grass,
            land,
            sand,
            water,
            ocean,
            deepocean,
            //stone,
            super
        };

        public const int MAX_STATES = 7;

        public static System.Random RNG = new System.Random();
    }
}
