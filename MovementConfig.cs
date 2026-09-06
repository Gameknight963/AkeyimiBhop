using Il2CppSystem.Linq;
using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using System.IO;

namespace AkeyimiBhop
{
    public class MovementConfig
    {
        public static MovementConfig Instance { get; private set; } = new();
        private static readonly FileSystemWatcher watcher;

        private static string ConfigPath = Path.Combine(MelonEnvironment.ModsDirectory, "AkeyimiBhop", "MovementConfig.json");

        public MovementConfig()
        {
            // I only need to do this cause melonloader devs decided to use null!
            ConfigPath ??= Path.Combine(MelonEnvironment.ModsDirectory, "AkeyimiBhop", "MovementConfig.json");
            if (!File.Exists(ConfigPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(this, Formatting.Indented));
                return;
            }
            JsonConvert.PopulateObject(File.ReadAllText(ConfigPath), this);
        }

        static MovementConfig()
        {
            string dir = Path.Combine(MelonEnvironment.ModsDirectory, "AkeyimiBhop");
            watcher = new FileSystemWatcher(dir, "MovementConfig.json")
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (s, e) =>
            {
                Thread.Sleep(100);
                Instance.Reload();
                MelonLogger.Msg($"Config reloaded!");
            };
            watcher.EnableRaisingEvents = true;
        }

        public void Reload()
        {
            string path = Path.Combine(ConfigPath);
            if (File.Exists(path))
                JsonConvert.PopulateObject(File.ReadAllText(path), this);
        }

        [JsonIgnore]
        private const float HU_TO_UNITY = 0.0254f;

        public float MaxGroundSpeed { get; set; } = 320f;
        public float GroundAccel { get; set; } = 600f;
        public float AirAccel { get; set; } = 150f;
        public float Friction { get; set; } = 4f;
        public float JumpVelocity { get; set; } = 268f;
        public float AirCap { get; set; } = 320f;

        public float MaxGroundSpeedUnity => MaxGroundSpeed * HU_TO_UNITY;
        public float GroundAccelUnity => GroundAccel * HU_TO_UNITY;
        public float AirAccelUnity => AirAccel * HU_TO_UNITY;
        public float JumpVelocityUnity => JumpVelocity * HU_TO_UNITY;
        public float AirCapUnity => AirCap * HU_TO_UNITY;
    }
}
