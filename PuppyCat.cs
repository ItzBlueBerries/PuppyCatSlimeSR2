using HarmonyLib;
using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppSystem;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Playables;
using static PuppyCatSlimeSR2.PuppyEntry;

#nullable disable
namespace PuppyCatSlimeSR2
{
    internal class PuppyCat
    {
        public static SlimeDefinition puppyDefinition;
        public static IdentifiableType puppyCatPlortType;
        public static Color[] puppyCatPalette =
        {
            Utility.LoadHex("#eef0e7"), // White/Gray?
            Utility.LoadHex("#f9c5c5"), // Pink
            Utility.LoadHex("#a79063"), // Brown
            Utility.LoadHex("#5e4e35") // Darker Brown
        };

        public static void InitializeSlime()
        {
            puppyDefinition = ScriptableObject.CreateInstance<SlimeDefinition>();
            puppyDefinition.hideFlags |= HideFlags.HideAndDontSave;
            puppyDefinition.name = "PuppyCat";
            puppyDefinition.color = puppyCatPalette[1];

            puppyCatPlortType = ScriptableObject.CreateInstance<IdentifiableType>();
            puppyCatPlortType.hideFlags |= HideFlags.HideAndDontSave;
            puppyCatPlortType.IsPlort = true;
            puppyCatPlortType.name = "PuppyCatPlort";
            puppyCatPlortType.color = puppyCatPalette[0];
        }

        public static void LoadSlime(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        puppyCatPlortType.localizedName = HarmonyPatches.LocalizationDirectorLoadTablePatch.AddTranslation("Actor", "l.puppycat_plort", "PuppyCat Plort");
                        puppyDefinition.localizedName = HarmonyPatches.LocalizationDirectorLoadTablePatch.AddTranslation("Actor", "l.puppycat_slime", "PuppyCat Slime");

                        #region PUPPYCAT_PLORT
                        puppyCatPlortType.prefab = Utility.PrefabUtils.CopyPrefab(Utility.Get<IdentifiableType>("PinkPlort").prefab);
                        puppyCatPlortType.prefab.name = "PuppyCatPlort";
                        puppyCatPlortType.prefab.GetComponent<Identifiable>().identType = puppyCatPlortType;
                        puppyCatPlortType.icon = Utility.CreateSprite(Utility.LoadImage("Assets.puppycat_plort_ico"));

                        Material plortMaterial = UnityEngine.Object.Instantiate(Utility.Get<GameObject>("plortPink").GetComponent<MeshRenderer>().sharedMaterial);
                        plortMaterial.SetColor("_TopColor", puppyCatPalette[1]);
                        plortMaterial.SetColor("_MiddleColor", puppyCatPalette[2]);
                        plortMaterial.SetColor("_BottomColor", puppyCatPalette[0]);
                        puppyCatPlortType.prefab.GetComponent<MeshRenderer>().sharedMaterial = plortMaterial;

                        plortsToPatch.Add(new MarketUI.PlortEntry
                        {
                            identType = puppyCatPlortType
                        });
                        valueMapsToPatch.Add(new EconomyDirector.ValueMap
                        {
                            accept = puppyCatPlortType.prefab.GetComponent<Identifiable>(),
                            fullSaturation = 5,
                            value = 120
                        });
                        #endregion

                        #region PUPPYCAT_SLIME
                        puppyDefinition.prefab = Utility.PrefabUtils.CopyPrefab(Utility.Get<GameObject>("slimeTabby"));
                        puppyDefinition.prefab.name = "PuppyCatSlime";

                        puppyDefinition.prefab.GetComponent<Identifiable>().tag = "PuppyCat Slime";
                        puppyDefinition.prefab.GetComponent<Identifiable>().identType = puppyDefinition;
                        puppyDefinition.prefab.GetComponent<SlimeEat>().slimeDefinition = puppyDefinition;

                        puppyDefinition.Diet = UnityEngine.Object.Instantiate(Utility.Get<SlimeDefinition>("Pink")).Diet;
                        puppyDefinition.Diet.MajorFoodGroups = new SlimeEat.FoodGroup[] { SlimeEat.FoodGroup.MEAT, SlimeEat.FoodGroup.FRUIT, SlimeEat.FoodGroup.VEGGIES };
                        puppyDefinition.Diet.ProduceIdents = new IdentifiableType[] { puppyCatPlortType };
                        puppyDefinition.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, puppyDefinition);

