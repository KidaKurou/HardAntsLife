namespace AntsProject
{
    public class AdvancedPickpocketWorker : Worker
    {
        public AdvancedPickpocketWorker() : base("продвинутый карманник", 6, 2, 2)
        {
            AllowedResources = new List<string> { "листик", "камушек" };
        }

        public override List<Resource> GatherResources(Dictionary<string, int> availableResources, List<Colony> otherColonies = null)
        {
            var gathered = new List<Resource>();

            // Try to gather from pile first
            foreach (var resourceType in AllowedResources)
            {
                if (gathered.Count >= ResourceLimit) break;

                if (availableResources.ContainsKey(resourceType) && availableResources[resourceType] > 0)
                {
                    gathered.Add(new Resource(resourceType, 1));
                    availableResources[resourceType]--;
                }
                else if (otherColonies != null) // Try to steal if resource not available
                {
                    // Try to steal from other colonies
                    foreach (var colony in otherColonies)
                    {
                        if (colony.Resources.ContainsKey(resourceType) && colony.Resources[resourceType] > 0)
                        {
                            gathered.Add(new Resource(resourceType, 1));
                            colony.Resources[resourceType]--;
                            break;
                        }
                    }
                }
            }

            return gathered;
        }
    }
}