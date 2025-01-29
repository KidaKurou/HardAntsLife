namespace AntsProject
{
    public class LegendaryWorker : Worker
    {
        public LegendaryWorker() : base("легендарный", 10, 6, 3)
        {
            AllowedResources = new List<string> { "росинка", "веточка", "росинка" };
        }

        public override List<Resource> GatherResources(Dictionary<string, int> availableResources, List<Colony> otherColonies = null)
        {
            var gathered = new List<Resource>();

            // Must gather resources in specific order: росинка, веточка, росинка
            foreach (var resourceType in AllowedResources)
            {
                if (availableResources.ContainsKey(resourceType) && availableResources[resourceType] > 0)
                {
                    gathered.Add(new Resource(resourceType, 1));
                    availableResources[resourceType]--;
                }
            }

            return gathered;
        }
    }
}