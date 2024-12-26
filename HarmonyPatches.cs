using ngov3;
using NGO;
using UniRx;
using UnityEngine.UI;
using TMPro;
using HarmonyLib;
using UnityEngine;

namespace NSOID;

[HarmonyPatch(typeof(Boot))]
public static class BootPatches {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Boot.Awake))]
    public static void AwakePrefix(Boot __instance) {
        __instance.gameObject.AddComponent<AltBoot>();
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Boot.BootOS))]
    public static void BootOSPrefix() {
        SingletonMonoBehaviour<Settings>.Instance.ChangeLanguage(LanguageType.EN);
        SingletonMonoBehaviour<Settings>.Instance.Save();

        Plugin.Logger.LogInfo("Pengaturan bahasa diubah paksa ke Bahasa Indonesia");
    }

    [HarmonyPrefix]
    [HarmonyPatch("waitAccept")]
    public static bool WaitAcceptPrefix(ref Boot __instance, CanvasGroup ___Login, CanvasGroup ___ChooseDay, CanvasGroup ___ChooseUser, CanvasGroup ___Welcome, Button ___Ok, Button ___Cancel, TMP_Text ___Caution_Header, TMP_Text ___Caution_Nakami) {
        AltBoot altBoot = __instance.GetComponent<AltBoot>();
        Boot instance = __instance;
        if (altBoot.shownModIntro) return true;

        Plugin.Logger.LogInfo("Window intro diinisiasi!");
        AudioManager.Instance.PlaySeByType(SoundType.SE_Boot_Caution, false);
        AudioManager.Instance.PlayBgmById("BGM_OP_PV", true);
        ___Login.alpha = 1f;
        ___Login.interactable = true;
        ___ChooseDay.alpha = 0f;
        ___ChooseDay.interactable = false;
        ___ChooseDay.blocksRaycasts = false;
        ___ChooseUser.alpha = 0f;
        ___ChooseUser.interactable = false;
        ___ChooseUser.blocksRaycasts = false;
        ___Welcome.alpha = 0f;
        ___Welcome.interactable = false;
        ___Welcome.blocksRaycasts = false;
        if (SingletonMonoBehaviour<ControllerGuideManager>.Instance != null)
        {
            SingletonMonoBehaviour<ControllerGuideManager>.Instance.IsReady = true;
        }

        Plugin.Logger.LogInfo("Window intro ditampilkan!");
        Rect r = (___Caution_Header.transform as RectTransform).rect;
        r.width = 999;
        ___Caution_Header.overflowMode = TextOverflowModes.Overflow;
        ___Caution_Header.enableWordWrapping = false;
        
        ___Caution_Header.text = NgoEx.SystemTextFromTypeString("System_idIntroTitle", LanguageType.EN);
        ___Caution_Nakami.text = NgoEx.SystemTextFromTypeString("System_idIntro", LanguageType.EN);

        Vector3 returnPos = ___Ok.transform.position;
        Vector3 centerPos = (___Ok.transform.position + ___Cancel.transform.position) / 2f;

        ___Ok.transform.position = centerPos;
        ___Ok.OnClickAsObservable().Subscribe(delegate (Unit _) {
            if (altBoot.shownModIntro) return;

            ___Ok.transform.position = returnPos;
            ___Cancel.gameObject.SetActive(true);
            Plugin.Logger.LogInfo("Window intro Ditutup!");

            altBoot.shownModIntro = true;
            new Traverse(instance).Method("waitAccept").GetValue();
        }).AddTo(___Ok);

        ___Cancel.gameObject.SetActive(false);
        Plugin.Logger.LogInfo("Window intro selesai!");

        return false;
    }
}

[HarmonyPatch(typeof(ControllPanelView))]
public static class ControllPanelViewPatches {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ControllPanelView.Start))]
    public static void StartPrefix(ToggleGroup ____languageToggleGroup, TMP_Text ____languageTitle) {
        ____languageTitle.gameObject.SetActive(false);
        ____languageToggleGroup.gameObject.SetActive(false);
    }
}