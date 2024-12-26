using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Linq;

namespace NSOID;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Windose.exe")]
public class Plugin : BaseUnityPlugin {
    internal static new ManualLogSource Logger;
    private static readonly Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

    private void Awake() {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION}");

        try {
            // Execute all Patch() methods
            Assembly assembly = Assembly.GetExecutingAssembly();
            var typesPatches = assembly.GetTypes()
                .Where(type => type.Namespace == "NSOID.Patches" && type.IsClass);
            foreach (var type in typesPatches) {
                MethodInfo method = type.GetMethod("Patch");
                method?.Invoke(null, null);
            }

            // Patch all Harmony patches
            harmony.PatchAll();
            Logger.LogInfo("Patching selesai!");
        } catch (System.Exception ex) {
            Logger.LogError($"Patch gagal dilakukan!");
            Logger.LogError(ex);
        }
    }
}

// Custom class
public class AltBoot : MonoBehaviour {
    public void Awake(){}
    public bool shownModIntro;
}