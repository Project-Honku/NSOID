using Newtonsoft.Json.Linq;
using System.Linq;
using ngov3;

namespace NSOID.Patches;

public static class SystemText {
    public static void Patch() {
        Plugin.Logger.LogInfo("SystemText sedang di patch...");

        JObject idSystemTexts = JObject.Parse(Utils.GetResource("Assets.Master.SystemText.json"));
        var systemTexts = NgoEx.getSystemTexts();

        foreach (var property in idSystemTexts.Properties()) {
            var founded = systemTexts.FirstOrDefault(item => item.Id == property.Name);

            if (founded != null) {
                founded.BodyEn = (string) idSystemTexts[property.Name]["BodyEn"];
            } else {
                var newText = new SystemTextMaster.Param {
                    Id = property.Name,
                    BodyEn = (string) idSystemTexts[property.Name]["BodyEn"]
                };

                systemTexts.Add(newText);
                Plugin.Logger.LogInfo($"{newText.Id} ditambahkan!");
            }
        }
    }
}