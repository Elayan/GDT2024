using UnityEngine;

public class Disposer : MachineBase
{
    public override void Activate(Tray tray)
    {
        if (tray.IsEmpty)
        {
            if (DebugLog)
                Debug.Log("Tray is empty, Disposer will not dispose.");
            return;
        }

        tray.ClearTray();
    }
}
