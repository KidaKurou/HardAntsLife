namespace AntsProject
{
    public class Resource
    {
        public string Type { get; set; }
        public int Amount { get; set; }

        public Resource(string type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}