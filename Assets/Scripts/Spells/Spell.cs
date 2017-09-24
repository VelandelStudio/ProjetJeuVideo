using UnityEngine;

public abstract class Spell : MonoBehaviour {
	protected float SpellCD;
	protected float CurrentCD;
	protected bool SpellInUse = false;

    protected virtual void Start() 
	{
		DisplaySpellCreation(this);
		CurrentCD = SpellCD;
    }
	protected virtual void Update() {
		if(!IsSpellLauncheable())
			CurrentCD = Mathf.Clamp(CurrentCD + Time.deltaTime, 0, SpellCD);	
	}
	
	public virtual void LaunchSpell(){
        if (!IsSpellLauncheable())
            DisplaySpellNotLauncheable(this);
        else
            SpellInUse = true;
	}
		
	protected virtual void OnSpellLaunched() {
		CurrentCD = 0;
		SpellInUse = false;
	}
	
	protected virtual bool IsSpellLauncheable() {
		return (SpellCD == CurrentCD);
	}
	
	public bool IsSpellInUse() {
		return SpellInUse;
	}

    protected void DisplaySpellNotLauncheable(Spell spell) {
		Debug.Log(spell.GetType().ToString()+" is not available for the moment.");
	}
	
	protected void DisplaySpellCreation(Spell spell) {
		Debug.Log(spell.GetType().ToString()+" created.");
	}
}
