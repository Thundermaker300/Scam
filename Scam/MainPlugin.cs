using Exiled.API.Features;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerHandler = Exiled.Events.Handlers.Player;
using Scp914Handler = Exiled.Events.Handlers.Scp914;
using ServerHandler = Exiled.Events.Handlers.Server;

namespace Scam
{
    public class MainPlugin : Plugin<Config>
    {
        private EventHandlers Handlers;

        public override void OnEnabled()
        {
            Handlers = new(this);

            PlayerHandler.InteractingDoor += Handlers.OnInteractingDoor;
            PlayerHandler.Spawned += Handlers.OnSpawned;
            PlayerHandler.FailingEscapePocketDimension += Handlers.OnFailingEscapePocketDimension;
            PlayerHandler.Dying += Handlers.OnDying;

            Scp914Handler.UpgradingPickup += Handlers.OnUpgrading;
            Scp914Handler.UpgradingInventoryItem += Handlers.OnUpgradingInventory;

            ServerHandler.RestartingRound += Handlers.OnRoundRestart;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerHandler.InteractingDoor -= Handlers.OnInteractingDoor;
            PlayerHandler.Spawned -= Handlers.OnSpawned;
            PlayerHandler.FailingEscapePocketDimension -= Handlers.OnFailingEscapePocketDimension;
            PlayerHandler.Dying -= Handlers.OnDying;

            Scp914Handler.UpgradingPickup -= Handlers.OnUpgrading;
            Scp914Handler.UpgradingInventoryItem -= Handlers.OnUpgradingInventory;

            ServerHandler.RestartingRound -= Handlers.OnRoundRestart;

            Handlers = null;
            base.OnDisabled();
        }
    }

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
}
