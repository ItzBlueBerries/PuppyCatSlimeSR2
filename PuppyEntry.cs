using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.UI;
using MelonLoader;
using PuppyCatSlimeSR2;

[assembly: MelonInfo(typeof(PuppyEntry), "PuppyCat Slime", "1.1.0", "FruitsyOG")]
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]
namespace PuppyCatSlimeSR2
{
    internal class PuppyEntry : MelonMod
    {
        internal static List<MarketUI.PlortEntry> plortsToPatch = new List<MarketUI.PlortEntry>();
        internal static List<EconomyDirector.ValueMap> valueMapsToPatch = new List<EconomyDirector.ValueMap>();

        public override void OnInitializeMelon() => PuppyCat.InitializeSlime();

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            switch (sceneName)
            {
                //Here you have SystemContext loaded and here you registering a translations
                case "SystemCore":
                    {
                        break;
                    }
                //Here you have loaded assets like Identifiables and GameContext
                case "GameCore":
                    {
                        PuppyCat.LoadSlime(sceneName);
                        break;
                    }
                //Here you have loaded SceneContext
                case "zoneCore":
                    {
                        PuppyCat.LoadSlime(sceneName);
                        break;
                    }
            }

            OnSceneManager.OnZoneScene(sceneName);
        }
    }
}