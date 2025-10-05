

namespace App

{
    public class Item //klassen item representerar en vara som användaren ska ladda upp
    {
        private static int _nextId = 1; //nextid är kommand som tilldelar varje item ett unikt ID med logik att nästa ID ökar uppåt

        public int Id { get; private set; } //unikt ID för varje item
        public string Name { get; set; } //namn för varje item
        public string Description { get; set; } //beskrivning för varje item
        public string Owner { get; set; } //items ägare

        public Item(string name, string description, string owner) //kontruktorn körs när man skaffar ny item, tilldelar uniq ID och egenskaper
        {
            Id = _nextId++; //ger varje ny item ett uniqt ID
            Name = name;
            Description = description;
            Owner = owner;
        }

    }
} 