namespace AntsProject
{
    public class AdvancedForemanWorker : Worker
    {
        public AdvancedForemanWorker() : base("продвинутый бригадир", 6, 2, 2)
        {
            AllowedResources = new List<string> { "росинка", "листик" };
        }

        public override List<Resource> GatherResources(Dictionary<string, int> availableResources, List<Colony> otherColonies = null)
        {
            var gathered = new List<Resource>();

            foreach (var resourceType in AllowedResources)
            {
                if (gathered.Count >= ResourceLimit) break;

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