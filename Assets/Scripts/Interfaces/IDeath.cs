using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeath
{
    void StopMovement();
    void StopAttacking();
    void DropLoot();
    void SpecialDeathAction();
    void PlayDeathAnimation();
}
