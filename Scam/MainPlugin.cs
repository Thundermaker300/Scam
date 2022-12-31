using Exiled.API.Features;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerHandler = Exiled.Events.Handlers.Player;

namespace Scam
{
    public class MainPlugin : Plugin<Config>
    {
        private EventHandlers Handlers;

        public override void OnEnabled()
        {
            Handlers = new(this);

            PlayerHandler.InteractingDoor += Handlers.OnInteractingDoor;
            PlayerHandler.FailingEscapePocketDimension += Handlers.OnFailingEscapePocketDimension;

            base.OnDisabled();
        }

        public override void OnDisabled()
        {
            PlayerHandler.InteractingDoor += Handlers.OnInteractingDoor;
            PlayerHandler.FailingEscapePocketDimension += Handlers.OnFailingEscapePocketDimension;

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
