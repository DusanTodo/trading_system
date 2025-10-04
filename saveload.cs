

namespace App



{

    public static class SaveLoad
    {

        private static string usersFile = "users.txt";
        private static string itemsFile = "items.txt";
        private static string tradesFile = "trades.txt";

        public static void LoadAll(List<User> users, List<Items> items, List<Trades> trades)
        {
            LoadUsers(users);
            LoadItems(items);
            LoadTrades(trades);

        }

        public static void SaveAll(List<User> users, List<Items> items, List<Trades> trades)
        {
            SaveUsers(users);
            SaveItems(items);
            SaveTrades(trades);
        }

        private static void LoadUsers(List<User> users)
        {
            if (File.Exists(usersFile))
            {
                foreach (var line in File.ReadAllLines(usersFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2)
                        users.Add(new User(parts[0], parts[1]));
                }
            }
        }

        private static void SaveUsers(List<User> users)
        {
            List<string> lines = new List<string>();
            foreach (var u in users)
            {
                lines.Add($"{u.Username};{u.Password}");
            }
            File.WriteAllLines(usersFile, lines);
        }

        private static void LoadItems(List<Item> items)
        {
            if (File.Exists(itemsFile))
            {
                foreach (var line in File.ReadAllLines(itemsFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length == 4)
                    {
                        Item item = new Item(parts[1], parts[2], parts[3]);
                        // Sätter Id manuellt via reflektion
                        item.GetType().GetProperty("Id").SetValue(item, int.Parse(parts[0]));
                        items.Add(item);
                    }
                }
            }
        }

        private static void SaveItems(List<Item> items)
        {
            List<string> lines = new List<string>();
            foreach (var it in items)
            {
                lines.Add($"{it.Id};{it.Name};{it.Description};{it.Owner}");
            }
            File.WriteAllLines(itemsFile, lines);
        }

        private static void LoadTrades(List<Trade> trades)
        {
            if (File.Exists(tradesFile))
            {
                foreach (var line in File.ReadAllLines(tradesFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length == 6)
                    {
                        Trade t = new Trade(int.Parse(parts[1]), parts[2], parts[3], parts[4]);
                        t.GetType().GetProperty("Id").SetValue(t, int.Parse(parts[0]));
                        t.GetType().GetProperty("Status").SetValue(t, Enum.Parse<TradeStatus>(parts[5]));
                        trades.Add(t);
                    }
                }
            }
        }

        private static void SaveTrades(List<Trade> trades)
        {
            List<string> lines = new List<string>();
            foreach (var t in trades)
            {
                lines.Add($"{t.Id};{t.ItemId};{t.ItemDescription};{t.SenderUsername};{t.ReceiverUsername};{t.Status}");
            }
            File.WriteAllLines(tradesFile, lines);
        }

    }
}