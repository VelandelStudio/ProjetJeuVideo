using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** HarpoonSpell, public class
 * @extends Spell
 * This script allows to grab a monster and move him near of player.
 **/
public class HarpoonSpell : Spell
{
    private GameObject _harpoon;
    private EntityLivingBase _monster;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;
    private Transform _posStart;
    private Transform _posEnd;

    private bool _monsterIsGrabbed = false;

    /** Start, protected override void method
     * Load Harpoon, get the player's camera and the transform of the right hand of player.
     **/
    protected override void Start()
    {
        _harpoon = LoadResource("Harpoon");
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }

    /** Update, protected override void method
     * If the Harpoon prefab touch a monster _monsterIsGrabbed equals true. We get the monster in the ApllyEffectOnHit. In this method when a monster is grabbed we modify linearly his position to the player's position.
     * When the distance between player and monster is lower than 3.0 unities the _monsterIsGrabbed equals false to stop the movement of the enemy.
     **/
    protected override void Update()
    {
        if (_monsterIsGrabbed == true)
        {
            
            _monster.transform.position = Vector3.Lerp(_posStart.position, _posEnd.position, Time.deltaTime * 3.0f);

            if (Vector3.Distance(_posStart.position, _posEnd.position) < 3.0f)
            {
                _monsterIsGrabbed = false;
            }

        }

        base.Update();
    }

    /** LaunchSpell, public override void
     * If the spell is launcheable we instantiate the harpoon gameObject forward the player. 
     **/
    public override void LaunchSpell()
    {
        if (IsSpellLauncheable())
        {
            Instantiate(_harpoon, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);

            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }

        base.LaunchSpell();
    }

    /** ApplyEffectOnHit, public void method
     * When the harpoon touchs a monster this method is called and gets the monster touched (_monster), his position (_posStart), the position of the player (_posEnd), and initializes _monsterIsGrabbed.
     * This method apply the HarpoonedStunStatus and HarpoonedDebuffStatus.
     **/
    public void ApplyEffectOnHit(EntityLivingBase entityHit)
    {
        _monster = entityHit;

        _posStart = _monster.transform;
        _posEnd = transform;
        _monsterIsGrabbed = true;

        ApplyStatus(Status[1], entityHit.transform);
        ApplyStatus(Status[0], entityHit.transform);
    }
}
