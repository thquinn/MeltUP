using System.Collections.Generic;
using UnityEngine;

namespace Assets.Structs {
    public class Gene {
        public static Dictionary<GeneID, Color> COLOR_LOOKUP = new Dictionary<GeneID, Color>() {
            { GeneID.Speed, new Color(1, .8f, .8f) },
            { GeneID.DoubleJump, new Color(.85f, .95f, 1) },
            { GeneID.LeapOfFaith, new Color(.825f, .925f, 1) },
            { GeneID.JumpControl, new Color(.95f, 1, .9f) },
            { GeneID.AirControl, new Color(.95f, .85f, 1) },
            { GeneID.Junk, new Color(.5f, .5f, .5f) },
            { GeneID.DamagedJunk, new Color(.55f, .55f, .55f) },
            { GeneID.Armor, new Color(.75f, .75f, .75f) },
            { GeneID.Respawn, new Color(1, 1, 1) },
            { GeneID.Glide, new Color(.825f, 1, .95f) },
            { GeneID.Blink, new Color(1, .75f, 1) },
        };
        public static Dictionary<GeneID, string> NAME_LOOKUP = new Dictionary<GeneID, string>() {
            { GeneID.Speed, "Speed" },
            { GeneID.DoubleJump, "Double Jump" },
            { GeneID.LeapOfFaith, "Leap of Faith" },
            { GeneID.JumpControl, "Jump Control" },
            { GeneID.AirControl, "Air Control" },
            { GeneID.Junk, "Junk" },
            { GeneID.DamagedJunk, "Junk (Damaged)" },
            { GeneID.Armor, "Armor" },
            { GeneID.Respawn, "Respawn" },
            { GeneID.Glide, "Glide" },
            { GeneID.Blink, "Blink" },
        };
        public static Dictionary<GeneID, string> DESCRIPTION_LOOKUP = new Dictionary<GeneID, string>() {
            { GeneID.Speed, "The more you have, the faster you move." },
            { GeneID.DoubleJump, "Gives you an extra jump." },
            { GeneID.LeapOfFaith, "50% chance to give you an extra jump." },
            { GeneID.JumpControl, "Allows you to control the height of your jump by holding the button." },
            { GeneID.AirControl, "Improves your mid-air movement." },
            { GeneID.Junk, "Junk DNA, resilient against radiation." },
            { GeneID.DamagedJunk, "Junk DNA, resilient against radiation. Still hangin' on..." },
            { GeneID.Armor, "Genetic armor, likely to take the hit from radiation." },
            { GeneID.Respawn, "Falling off the edge of the world doesn't end the game." },
            { GeneID.Glide, "Hold X to glide." },
            { GeneID.Blink, "Press X to blink." },
        };
        public static Dictionary<GeneID, int[]> PRICE_LOOKUP = new Dictionary<GeneID, int[]>() {
            { GeneID.Speed, new int[]{ 7, 10 } },
            { GeneID.DoubleJump, new int[]{ 14, 18 } },
            { GeneID.LeapOfFaith, new int[]{ 9, 12 } },
            { GeneID.JumpControl, new int[]{ 6, 9 } },
            { GeneID.AirControl, new int[]{ 5, 8 } },
            { GeneID.Junk, new int[]{ 11, 15 } },
            { GeneID.Armor, new int[]{ 6, 9 } },
            { GeneID.Respawn, new int[]{ 6, 9 } },
            { GeneID.Glide, new int[]{ 7, 10 } },
            { GeneID.Blink, new int[]{ 13, 17 } },
        };

        public GeneID id;

        public Gene(GeneID id) {
            this.id = id;
        }
    }

    public enum GeneID {
        Speed, DoubleJump, LeapOfFaith, JumpControl, AirControl, Junk, DamagedJunk, Armor, Respawn, Glide, Blink
    }
}
