namespace AntsProject
{
    public class ResourcePile
    {
        public Dictionary<string, int> Resources { get; private set; }
        public int Index { get; private set; }

        public ResourcePile(int index, Dictionary<string, int> initialResources)
        {
            Index = index;
            Resources = new Dictionary<string, int>(initialResources);
        }

        public bool IsExhausted => Resources.Values.All(v => v == 0);

        public void DisplayStatus()
        {
            if (IsExhausted)
                Console.WriteLine($"Куча {Index}: истощена");
            else
                Console.WriteLine($"Куча {Index}: {string.Join(", ", Resources.Select(r => $"{r.Key}={r.Value}"))}");
        }
    }
}