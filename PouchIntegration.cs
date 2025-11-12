using HkmpPouch;
using System;
using UnityEngine;

namespace WiddleKnight
{
    internal static class SafePouchIntegration
    {
        internal static PipeClient pipe = new PipeClient("WiddleKnight");

    }
    internal static class PouchIntegration
    {
        internal static string oldUpdate = null;
        internal static void Initialize()
        {
            SafePouchIntegration.pipe.On(WiddleKnightUpdateFactory.Instance).Do<WiddleKnightUpdate>(WiddleKnightUpdateHandler);
        }
        internal static void SendUpdate(WiddleKnightControl WiddleKnightControl)
        {
            if (SafePouchIntegration.pipe != null && SafePouchIntegration.pipe.ClientApi != null && SafePouchIntegration.pipe.ClientApi.NetClient.IsConnected)
            {

                var newUpdate = new WiddleKnightUpdate
                {
                    pos = (Hkmp.Math.Vector2)(Vector2)WiddleKnightControl.transform.position,
                    dir = WiddleKnightControl.lookDirection,
                    anim = WiddleKnightControl.state
                };

                if (oldUpdate != newUpdate.ToString()) {

                    SafePouchIntegration.pipe.Broadcast(newUpdate);
                    oldUpdate = newUpdate.ToString();
                }
            }
        }

        internal static void WiddleKnightUpdateHandler(WiddleKnightUpdate update)
        {
            var _go = WiddleKnight.Instance.GetNetworkWiddleKnight(update.FromPlayer);
            var _control = _go.GetComponent<WiddleKnightControl>();
            _control.state = update.anim;
            _control.networkMovementTarget = new Vector2(update.pos.X, update.pos.Y);
            _control.lookDirection = update.dir;
            _control.isNetworkControlled = true;
            _control.UpdateNetworkCoro();
        }
    }
}
