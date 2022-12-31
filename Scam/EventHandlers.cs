using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;
using MEC;
using PlayerRoles;
using CustomPlayerEffects;
using Exiled.CustomItems.API.EventArgs;
using Exiled.Events.EventArgs.Scp914;
using System.Collections.Generic;
using Exiled.API.Features;

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

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Team is not Team.SCPs)
            {
                ev.Player.EnableEffect<Exhausted>(10f);
                ev.Player.EnableEffect<Concussed>(10f);
            }
        }

        public void OnUpgrading(UpgradingPickupEventArgs ev)
        {
            if (ev.IsAllowed && Random.value > .75)
                ev.IsAllowed = false;
        }

        public void OnUpgradingInventory(UpgradingInventoryItemEventArgs ev)
        {
            if (ev.IsAllowed && Random.value > .75)
                ev.IsAllowed = false;
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

        private readonly Dictionary<Player, int> ThrownAttempts = new();
        public void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {
            if (!ThrownAttempts.ContainsKey(ev.Player))
            {
                ThrownAttempts[ev.Player] = 0;
                ev.IsAllowed = false;
                return;
            }
            if (ThrownAttempts[ev.Player] < Random.Range(5, 10))
            {
                ev.Player.ShowHint("Keep trying...", 0.5f);
                ev.IsAllowed = false;
                ThrownAttempts[ev.Player]++;
            }
            else
            {
                ThrownAttempts[ev.Player] = 0;
            }
        }

        public void OnRoundRestart()
        {
            ThrownAttempts.Clear();
        }
    }
}
