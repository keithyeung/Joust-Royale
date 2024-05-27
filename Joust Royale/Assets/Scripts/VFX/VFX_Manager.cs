using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : Singleton<VFX_Manager>
{
    public ParticleSystem deathSmoke;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService(this);
    }

    public void SetDeathSmokePositionAndPlay(Vector3 position)
    {
        deathSmoke.gameObject.transform.position = position;
        deathSmoke.Play();
    }


}
