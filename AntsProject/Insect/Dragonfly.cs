namespace AntsProject
{
    public class Dragonfly : SpecialInsect
    {
        public Dragonfly() : base("ленивый неуязвимый мирный подготовленный - Стрекоза", 22, 7)
        {
            IsInvulnerable = true;
            CanTakeResources = false;
        }

        public void ApplyColonyEffect(Colony colony)
        {
            // Ignore negative effects
            colony.IgnoreNegativeEffects = true;
        }
    }
}