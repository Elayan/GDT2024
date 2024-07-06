using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MachineBase
{
    public override void Activate(Tray tray)
    {
        if (tray.IsFull)
        {
            if (DebugLog)
                Debug.Log("Tray is full, Dispenser will not dispense.");
            return;
        }

        tray.PutOnTray(Dispensed);
    }
}
