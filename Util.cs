using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;
using monogame.States;

namespace monogame
{

    public static class Util
    {

        private static string gameDataLocation = "data.json";
        private static string skinDataLocation = "SkinData.json";

        public static void LoadGameData()
        {
            GameData data = new GameData();
            using (StreamReader r = new StreamReader(gameDataLocation))
            {
                string json = r.ReadToEnd();
                data = JsonSerializer.Deserialize<GameData>(json);
            }

            if (data.HighScore > GameState.HiScore)
            {
                GameState.HiScore = data.HighScore;
            }

            if (data.Coins > GameState.Coins)
            {
                GameState.Coins = data.Coins;
            }
        }

        public static void SaveGameData(int currentScore, int currentCoins)
        {
            GameData data = new GameData();

            using (StreamReader r = new StreamReader(gameDataLocation))
            {
                string json = r.ReadToEnd();
                data = JsonSerializer.Deserialize<GameData>(json);
            }

            if (currentScore > data.HighScore)
            {
                data.HighScore = currentScore;
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(gameDataLocation, json);
            }

            if (currentCoins > data.Coins)
            {
                data.Coins = currentCoins;
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(gameDataLocation, json);
            }
        }

        public static void LoadSkinData(ContentManager content)
        {
            List<SkinData> skinData = new List<SkinData>();
            SkinsState.Skins = new List<Skin>();

            using (StreamReader r = new StreamReader(skinDataLocation))
            {
                string json = r.ReadToEnd();
                skinData = JsonSerializer.Deserialize<List<SkinData>>(json);
                for (int i = 0; i < skinData.Count; i++)
                {
                    Skin skin = new Skin(content, skinData[i].Name);
                    skin.Name = skinData[i].Name;
                    skin.Selected = skinData[i].Selected;
                    SkinsState.Skins.Add(skin);
                }
            }
        }

        public static void SaveSkinData()
        {
            if (SkinsState.Skins is not null)
            {
                List<SkinData> skinData = new List<SkinData>();
                for (int i = 0; i < SkinsState.Skins.Count; i++)
                {
                    SkinData skin = new SkinData();
                    skin.Name = SkinsState.Skins[i].Name;
                    skin.Selected = SkinsState.Skins[i].Selected;
                    skinData.Add(skin);
                }
                string json = JsonSerializer.Serialize(skinData);
                Console.WriteLine("Save skins.");
                File.WriteAllText(skinDataLocation, json);
            }
        }
    }
    
}