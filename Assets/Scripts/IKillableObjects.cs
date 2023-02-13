using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillableObjects
{
    void TakeHit(int hitDamage);
    void Killed();
}
