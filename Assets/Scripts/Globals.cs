using System.Collections.Generic;

namespace VargheseJoshua.Lab6
{
    public static class Globals
    {
        public static Dictionary<superpositions, 
            (superpositions L, superpositions R)> Rules 
            = new Dictionary<superpositions, 
                (superpositions L, superpositions R)>()
        {
            {superpositions.forest, 
                    (superpositions.grass, superpositions.grass) },
            {superpositions.grass, 
                    (superpositions.forest, superpositions.land) },
            {superpositions.land, 
                    (superpositions.grass, superpositions.sand) },
            {superpositions.sand, 
                    (superpositions.land, superpositions.water) },
            {superpositions.water, 
                    (superpositions.sand, superpositions.ocean) },
            {superpositions.ocean, 
                    (superpositions.water, superpositions.deepocean) },
            {superpositions.deepocean, 
                    (superpositions.ocean, superpositions.ocean) },
        };
        public enum superpositions
        {
            forest,
            grass,
            land,
            sand,
            water,
            ocean,
            deepocean,
            super
        };

        public const int MAX_STATES = 7;

        public static System.Random RNG = new System.Random();

        public const int RESET_DEPTH = 7;
    }
}
