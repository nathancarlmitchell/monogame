using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;
using monogame.States;

namespace monogame
{
    public static class Util
    {
        private static string gameDataLocation = "GameData.json";
        private static string skinDataLocation = "SkinData.json";
        private static readonly string defaultGameData = @"{""HighScore"":0,""Coins"":0}";
        private static readonly string defaultSkinData = @"[{""Name"":""anim_idle_default"",""Selected"":true,""Locked"":false,""Cost"":0,""Frames"":2,""FPS"":2},{""Name"":""anim_idle_pink"",""Selected"":false,""Locked"":true,""Cost"":3,""Frames"":2,""FPS"":2},{""Name"":""anim_idle_clear"",""Selected"":false,""Locked"":true,""Cost"":5,""Frames"":2,""FPS"":2},{""Name"":""anim_idle_rubiks"",""Selected"":false,""Locked"":true,""Cost"":10,""Frames"":4,""FPS"":12},{""Name"":""anim_idle_companion"",""Selected"":false,""Locked"":true,""Cost"":15,""Frames"":4,""FPS"":8},{""Name"":""anim_idle_dice"",""Selected"":false,""Locked"":true,""Cost"":15,""Frames"":6,""FPS"":12},{""Name"":""anim_idle_locked"",""Selected"":false,""Locked"":true,""Cost"":25,""Frames"":2,""FPS"":2},{""Name"":""anim_idle_tv"",""Selected"":false,""Locked"":true,""Cost"":25,""Frames"":4,""FPS"":16}]";
        private static readonly string androidContext = "/data/data/monogame.monogame/files/";

        public static void CheckOS()
        {
            if (OperatingSystem.IsAndroid())
            {
                Console.WriteLine("Device running Android.");
                gameDataLocation = androidContext + gameDataLocation;
                skinDataLocation = androidContext + skinDataLocation;
            }
        }

        public static void LoadGameData()
        {
            GameData gameData = new();

            try
            {
                using (StreamReader r = new StreamReader(gameDataLocation))
                {
                    Console.WriteLine(gameDataLocation + ": reading data.");
                    string json = r.ReadToEnd();
                    gameData = JsonSerializer.Deserialize<GameData>(json);
                }

                GameState.HiScore = gameData.HighScore;
                GameState.Coins = gameData.Coins;
            }

            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine(gameDataLocation+": file not found.");
                using (FileStream fs = File.Create(gameDataLocation))
                {
                    Console.WriteLine(gameDataLocation + ": file created.");
                    byte[] data = new UTF8Encoding(true).GetBytes(defaultGameData);
                    fs.Write(data, 0, data.Length);
                }
            }
        }

        public static void SaveGameData(int currentScore, int currentCoins)
        {
            GameData gameData = new GameData();
            string json;

            using (StreamReader r = new StreamReader(gameDataLocation))
            {
                json = r.ReadToEnd();
                gameData = JsonSerializer.Deserialize<GameData>(json);
            }

            if (currentScore > gameData.HighScore)
            {
                gameData.HighScore = currentScore;
                json = JsonSerializer.Serialize(gameData);
                File.WriteAllText(gameDataLocation, json);
            }

            gameData.Coins = currentCoins;
            json = JsonSerializer.Serialize(gameData);
            File.WriteAllText(gameDataLocation, json);
        }

        public static void LoadSkinData(ContentManager content)
        {
            List<SkinData> skinData = new List<SkinData>();
            SkinsState.Skins = new List<Skin>();

            try
            {
                using (StreamReader r = new StreamReader(skinDataLocation))
                {
                    Console.WriteLine(skinDataLocation + ": reading data.");
                    string json = r.ReadToEnd();
                    skinData = JsonSerializer.Deserialize<List<SkinData>>(json);
                    for (int i = 0; i < skinData.Count; i++)
                    {
                        Skin skin = new(content, skinData[i].Name)
                        {
                            Name = skinData[i].Name,
                            Selected = skinData[i].Selected,
                            Locked = skinData[i].Locked,
                            Cost = skinData[i].Cost,
                            Frames = skinData[i].Frames,
                            FPS = skinData[i].FPS
                        };
                        skin.LoadTexture(content, skin.Name);
                        SkinsState.Skins.Add(skin);
                    }
                }
            }

            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine(skinDataLocation+": file not found.");
                using (FileStream fs = File.Create(skinDataLocation))
                {
                    Console.WriteLine(skinDataLocation + ": file created.");
                    byte[] data = new UTF8Encoding(true).GetBytes(defaultSkinData);
                    fs.Write(data, 0, data.Length);
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
                    SkinData skin = new()
                    {
                        Name = SkinsState.Skins[i].Name,
                        Selected = SkinsState.Skins[i].Selected,
                        Locked = SkinsState.Skins[i].Locked,
                        Cost = SkinsState.Skins[i].Cost,
                        Frames = SkinsState.Skins[i].Frames,
                        FPS = SkinsState.Skins[i].FPS
                    };
                    skinData.Add(skin);
                }
                string json = JsonSerializer.Serialize(skinData);
                File.WriteAllText(skinDataLocation, json);
            }
        }
    }
}