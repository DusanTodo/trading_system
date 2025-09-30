namespace App;
{
    public class Item
    {
        // Enkel autoincrement (observera: nollställs när programmet startar om; vi löser detta när vi implementerar persistens)
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // Spara ägaren som användarnamn för att undvika komplex serialisering av User-objekt i första steget
        public string Owner { get; private set; }

        public Item(string name, string description, string owner)
        {
            Id = _nextId++;
            Name = name;
            Description = description;
            Owner = owner;
        }
    }
} 