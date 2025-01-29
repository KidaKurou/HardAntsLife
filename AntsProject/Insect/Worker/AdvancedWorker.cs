namespace AntsProject
{
    public class AdvancedWorker : Worker
    {
        public AdvancedWorker() : base("advanced", 6, 2, 2)
        {
            AllowedResources = new List<string> { "росинка", "камушек" };
        }
    }
}