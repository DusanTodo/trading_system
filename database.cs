// File: Database.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace App

{
    internal class Snapshot
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Trade> Trades { get; set; } = new List<Trade>();
        public int NextItemId { get; set; }
        public int NextTradeId { get; set; }
    }

    public static class Database
    {
        private static readonly string Folder = Path.Combine(Environment.CurrentDirectory, "Data");
        private static readonly string FileName = Path.Combine(Folder, "data.json");
        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { WriteIndented = true };

        // --- Spara data ---
        public static void Save(List<User> users, List<Item> items, List<Trade> trades)
        {
            try
            {
                var dir = Path.GetDirectoryName(FileName);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var snap = new Snapshot
                {
                    Users = users ?? new List<User>(),
                    Items = items ?? new List<Item>(),
                    Trades = trades ?? new List<Trade>(),
                    NextItemId = Item.GetNextId(),
                    NextTradeId = Trade.GetNextId()
                };

                string json = JsonSerializer.Serialize(snap, JsonOpts);

                // Använd StreamWriter istället för WriteAllText
                using (StreamWriter writer = new StreamWriter(FileName, false)) // false = skriv över
                {
                    writer.Write(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not save data: {ex.Message}");
            }
        }

        // --- Ladda data ---
        public static void Load(out List<User> users, out List<Item> items, out List<Trade> trades)
        {
            users = new List<User>();
            items = new List<Item>();
            trades = new List<Trade>();

            try
            {
                if (!File.Exists(FileName))
                    return; // ingen fil finns ännu

                string json;
                // Använd StreamReader istället för ReadAllText
                using (StreamReader reader = new StreamReader(FileName))
                {
                    json = reader.ReadToEnd();
                }

                var snap = JsonSerializer.Deserialize<Snapshot>(json);
                if (snap != null)
                {
                    users = snap.Users ?? new List<User>();
                    items = snap.Items ?? new List<Item>();
                    trades = snap.Trades ?? new List<Trade>();

                    Item.SetNextId(snap.NextItemId);
                    Trade.SetNextId(snap.NextTradeId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not load data: {ex.Message}");
            }
        }
    }
}