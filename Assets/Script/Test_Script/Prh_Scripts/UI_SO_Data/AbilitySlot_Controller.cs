using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlot_Controller : MonoBehaviour
{
    public AbilitySlot_Data attackSlot;
    public AbilitySlot_Data healthSlot;
    public AbilitySlot_Data speedSlot;

    public CAbilityDataSO attackSO;
    public CAbilityDataSO healthSO;
    public CAbilityDataSO speedSO;

    void Start()
    {
        attackSlot.Init(attackSO);
        healthSlot.Init(healthSO);
        speedSlot.Init(speedSO);
    }
}
