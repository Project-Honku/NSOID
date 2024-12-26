using System.Reflection;

namespace NSOID.Patches;

public static class Utils {
    public static Assembly assembly = Assembly.GetExecutingAssembly();

    public static string GetResource(string name) {
        using var stream = assembly.GetManifestResourceStream(MyPluginInfo.PLUGIN_GUID + "." + name);
        using var reader = new System.IO.StreamReader(stream);
        return reader.ReadToEnd();
    }
}