using Harmony;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

#nullable disable
internal class Utility
{
    public static T Get<T>(string name) where T : UnityEngine.Object
    {
        return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault((T found) => found.name.Equals(name));
    }

    public static Color LoadHex(string hexCode)
    {
        ColorUtility.TryParseHtmlString(hexCode, out var returnedColor);
        return returnedColor;
    }

    public static Texture2D LoadImage(string filename)
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(executingAssembly.GetName().Name + "." + filename + ".png");
        byte[] array = new byte[manifestResourceStream.Length];
        manifestResourceStream.Read(array, 0, array.Length);
        Texture2D texture2D = new Texture2D(1, 1);
        ImageConversion.LoadImage(texture2D, array);
        texture2D.filterMode = FilterMode.Bilinear;
        return texture2D;
    }

    public static Sprite CreateSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1f);
    }

    public static class PrefabUtils
    {
        static PrefabUtils()
        {
            DisabledParent.gameObject.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(DisabledParent.gameObject);
            DisabledParent.gameObject.hideFlags |= HideFlags.HideAndDontSave;
        }

        public static GameObject CopyPrefab(GameObject prefab)
        {
            return UnityEngine.Object.Instantiate(prefab, DisabledParent);
        }

        public static Transform DisabledParent = new GameObject("DeactivedObject").transform;
    }

    public static class Spawner
    {
        public static void ToSpawn(string name)
        {
            SRBehaviour.InstantiateActor(Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault((GameObject x) => x.name == name), SRSingleton<SceneContext>.Instance.RegionRegistry.CurrentSceneGroup, SRSingleton<SceneContext>.Instance.Player.transform.position, Quaternion.identity, false, SlimeAppearance.AppearanceSaveSet.NONE, SlimeAppearance.AppearanceSaveSet.NONE);
        }
    }
}

internal static class UtilityExtensions
{
    public static void ChangeEyes(this SlimeAppearance appearance, SlimeFace.SlimeExpression face, SlimeFace.SlimeExpression[] ignoredFaces)
    {
        Material eyes = appearance.Face.ExpressionFaces.First(x => x.SlimeExpression == face).Eyes;
        for (int i = 0; i < appearance.Face.ExpressionFaces.Length; i++)
        {
            if (ignoredFaces.Contains(appearance.Face.ExpressionFaces[i].SlimeExpression)) continue;
            appearance.Face.ExpressionFaces[i].Eyes = eyes;
        }
    }

    public static void ChangeMouth(this SlimeAppearance appearance, SlimeFace.SlimeExpression face, SlimeFace.SlimeExpression[] ignoredFaces)
    {
        Material mouth = appearance.Face.ExpressionFaces.First(x => x.SlimeExpression == face).Mouth;
        for (int i = 0; i < appearance.Face.ExpressionFaces.Length; i++)
        {
            if (ignoredFaces.Contains(appearance.Face.ExpressionFaces[i].SlimeExpression)) continue;
            appearance.Face.ExpressionFaces[i].Mouth = mouth;
        }
    }

    public static SlimeAppearance.SlimeBone[] AddDefaultBones(this SlimeAppearance.SlimeBone[] slimeBones)
    {
        slimeBones = new SlimeAppearance.SlimeBone[]
        {
            SlimeAppearance.SlimeBone.JiggleBack,
            SlimeAppearance.SlimeBone.JiggleBottom,
            SlimeAppearance.SlimeBone.JiggleFront,
            SlimeAppearance.SlimeBone.JiggleLeft,
            SlimeAppearance.SlimeBone.JiggleRight,
            SlimeAppearance.SlimeBone.JiggleTop
        };

        return slimeBones;
    }

    public static T LoadFromObject<T>(this AssetBundle bundle, string name) where T : UnityEngine.Object
    { return bundle.LoadAsset(name).Cast<GameObject>().GetComponentInChildren<T>(); }
}
#nullable restore