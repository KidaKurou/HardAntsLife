namespace AntsProject
{
    public abstract class Worker : Insect
    {
        protected List<string> AllowedResources;
        protected int ResourceLimit;
        protected static bool ForemanBonusActive = false;

        protected Worker(string type, int health, int defense, int resourceLimit)
            : base(type, health, defense)
        {
            ResourceLimit = resourceLimit;
            AllowedResources = new List<string>();
        }

        public virtual List<Resource> GatherResources(Dictionary<string, int> availableResources, List<Colony> otherColonies = null)
        {
            var effectiveLimit = ForemanBonusActive ? ResourceLimit + 1 : ResourceLimit;
            var gathered = new List<Resource>();

            foreach (var resourceType in AllowedResources)
            {
                if (gathered.Count >= effectiveLimit) break;

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