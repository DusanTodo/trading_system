namespace App

{
    public enum TradeStatus //enum är et tsätt att definera tydligare status värden istället för siffror
    {
        Pending,
        Approved,
        Denied
    }

    public class Trade //klassen trade representerar oss bytesförfrågan
    {
        private static int _nextId = 1;

        public int Id { get; private set; } //uniqt items ID
        public int ItemId { get; private set; } //ID på item som efterfrågas
        public string ItemDescription { get; private set; }   //tillagt senare så att man kan se vad man accpterar inte bara ID
        public string SenderUsername { get; private set; }   //vem som skickade förfrågan
        public string ReceiverUsername { get; private set; } //ägaren av item
        public TradeStatus Status { get; private set; } //status på förfrågan

        public Trade(int itemId, string itemDescription, string senderUsername, string receiverUsername) //konstruktorn skappa en ny affär med status pending som automatisk
        {
            Id = _nextId++; //gör ett unikt ID för varje nytt item
            ItemId = itemId; 
            ItemDescription = itemDescription;
            SenderUsername = senderUsername;
            ReceiverUsername = receiverUsername;
            Status = TradeStatus.Pending; //ustomatisk status pending tilldelas när ett nytt item är inalgt i systemet
        }

        public void Approve() //metod för att godkänna affären
        {
            Status = TradeStatus.Approved;
        }

        public void Deny() //metod för att neka affären
        {
            Status = TradeStatus.Denied;
        }

    }
}