using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponDemage = 10;

    public int WeaponDemage { get => weaponDemage; set => weaponDemage = value; }
}
