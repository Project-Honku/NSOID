using Newtonsoft.Json.Linq;
using System.Linq;
using ngov3;

namespace NSOID.Patches;

public static class Cmd {
    public static void Patch() {
        Plugin.Logger.LogInfo("Cmd sedang di patch...");

        JObject idSystemTexts = JObject.Parse(Utils.GetResource("Assets.Master.Cmd.json"));
        var systemTexts = NgoEx.getCmds();

        foreach (var property in idSystemTexts.Properties()) {
            var founded = systemTexts.FirstOrDefault(item => item.Id == property.Name);

            if (founded != null) {
                founded.LabelEn = (string) idSystemTexts[property.Name]["LabelEn"];
                founded.DescEn = (string) idSystemTexts[property.Name]["DescEn"];
            } else {
                var newText = new CmdMaster.Param {
                    Id = property.Name,
                    LabelEn = (string) idSystemTexts[property.Name]["LabelEn"],
                    DescEn = (string) idSystemTexts[property.Name]["DescEn"]
                };

                systemTexts.Add(newText);
                Plugin.Logger.LogInfo($"{newText.Id} ditambahkan!");
            }
        }
    }
}