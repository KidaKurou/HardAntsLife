namespace AntsProject
{
    public class LegendaryMythicalAnt
    {
        private Random random = new Random();
        private bool hasActivated = false;

        public void ApplyEffect(List<Colony> colonies)
        {
            if (!hasActivated)
            {
                int targetColonyIndex = random.Next(colonies.Count);
                Console.WriteLine($"Легендарный мифический муравей поселился в колонию {colonies[targetColonyIndex].Name}");
                for (int i = 0; i < colonies.Count; i++)
                {
                    if (i != targetColonyIndex && !colonies[i].IsDestroyed && !colonies[i].IgnoreNegativeEffects)
                    {
                        Console.WriteLine($"Колония {colonies[i].Name} уничтожена");
                        colonies[i].Destroy();
                    }
                }
                hasActivated = true;
            }
        }
    }
}