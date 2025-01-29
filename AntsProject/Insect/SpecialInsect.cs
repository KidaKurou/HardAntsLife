namespace AntsProject
{
    public abstract class SpecialInsect : Insect
    {
        protected bool CanTakeResources;
        protected bool IsAggressive;
        public int Damage { get; protected set; }
        public int TargetLimit { get; protected set; }

        protected SpecialInsect(string type, int health, int defense, int damage = 0, int targetLimit = 0)
            : base(type, health, defense)
        {
            Damage = damage;
            TargetLimit = targetLimit;
            CanTakeResources = false;
            IsInvulnerable = false;
            IsAggressive = false;
        }
    }
}