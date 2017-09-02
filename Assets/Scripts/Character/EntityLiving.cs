using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLiving : MonoBehaviour, IEntityLivingBase {

    int HP;
    int maxHP;

    public int GetHP() {
        return HP;
    }
    public void SetHP(int amount) {
        HP = amount;
    }
    public int GetMaxHP() {
        return maxHP;
    }
    public void SetMaxHP(int amount) {
        maxHP = amount;
    }

    public void ReduceHP(int amount)
    {
        HP -= amount;
    }
    public void AddHP(int amount)
    {
        HP += amount;
    }
    public void InstantKill() {
        KillEntity();
    }

    public void KillEntity()
    {
        SetHP(0);
    }

    public void DespawnEntity()
    {
        Debug.Log("Despawn");
    }
}