                        puppyDefinition.properties = UnityEngine.Object.Instantiate(Utility.Get<SlimeDefinition>("Pink").properties);
                        puppyDefinition.defaultPropertyValues = UnityEngine.Object.Instantiate(Utility.Get<SlimeDefinition>("Pink")).defaultPropertyValues;

                        SlimeAppearance slimeAppearance = UnityEngine.Object.Instantiate(Utility.Get<SlimeAppearance>("TabbyDefault"));
                        SlimeAppearanceApplicator slimeAppearanceApplicator = puppyDefinition.prefab.GetComponent<SlimeAppearanceApplicator>();
                        slimeAppearance.name = "PuppyCatDefault";
                        slimeAppearanceApplicator.Appearance = slimeAppearance;
                        slimeAppearanceApplicator.SlimeDefinition = puppyDefinition;

                        // EARS
                        GameObject earsObject = new GameObject("puppyCat_ears");
                        earsObject.hideFlags |= HideFlags.HideAndDontSave;

                        earsObject.AddComponent<SkinnedMeshRenderer>().sharedMesh = AB.puppycat_slime.LoadFromObject<MeshFilter>("ears").sharedMesh;

                        earsObject.AddComponent<SlimeAppearanceObject>().hideFlags |= HideFlags.HideAndDontSave;
                        earsObject.GetComponent<SlimeAppearanceObject>().RootBone = SlimeAppearance.SlimeBone.JiggleTop;
                        earsObject.GetComponent<SlimeAppearanceObject>().ParentBone = SlimeAppearance.SlimeBone.None;
                        earsObject.GetComponent<SlimeAppearanceObject>().AttachedBones = new SlimeAppearance.SlimeBone[0].AddDefaultBones();
                        earsObject.GetComponent<SlimeAppearanceObject>().IgnoreLODIndex = true;
                        UnityEngine.Object.DontDestroyOnLoad(earsObject.GetComponent<SlimeAppearanceObject>());

                        slimeAppearance.Structures[1].Element = ScriptableObject.CreateInstance<SlimeAppearanceElement>();
                        slimeAppearance.Structures[1].Element.Name = "puppyCatEars";
                        slimeAppearance.Structures[1].Element.Prefabs = new SlimeAppearanceObject[] { earsObject.GetComponent<SlimeAppearanceObject>() };
                        slimeAppearance.Structures[1].Element.Type = SlimeAppearanceElement.ElementType.Top;
                        slimeAppearance.Structures[1].SupportsFaces = false;

                        // TAIL
                        GameObject tailObject = new GameObject("puppyCat_tail");
                        tailObject.hideFlags |= HideFlags.HideAndDontSave;

                        tailObject.AddComponent<SkinnedMeshRenderer>().sharedMesh = AB.puppycat_slime.LoadFromObject<MeshFilter>("tail").sharedMesh;

                        tailObject.AddComponent<SlimeAppearanceObject>().hideFlags |= HideFlags.HideAndDontSave;
                        tailObject.GetComponent<SlimeAppearanceObject>().RootBone = SlimeAppearance.SlimeBone.JiggleTop;
                        tailObject.GetComponent<SlimeAppearanceObject>().ParentBone = SlimeAppearance.SlimeBone.None;
                        tailObject.GetComponent<SlimeAppearanceObject>().AttachedBones = new SlimeAppearance.SlimeBone[0].AddDefaultBones();
                        tailObject.GetComponent<SlimeAppearanceObject>().IgnoreLODIndex = true;
                        UnityEngine.Object.DontDestroyOnLoad(tailObject.GetComponent<SlimeAppearanceObject>());

                        slimeAppearance.Structures[3].Element = ScriptableObject.CreateInstance<SlimeAppearanceElement>();
                        slimeAppearance.Structures[3].Element.Name = "puppyCatTail";
                        slimeAppearance.Structures[3].Element.Prefabs = new SlimeAppearanceObject[] { tailObject.GetComponent<SlimeAppearanceObject>() };
                        slimeAppearance.Structures[3].Element.Type = SlimeAppearanceElement.ElementType.Tail;
                        slimeAppearance.Structures[3].SupportsFaces = false;

                        Material slimeMaterial = UnityEngine.Object.Instantiate(Utility.Get<SlimeAppearance>("PinkDefault").Structures[0].DefaultMaterials[0]);
                        slimeMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        slimeMaterial.SetColor("_TopColor", puppyCatPalette[0]);
                        slimeMaterial.SetColor("_MiddleColor", Color.grey);
                        slimeMaterial.SetColor("_BottomColor", puppyCatPalette[0]);
                        slimeMaterial.SetColor("_SpecColor", Color.grey);
                        slimeAppearance.Structures[0].DefaultMaterials[0] = slimeMaterial;

