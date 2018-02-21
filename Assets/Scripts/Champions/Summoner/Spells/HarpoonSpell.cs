using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonSpell : Spell
{
    private GameObject _arpoon;
    private EntityLivingBase _monster;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;
    private Transform _posStart;
    private Transform _posEnd;

    private bool _monsterIsGrabbed = false;

    protected override void Start()
    {
        _arpoon = LoadResource("Harpoon");
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }


    protected override void Update()
    {
        if (_monsterIsGrabbed == true)
        {
            // Debug.Log("moving" + _posEnd.position.x + " " + _posStart.position.x);

            _monster.transform.position = Vector3.Lerp(_posStart.position, _posEnd.position, Time.deltaTime * 3.0f);

            if (Vector3.Distance(_posStart.position, _posEnd.position) < 3.0f)
            {
                _monsterIsGrabbed = false;
            }

        }

        base.Update();
    }

    public override void LaunchSpell()
    {
        if (IsSpellLauncheable())
        {
            Instantiate(_arpoon, transform.position + (transform.forward * 1f) + (transform.up * 1.5f), transform.rotation, this.transform);

            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }

        base.LaunchSpell();
    }

    public void ApplyEffectOnHit(EntityLivingBase entityHit)
    {
        _monster = entityHit;

        _posStart = _monster.transform;
        _posEnd = transform;
        _monsterIsGrabbed = true;

        ApplyStatus(Status[0], entityHit.transform);
    }
}
