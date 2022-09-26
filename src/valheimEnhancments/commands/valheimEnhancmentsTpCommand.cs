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

        private string _invalidArgumentsMessage => "Invalid arguments - " + this.Description;

        public override void Execute(Terminal instance, string[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                instance.AddString(this._invalidArgumentsMessage);
                return;
            }

            string result;
            switch (arguments.Length)
            {
                case 1:
                    result = this.HandleLocation(arguments);
                    break;
                case 2:
                case 3:
                    if (float.TryParse(arguments[0], out var x) == false || float.TryParse(arguments[1], out var y) == false)
                    {
                        result = this._invalidArgumentsMessage;
                        break;
                    }

                    float? z = null;
                    if (arguments.Length == 4)
                    {
                        if (float.TryParse(arguments[2], out var zTemp) == false)
                        {
                            result = this._invalidArgumentsMessage;
                            break;
                        }

                        z = zTemp;
                    }

                    result = this.HandleCoordinates(x, y, z);
                    break;
                default:
                    result = this._invalidArgumentsMessage;
                    break;
            }

            instance.AddString(result);
            ZLog.Log(result);
        }

        private void Teleport(float x, float y, float? z = null)
        {
            Player.m_localPlayer.SetGodMode(true);
            Console.instance.TryRunCommand(z.HasValue ? $"goto {x} {y} {z.Value}" : $"goto {x} {y}");
        }

        private string HandleCoordinates(float x, float y, float? z = null)
        {
            this.Teleport(x, y, z);
            return $"Teleported player {Player.m_localPlayer.GetPlayerName()} to {x} {y} {(z.HasValue ? z.Value.ToString() : string.Empty)}";
        }

        private string HandleLocation(string[] arguments)
        {
            var locationName = arguments[0];

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
                new Location("Spawn", Minimap.PinType.None, 0, 0),

                new Location("North", Minimap.PinType.None, 0, 10000),
                new Location("South", Minimap.PinType.None, 0, -10000),
                new Location("West", Minimap.PinType.None, -10000, 0),
                new Location("East", Minimap.PinType.None, 10000, 0),

                new Location("Random", Minimap.PinType.None, () =>
                {
                    var x = Random.Range(-10000, 10000);
                    var y = Random.Range(-10000, 10000);
                    return (x, y, null);
                })
            };

            locations.AddRange(this.GetPinLocations());

            return locations;
        }

        private List<Location> GetPinLocations()
        {
            var locations = new List<Location>();

            var pins = this.GetAllPins();
            var unknownLocationCount = 0;

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
                        name = currentPin.m_name;
                        break;

                    case Minimap.PinType.Boss:
                        name = GetBossName(currentPin);
                        y += 10; // slight offset so 
                        break;

                    case Minimap.PinType.Death:
                        name = GetDeath(currentPin);
                        break;

                    case Minimap.PinType.Bed:
                        name = "Bed";
                        break;

                    case Minimap.PinType.Ping:
                        name = "Ping";
                        break;

                    case Minimap.PinType.None:
                        name = "UnknownLocation" + unknownLocationCount;
                        unknownLocationCount++;
                        break;

                    default:
                        break;
                }

                if (string.IsNullOrWhiteSpace(name) == false)
                    locations.Add(new Location(name, currentPin.m_type, x, z, y <= 0.1 ? (float?) null : y));
            }

            return locations;

            string GetDeath(Minimap.PinData pinData)
            {
                var deathCount = locations.Count(f => f.Type == Minimap.PinType.Death);
                return deathCount == 0 ? "Death" : "Death" + deathCount;
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
            public string Name { get; }

            public Func<(float x, float y, float? z)> GetCoordinates { get; }

            public Minimap.PinType Type { get; }

            private Location(string name, Minimap.PinType type)
            {
                this.Name = name;
                this.Type = type;
            }

            public Location(string name, Minimap.PinType type, float x, float y, float? z = null)
                : this(name, type)
            {
                this.GetCoordinates = () =>
                {
                    return (x, y, z);
                };
            }

            public Location(string name, Minimap.PinType type, Func<(float x, float y, float? z)> getCoordinatesFunc)
                : this(name, type)
            {
                this.GetCoordinates = getCoordinatesFunc;
            }

            public override string ToString()
            {
                (var x, var y, var z) = this.GetCoordinates();
                return $"{this.Name} (x={x} y={y}{(z.HasValue ? $" z={z}" : string.Empty)})";
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

            ZLog.Log($"Gathered all pins:{Environment.NewLine}{string.Join(Environment.NewLine, allPins.Select(f => f.m_name + " " + f.m_type + $"(x={f.m_pos.x} y={f.m_pos.y}{f.m_pos.z}"))}");
            return allPins;
        }
    }
}
