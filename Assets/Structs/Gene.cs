using System.Collections.Generic;
using UnityEngine;

namespace Assets.Structs {
    public class Gene {
        public static Dictionary<GeneID, Color> COLOR_LOOKUP = new Dictionary<GeneID, Color>() {
            { GeneID.Speed, new Color(1, .8f, .8f) },
            { GeneID.JumpPower, new Color(.75f, .65f, .9f) },
            { GeneID.DoubleJump, new Color(.85f, .95f, 1) },
            { GeneID.LeapOfFaith, new Color(.825f, .925f, 1) },
            { GeneID.JumpControl, new Color(.95f, 1, .9f) },
            { GeneID.AirControl, new Color(.95f, .85f, 1) },
            { GeneID.Junk, new Color(.5f, .5f, .5f) },
            { GeneID.DamagedJunk, new Color(.55f, .55f, .55f) },
            { GeneID.Armor, new Color(.75f, .75f, .75f) },
            { GeneID.UraniumBlock, new Color(0, .866f, .45f) },
            { GeneID.Respawn, new Color(1, 1, 1) },
            { GeneID.Glide, new Color(.825f, 1, .95f) },
            { GeneID.Blink, new Color(1, .75f, 1) },
            { GeneID.Restoration, new Color(1, .66f, .66f) },
        };
        public static Dictionary<GeneID, string> NAME_LOOKUP = new Dictionary<GeneID, string>() {
            { GeneID.Speed, "Speed" },
            { GeneID.JumpPower, "Jump Power" },
            { GeneID.DoubleJump, "Double Jump" },
            { GeneID.LeapOfFaith, "Leap of Faith" },
            { GeneID.JumpControl, "Jump Control" },
            { GeneID.AirControl, "Air Control" },
            { GeneID.Junk, "Junk" },
            { GeneID.DamagedJunk, "Junk (Damaged)" },
            { GeneID.Armor, "Armor" },
            { GeneID.UraniumBlock, "Alpha Absorption" },
            { GeneID.Respawn, "Respawn" },
            { GeneID.Glide, "Glide" },
            { GeneID.Blink, "Blink" },
            { GeneID.Restoration, "Restoration" },
        };
        public static Dictionary<GeneID, string> DESCRIPTION_LOOKUP = new Dictionary<GeneID, string>() {
            { GeneID.Speed, "The more you have, the faster you move." },
            { GeneID.JumpPower, "The more you have, the higher you jump." },
            { GeneID.DoubleJump, "Gives you an extra jump." },
            { GeneID.LeapOfFaith, "50% chance to give you an extra jump." },
            { GeneID.JumpControl, "Allows you to control the height of your jump by holding the button." },
            { GeneID.AirControl, "Improves your midair movement." },
            { GeneID.Junk, "Junk DNA, resilient against radiation." },
            { GeneID.DamagedJunk, "Junk DNA, resilient against radiation. Still hangin' on..." },
            { GeneID.Armor, "Genetic armor, likely to take the hit from radiation." },
            { GeneID.UraniumBlock, "Reduces irradiation from carried uranium." },
            { GeneID.Respawn, "Falling off the edge of the world doesn't kill you." },
            { GeneID.Glide, "Hold X to glide." },
            { GeneID.Blink, "Press X to blink." },
            { GeneID.Restoration, "Reduces irradiation whenever you reach a checkpoint." },
        };
        public static Dictionary<GeneID, int[]> PRICE_LOOKUP = new Dictionary<GeneID, int[]>() {
            { GeneID.Speed, new int[]{ 7, 10 } },
            { GeneID.JumpPower, new int[]{ 10, 14 } },
            { GeneID.DoubleJump, new int[]{ 14, 18 } },
            { GeneID.LeapOfFaith, new int[]{ 9, 12 } },
            { GeneID.JumpControl, new int[]{ 6, 9 } },
            { GeneID.AirControl, new int[]{ 5, 8 } },
            { GeneID.Junk, new int[]{ 9, 12 } },
            { GeneID.Armor, new int[]{ 6, 9 } },
            { GeneID.UraniumBlock, new int[]{ 8, 11 } },
            { GeneID.Respawn, new int[]{ 6, 9 } },
            { GeneID.Glide, new int[]{ 7, 10 } },
            { GeneID.Blink, new int[]{ 13, 17 } },
            { GeneID.Restoration, new int[]{ 8, 11 } },
        };
        public static Dictionary<GeneID, float> SHOP_WEIGHT_LOOKUP = new Dictionary<GeneID, float>() {
            { GeneID.Speed, 2f },
            { GeneID.JumpPower, .5f },
            { GeneID.DoubleJump, .5f },
            { GeneID.LeapOfFaith, .33f },
            { GeneID.JumpControl, 1f },
            { GeneID.AirControl, 2f },
            { GeneID.Junk, .66f },
            { GeneID.Armor, 1f },
            { GeneID.UraniumBlock, .25f },
            { GeneID.Respawn, .5f },
            { GeneID.Glide, .2f },
            { GeneID.Blink, .1f },
            { GeneID.Restoration, .4f },
        };

        public GeneID id;

        public Gene(GeneID id) {
            this.id = id;
        }
    }

    public enum GeneID {
        Speed, JumpPower, DoubleJump, LeapOfFaith, JumpControl, AirControl, Junk, DamagedJunk, Armor, UraniumBlock, Respawn, Glide, Blink, Restoration
    }
}
