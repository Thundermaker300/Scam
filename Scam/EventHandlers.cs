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
using Exiled.API.Features.Items;

namespace Scam
{
    public class EventHandlers
    {
        private readonly MainPlugin Plugin;
        public readonly List<int> ScamCards = new();

        public ItemType GetScamCardType(RoleTypeId role, Room roomOfDeath)
        {
            if (roomOfDeath.Type == RoomType.Hcz106)
                return ItemType.KeycardO5;

            ItemType[] weakCards = { ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardZoneManager };
            ItemType[] goodCards = { ItemType.KeycardO5, ItemType.KeycardNTFCommander, ItemType.KeycardFacilityManager, ItemType.KeycardContainmentEngineer };
            if (role is RoleTypeId.ClassD)
                switch (roomOfDeath.Zone)
                {
                    case ZoneType.LightContainment:
                        return weakCards[Random.Range(0, weakCards.Length)];
                    case ZoneType.HeavyContainment or ZoneType.Entrance or ZoneType.Surface:
                        return goodCards[Random.Range(0, goodCards.Length)];
                }
            else if (role is RoleTypeId.Scientist)
                switch (roomOfDeath.Zone)
                {
                    case ZoneType.LightContainment:
                        return ItemType.KeycardScientist;
                    case ZoneType.HeavyContainment or ZoneType.Entrance or ZoneType.Surface:
                        return goodCards[Random.Range(0, goodCards.Length)];
                }
            else if (role.GetTeam() is Team.ChaosInsurgency)
                return ItemType.KeycardChaosInsurgency;
            else if (role.GetTeam() is Team.FoundationForces)
                switch (role)
                {
                    case RoleTypeId.FacilityGuard: return ItemType.KeycardGuard;
                    case RoleTypeId.NtfPrivate: return ItemType.KeycardNTFOfficer;
                    case RoleTypeId.NtfSpecialist or RoleTypeId.NtfSergeant: return ItemType.KeycardNTFLieutenant;
                    case RoleTypeId.NtfCaptain: return ItemType.KeycardNTFCommander;
                }
            return ItemType.KeycardO5;
        }

        public EventHandlers(MainPlugin plugin) => Plugin = plugin;

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Door.IsKeycardDoor && (ev.Player.CurrentItem is null || ev.Player.CurrentItem.IsKeycard == false))
            {
                ev.Player.Kill("You cannot open that door without a keycard.");
                return;
            }
            if (ev.Door.IsKeycardDoor && ev.IsAllowed && (Random.value > .5 || ScamCards.Contains(ev.Player.CurrentItem.Serial)))
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
            if (ev.IsAllowed && (Random.value > .75 || ScamCards.Contains(ev.Pickup.Serial)))
                ev.IsAllowed = false;
        }

        public void OnUpgradingInventory(UpgradingInventoryItemEventArgs ev)
        {
            if (ev.IsAllowed && (Random.value > .75 || ScamCards.Contains(ev.Item.Serial)))
                ev.IsAllowed = false;
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Player.IsHuman && Random.value >= .5)
            {
                Item item = ev.Player.AddItem(GetScamCardType(ev.Player.Role.Type, ev.Player.CurrentRoom));
                ScamCards.Add(item.Serial);
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

        public void OnThrownItem(ThrownItemEventArgs ev)
        {
            if (ev.Throwable is ExplosiveGrenade grenade && Random.value >= .95)
            {
                grenade.FuseTime = 0f;
            }
        }

        public void OnRoundRestart()
        {
            ScamCards.Clear();
        }
    }
}
