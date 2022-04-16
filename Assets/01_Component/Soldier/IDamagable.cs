public struct AttackDefiniton
{
    public IAttackable atacker;

    //public IDamagable target;

    public float damagePoint;
}

public interface IDamagable
{
    void GetDamagable(AttackDefiniton attackDefiniton);
}
