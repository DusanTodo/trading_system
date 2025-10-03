namespace App

{
    public class Item
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

        public Item(string name, string description, string owner)
        {
            Id = _nextId++;
            Name = name;
            Description = description;
            Owner = owner;
        }

    }
} 