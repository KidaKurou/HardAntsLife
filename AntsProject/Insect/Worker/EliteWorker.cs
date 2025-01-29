namespace AntsProject
{
    public class EliteWorker : Worker
    {
        public EliteWorker() : base("elite", 8, 4, 2)
        {
            AllowedResources = new List<string> { "камушек", "листик" };
        }
    }
}