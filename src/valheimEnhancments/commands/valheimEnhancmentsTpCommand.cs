using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using valheimEnhancments.extensions;
using Random = UnityEngine.Random;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsTpCommand : valheimEnhancmentsCommand
    {
        public override string Name => "tp";
        public override string Description => "teleports the player";
        public override string Syntax => "[location] or [x] [y] [z]";

        private string _invalidArgumentsMessage => "Invalid arguments: use " + this.Syntax +
            Environment.NewLine + "Available locations:" +
            Environment.NewLine + string.Join(Environment.NewLine, this.GetLocations());

        public override void Execute(Terminal instance, List<string> arguments)
        {
            string result;
            switch (arguments.Count)
            {
                case 1:
                    result = this.HandleLocation(arguments[0]);
                    break;
                case 2:
                case 3:
                    result = this.HandleCoordinates(arguments);
                    break;
                default:
                    result = this._invalidArgumentsMessage;
                    break;
            }

            instance.AddString(result);
        }

        // because i'm a big dummy:
        // https://www.evl.uic.edu/ralph/508S98/coordinates.html
        // x = left/right
        // y = up/down
        // z = forward/backward
        private void Teleport(float x, float y, float z)
        {
            var destination = new UnityEngine.Vector3(x, y, z);

            Player.m_localPlayer.TeleportTo(destination, Player.m_localPlayer.transform.rotation, true);
        }

        private string HandleCoordinates(List<string> arguments)
        {
            float x = 0, y = 0, z = 0;

            if (arguments.Count == 2)
            {
                if (float.TryParse(arguments[0], out var parsedX))
                    x = parsedX;

                if (float.TryParse(arguments[1], out var parsedZ))
                    z = parsedZ;
            }
            else if (arguments.Count == 3)
            {
                if (float.TryParse(arguments[0], out var parsedX))
                    x = parsedX;

                if (float.TryParse(arguments[1], out var parsedY))
                    y = parsedY;

                if (float.TryParse(arguments[2], out var parsedZ))
                    z = parsedZ;
            }
            else
            {
                return this._invalidArgumentsMessage;
            }

            return this.HandleCoordinates(x, y, z);
        }

        private string HandleCoordinates(float x, float y, float z)
        {
            this.Teleport(x, y, z);
            return $"Teleported player {Player.m_localPlayer.GetPlayerName()} to {x.FormatCoordinate()} {(y != 0 ? y.FormatCoordinate() + " " : string.Empty)}{z.FormatCoordinate()}";
        }

        private string HandleLocation(string locationName)
        {
            var location = this.GetLocations().FirstOrDefault(f => string.Equals(f.Name, locationName, StringComparison.InvariantCultureIgnoreCase));

            if (location != null)
            {
                (var x, var y, var z) = location.GetCoordinates();

                this.Teleport(x, y, z);
                return $"Teleported player {Player.m_localPlayer.GetPlayerName()} to {location}";
            }
            else
            {
                return $"Could not find location {locationName} - available locations:{Environment.NewLine}{string.Join(Environment.NewLine, this.GetLocations())}";
            }
        }

        private List<Location> GetLocations()
        {
            var locations = new List<Location>
            {
                new Location("Spawn", Minimap.PinType.None, 0, 0, 0),

                new Location("North", Minimap.PinType.None, 0,0, 10000),
                new Location("South", Minimap.PinType.None, 0,0, -10000),
                new Location("West", Minimap.PinType.None, -10000,0, 0),
                new Location("East", Minimap.PinType.None, 10000,0, 0),

                new Location("Random", Minimap.PinType.None, () =>
                {
                    var x = Random.Range(-10000, 10000);
                    var z = Random.Range(-10000, 10000);
                    return (x, 0, z);
                })
            };

            locations.AddRange(this.GetPinLocations());

            foreach (var currentGroup in locations.GroupBy(f => f.Name).Where(f => f.Count() > 1))
            {
                var items = currentGroup.ToList();
                for (int i = 1; i < currentGroup.Count(); i++)
                {
                    items[i].Name += i;
                }
            }

            return locations;
        }

        private List<Location> GetPinLocations()
        {
            var locations = new List<Location>();

            var pins = this.GetAllPins();

            foreach (var currentPin in pins)
            {
                var name = string.Empty;

                var x = currentPin.m_pos.x;
                var y = currentPin.m_pos.y;
                var z = currentPin.m_pos.z;

                switch (currentPin.m_type)
                {
                    case Minimap.PinType.Icon0:
                    case Minimap.PinType.Icon1:
                    case Minimap.PinType.Icon2:
                    case Minimap.PinType.Icon3:
                    case Minimap.PinType.Icon4:
                        name = GetUserGivenName(currentPin);
                        break;

                    case Minimap.PinType.Boss:
                        name = GetBossName(currentPin);
                        break;

                    case Minimap.PinType.Death:
                        name = "Death";
                        break;

                    case Minimap.PinType.Bed:
                        name = "Bed";
                        break;

                    case Minimap.PinType.Ping:
                        name = "Ping";
                        break;

                    case Minimap.PinType.None:
                        name = "UnknownLocation";
                        break;

                    default:
                        break;
                }

                if (string.IsNullOrWhiteSpace(name) == false)
                    locations.Add(new Location(name, currentPin.m_type, x, y, z));
            }

            return locations;

            string GetUserGivenName(Minimap.PinData pinData)
            {
                if (string.IsNullOrWhiteSpace(pinData.m_name))
                    return "UnnamedLocation";

                return pinData.m_name;
            }

            string GetBossName(Minimap.PinData pinData)
            {
                var name = pinData.m_name.Split('_')[1];

                if (string.Equals(name, "gdking", StringComparison.InvariantCultureIgnoreCase))
                    name = "Elder";

                if (string.Equals(name, "goblinking", StringComparison.InvariantCultureIgnoreCase))
                    name = "Yagluth";

                if (string.Equals(name, "dragon", StringComparison.InvariantCultureIgnoreCase))
                    name = "Moder";

                return name.CapitalizeFirstLetter();
            }
        }

        private class Location
        {
            public string Name { get; set; }

            public Func<(float x, float y, float z)> GetCoordinates { get; }

            public Minimap.PinType Type { get; }

            private Location(string name, Minimap.PinType type)
            {
                this.Name = name;
                this.Type = type;
            }

            public Location(string name, Minimap.PinType type, float x, float y, float z)
                : this(name, type)
            {
                this.GetCoordinates = () =>
                {
                    return (x, y, z);
                };
            }

            public Location(string name, Minimap.PinType type, Func<(float x, float y, float z)> getCoordinatesFunc)
                : this(name, type)
            {
                this.GetCoordinates = getCoordinatesFunc;
            }

            public override string ToString()
            {
                (var x, var y, var z) = this.GetCoordinates();
                return $"{this.Name} (x={x.FormatCoordinate()} y={y.FormatCoordinate()} z={z.FormatCoordinate()})";
            }
        }

        private List<Minimap.PinData> GetAllPins()
        {
            var allPins = new List<Minimap.PinData>();

            Traverse.IterateFields(Minimap.instance, (Traverse traverse) =>
            {
                var values = traverse.GetValue();

                if (values is IEnumerable<Minimap.PinData> pins)
                {
                    allPins.AddRange(pins);
                }
            });

            ZLog.Log($"Gathered all pins:{Environment.NewLine}{string.Join(Environment.NewLine, allPins.Select(f => f.m_name + " " + f.m_type + $"(x={f.m_pos.x.FormatCoordinate()} y={f.m_pos.y.FormatCoordinate()} z={f.m_pos.z.FormatCoordinate()})"))}");
            return allPins;
        }
    }
}
