
namespace App

{

    public static class SaveLoad //klassen saveload är ett statisk klass så att man behöver inte skapa ett objekt av den utan man kan bara kalla den med kommand SaveLoad.LoadAll/SaveAll
    {

        private static string usersFile = "users.txt"; //vi skapar 3 filer i programens root mapp, privata så att ingen utanför klassen kan ändra dem
        private static string itemsFile = "items.txt";
        private static string tradesFile = "trades.txt";

        public static void LoadAll(List<User> users, List<Item> items, List<Trade> trades) //läser in all data från alla filer
        {
            LoadUsers(users); //först läses in alla användare sedan items och till sist trades
            LoadItems(items);
            LoadTrades(trades);

        }

        public static void SaveAll(List<User> users, List<Item> items, List<Trade> trades) //funktionen sparar data från dom 3 listorna till filer
        {
            SaveUsers(users);
            SaveItems(items);
            SaveTrades(trades);
        }

        private static void LoadUsers(List<User> users) //läser in data från users filen
        {
            if (File.Exists(usersFile))
            {
                foreach (var line in File.ReadAllLines(usersFile)) //kontrollerar om det finns 2 delar(username och password)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2)
                        users.Add(new User(parts[0], parts[1]));
                }
            }
        }

        private static void SaveUsers(List<User> users) //sparar användare i filen i format ny rad och i varje rad format: username;password
        {
            List<string> lines = new List<string>();
            foreach (var u in users)
            {
                lines.Add($"{u.Username};{u.Password}");
            }
            File.WriteAllLines(usersFile, lines);
        }

        private static void LoadItems(List<Item> items) //läser in items(objekt) från filen med sina egenskaper formaterat i en rad med ; i mällan
        {
            if (File.Exists(itemsFile))
            {
                foreach (var line in File.ReadAllLines(itemsFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length == 4)
                    {
                        Item item = new Item(parts[1], parts[2], parts[3]);
                        item.GetType().GetProperty("Id").SetValue(item, int.Parse(parts[0]));
                        items.Add(item);
                    }
                }
            }
        }

        private static void SaveItems(List<Item> items) // sparar alla nya items i samma format som ovannämnd
        {
            List<string> lines = new List<string>();
            foreach (var it in items)
            {
                lines.Add($"{it.Id};{it.Name};{it.Description};{it.Owner}");
            }
            File.WriteAllLines(itemsFile, lines);
        }

        private static void LoadTrades(List<Trade> trades) // laddar in tidigare sparade trades filen om formattering uppfyler sina krav
        {
            if (File.Exists(tradesFile))
            {
                foreach (var line in File.ReadAllLines(tradesFile))
                {
                    var parts = line.Split(';');
                    if (parts.Length == 6)
                    {
                        Trade t = new Trade(int.Parse(parts[1]), parts[2], parts[3], parts[4]); //formateringssätt
                        t.GetType().GetProperty("Id").SetValue(t, int.Parse(parts[0]));
                        t.GetType().GetProperty("Status").SetValue(t, Enum.Parse<TradeStatus>(parts[5]));
                        trades.Add(t);
                    }
                }
            }
        }

        private static void SaveTrades(List<Trade> trades) //sparar och uppdaterar trades och ändringar i trades med sina information så som id, item id, description, senderusername, receiver username
        {
            List<string> lines = new List<string>();
            foreach (var t in trades)
            {
                lines.Add($"{t.Id};{t.ItemId};{t.ItemDescription};{t.SenderUsername};{t.ReceiverUsername};{t.Status}"); //formateringssätt
            }
            File.WriteAllLines(tradesFile, lines);
        }

    }
}