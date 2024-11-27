using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Punkt wejscia
public class EntryPoint : MonoBehaviour
{
    private bool czyZajety = false;
    public void UstawNaZajety(bool value = true) => czyZajety = value;
    public bool CzyZajety() => czyZajety;
}
