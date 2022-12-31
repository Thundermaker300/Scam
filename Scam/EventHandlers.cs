using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;
using MEC;

namespace Scam
{
    public class EventHandlers
    {
        private readonly MainPlugin Plugin;

        public EventHandlers(MainPlugin plugin) => Plugin = plugin;

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Door.IsKeycardDoor && ev.IsAllowed && Random.value > .5)
            {
                ev.IsAllowed = false;
                ev.Door.PlaySound(DoorBeepType.PermissionDenied);
            }
        }

        public void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
        {
            ev.IsAllowed = false;
            Vector3 exit = Scp106PocketExitFinder.GetBestExitPosition(ev.Player.Role.Base as PlayerRoles.FirstPersonControl.IFpcRole);
            ev.Player.Teleport(exit);
            Timing.CallDelayed(5f, () =>
            {
                ev.Player.Kill(DamageType.PocketDimension);
            });
        }

        public void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
        {
            ev.IsAllowed = false;
            Vector3 exit = Scp106PocketExitFinder.GetBestExitPosition(ev.Player.Role.Base as PlayerRoles.FirstPersonControl.IFpcRole);
            ev.Player.Teleport(exit);
            Timing.CallDelayed(5f, () =>
            {
                ev.Player.Kill(DamageType.PocketDimension);
            });
        }
    }
}
