using HarmonyLib;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppyCatSlimeSR2
{
    internal class OnSceneManager
    {
        public static void OnZoneScene(string sceneName)
        {
            switch (sceneName.Contains("zoneFields"))
            {
                case true:
                    {
                        IEnumerable<DirectedSlimeSpawner> source = UnityEngine.Object.FindObjectsOfType<DirectedSlimeSpawner>();
                        foreach (DirectedSlimeSpawner directedSlimeSpawner in source)
                        {
                            foreach (DirectedActorSpawner.SpawnConstraint spawnConstraint in directedSlimeSpawner.constraints)
                            {
                                spawnConstraint.slimeset.members = spawnConstraint.slimeset.members.AddItem(new SlimeSet.Member
                                {
                                    prefab = PuppyCat.puppyDefinition.prefab,
                                    identType = PuppyCat.puppyDefinition,
                                    weight = 0.3f
                                }).ToArray();
                            }
                        }
                        break;
                    }
            }

            switch (sceneName.Contains("zoneGorge"))
            {
                case true:
                    {
                        IEnumerable<DirectedSlimeSpawner> source = UnityEngine.Object.FindObjectsOfType<DirectedSlimeSpawner>();
                        foreach (DirectedSlimeSpawner directedSlimeSpawner in source)
                        {
                            foreach (DirectedActorSpawner.SpawnConstraint spawnConstraint in directedSlimeSpawner.constraints)
                            {
                                spawnConstraint.slimeset.members = spawnConstraint.slimeset.members.AddItem(new SlimeSet.Member
                                {
                                    prefab = PuppyCat.puppyDefinition.prefab,
                                    identType = PuppyCat.puppyDefinition,
                                    weight = 0.3f
                                }).ToArray();
                            }
                        }
                        break;
                    }
            }

            switch (sceneName.Contains("zoneStrand"))
            {
                case true:
                    {
                        IEnumerable<DirectedSlimeSpawner> source = UnityEngine.Object.FindObjectsOfType<DirectedSlimeSpawner>();
                        foreach (DirectedSlimeSpawner directedSlimeSpawner in source)
                        {
                            foreach (DirectedActorSpawner.SpawnConstraint spawnConstraint in directedSlimeSpawner.constraints)
                            {
                                spawnConstraint.slimeset.members = spawnConstraint.slimeset.members.AddItem(new SlimeSet.Member
                                {
                                    prefab = PuppyCat.puppyDefinition.prefab,
                                    identType = PuppyCat.puppyDefinition,
                                    weight = 0.3f
                                }).ToArray();
                            }
                        }
                        break;
                    }
            }

            switch (sceneName.Contains("zoneBluffs"))
            {
                case true:
                    {
                        IEnumerable<DirectedSlimeSpawner> source = UnityEngine.Object.FindObjectsOfType<DirectedSlimeSpawner>();
                        foreach (DirectedSlimeSpawner directedSlimeSpawner in source)
                        {
                            foreach (DirectedActorSpawner.SpawnConstraint spawnConstraint in directedSlimeSpawner.constraints)
                            {
                                spawnConstraint.slimeset.members = spawnConstraint.slimeset.members.AddItem(new SlimeSet.Member
                                {
                                    prefab = PuppyCat.puppyDefinition.prefab,
                                    identType = PuppyCat.puppyDefinition,
                                    weight = 0.3f
                                }).ToArray();
                            }
                        }
                        break;
                    }
            }
        }
    }
}
