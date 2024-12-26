using Newtonsoft.Json.Linq;
using System.Linq;
using ngov3;

namespace NSOID.Patches;

public static class Act {
    public static void Patch() {
        Plugin.Logger.LogInfo("Act sedang di patch...");

        JObject idSystemTexts = JObject.Parse(Utils.GetResource("Assets.Master.Act.json"));
        var systemTexts = CommandHelper.getActMaster().param;
        var systemTexts2 = NgoEx.getActions();

        foreach (var property in idSystemTexts.Properties()) {
            var founded = systemTexts.FirstOrDefault(item => item.Id == property.Name);
            var founded2 = systemTexts.FirstOrDefault(item => item.Id == property.Name);

            if (founded != null && founded2 != null) {
                founded.TitleEn = (string) idSystemTexts[property.Name]["TitleEn"];
                founded2.TitleEn = (string) idSystemTexts[property.Name]["TitleEn"];
            } else {
                var newText = new ActMaster.Param {
                    Id = property.Name,
                    TitleEn = (string) idSystemTexts[property.Name]["TitleEn"]
                };

                systemTexts.Add(newText);
                systemTexts2.Add(newText);
                Plugin.Logger.LogInfo($"{newText.Id} ditambahkan!");
            }
        }
    }
}