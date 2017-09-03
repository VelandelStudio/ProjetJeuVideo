public interface IEntityLivingBase {

    int GetHP();
    void SetHP(int amount);
    int GetMaxHP();
    void SetMaxHP(int amount);

    void ReduceHP(int amount);
    void AddHP(int amount);
    void InstantKill();

    void KillEntity();
    void DespawnEntity();
}