                        Material earsMaterial = UnityEngine.Object.Instantiate(Utility.Get<SlimeAppearance>("PinkDefault").Structures[0].DefaultMaterials[0]);
                        earsMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        earsMaterial.SetColor("_TopColor", puppyCatPalette[2]);
                        earsMaterial.SetColor("_MiddleColor", puppyCatPalette[1]);
                        earsMaterial.SetColor("_BottomColor", puppyCatPalette[2]);
                        slimeAppearance.Structures[1].DefaultMaterials[0] = earsMaterial;

                        Material tailMaterial = UnityEngine.Object.Instantiate(Utility.Get<SlimeAppearance>("PinkDefault").Structures[0].DefaultMaterials[0]);
                        tailMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        tailMaterial.SetColor("_TopColor", puppyCatPalette[2]);
                        tailMaterial.SetColor("_MiddleColor", puppyCatPalette[0]);
                        tailMaterial.SetColor("_BottomColor", puppyCatPalette[0]);
                        slimeAppearance.Structures[3].DefaultMaterials[0] = tailMaterial;

                        slimeAppearance.Face = UnityEngine.Object.Instantiate(Utility.Get<SlimeAppearance>("TabbyDefault").Face);
                        slimeAppearance.Face.name = "PuppyCatFace";

                        SlimeExpressionFace[] expressionFaces = new SlimeExpressionFace[0];
                        foreach (SlimeExpressionFace slimeExpressionFace in slimeAppearance.Face.ExpressionFaces)
                        {
                            Material angryEyes = null;
                            Material angryMouth = null;

                            if (slimeExpressionFace.Eyes)
                                angryEyes = UnityEngine.Object.Instantiate(slimeAppearance.Face.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.Angry).Eyes);
                            if (slimeExpressionFace.Mouth)
                                angryMouth = UnityEngine.Object.Instantiate(slimeAppearance.Face.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.Angry).Mouth);

                            if (angryEyes)
                            {
                                angryEyes.SetColor("_EyeRed", puppyCatPalette[3]);
                                angryEyes.SetColor("_EyeGreen", puppyCatPalette[3]);
                                angryEyes.SetColor("_EyeBlue", puppyCatPalette[3]);
                            }
                            if (angryMouth)
                            {
                                angryMouth.SetColor("_MouthBot", puppyCatPalette[3]);
                                angryMouth.SetColor("_MouthMid", puppyCatPalette[3]);
                                angryMouth.SetColor("_MouthTop", puppyCatPalette[3]);
                            }
                            slimeExpressionFace.Eyes = angryEyes;
                            slimeExpressionFace.Mouth = angryMouth;
                            expressionFaces = expressionFaces.AddToArray(slimeExpressionFace);
                        }
                        slimeAppearance.Face.ExpressionFaces = expressionFaces;
                        slimeAppearance.Face.OnEnable();

                        slimeAppearance.Icon = Utility.CreateSprite(Utility.LoadImage("Assets.puppycat_slime_ico"));
                        slimeAppearance.SplatColor = puppyCatPalette[0];
                        slimeAppearance.ColorPalette = new SlimeAppearance.Palette
                        {
                            Ammo = puppyCatPalette[1],
                            Top = puppyCatPalette[0],
                            Middle = puppyCatPalette[1],
                            Bottom = puppyCatPalette[2]
                        };
                        puppyDefinition.AppearancesDefault = new SlimeAppearance[] { slimeAppearance };
                        slimeAppearance.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
                case "zoneCore":
                    {
                        SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.RegisterDependentAppearances(Utility.Get<SlimeDefinition>("PuppyCat"), Utility.Get<SlimeDefinition>("PuppyCat").AppearancesDefault[0]);
                        SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.UpdateChosenSlimeAppearance(Utility.Get<SlimeDefinition>("PuppyCat"), Utility.Get<SlimeDefinition>("PuppyCat").AppearancesDefault[0]);
                        SRSingleton<GameContext>.Instance.SlimeDefinitions.Slimes = SRSingleton<GameContext>.Instance.SlimeDefinitions.Slimes.AddItem(puppyDefinition).ToArray();
                        SRSingleton<GameContext>.Instance.SlimeDefinitions.slimeDefinitionsByIdentifiable.Add(puppyDefinition, puppyDefinition);
                        break;
                    }
            }
        }
    }
}
#nullable restore