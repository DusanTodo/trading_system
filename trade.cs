namespace App

{
    public enum TradeStatus
    {
        Pending,
        Approved,
        Denied
    }

    public class Trade
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public int ItemId { get; private set; } // id på item som efterfrågas
        public string ItemDescription { get; private set; }   //tillagt senare så att man kan se vad man accepterar inte bara ID
        public string SenderUsername { get; private set; }   // vem som skickade förfrågan
        public string ReceiverUsername { get; private set; } // ägaren av item
        public TradeStatus Status { get; private set; }

        public Trade(int itemId, string itemDescription, string senderUsername, string receiverUsername)
        {
            Id = _nextId++;
            ItemId = itemId;
            ItemDescription = itemDescription;
            SenderUsername = senderUsername;
            ReceiverUsername = receiverUsername;
            Status = TradeStatus.Pending;
        }

        public void Approve()
        {
            Status = TradeStatus.Approved;
        }

        public void Deny()
        {
            Status = TradeStatus.Denied;
        }
        public static void SetNextId(int next)
        {
            if (next > _nextId)
                _nextId = next;
        }

        public static int GetNextId() => _nextId;
    }
}