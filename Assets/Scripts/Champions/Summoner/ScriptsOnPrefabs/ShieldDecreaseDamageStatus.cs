using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDecreaseDamageStatus : StatusBase, IBuff
{
    bool damaged;                                               // True when the player gets damaged.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public float currentHealth;                                   // The current health the player has.
    public double reduction = 0.5;
    // Use this for initialization
    protected override void Start()
    {
       // currentHealth = GetComponents<>();

        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {
            Debug.Log(" Damage dealt to the player");
            //TakeDamage();
        }
        damaged = false;
    }

    public void TakeDamage(double amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;
        // Reduce the current damage by 50%.
        amount *= reduction;

        // Reduce the current health by the damage amount.
        currentHealth -= (float)amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

      
    }

    public override void OnStatusApplied()
    {
        Debug.Log(" Dammage dealt to the player reduced by" + int.Parse(OtherValues[0]) );  // % dammage reduction
    }


    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
