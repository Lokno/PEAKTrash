using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PEAKTrash;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    string prevSceneName = string.Empty;
    const float binThickness = 0.1f;

    private void Awake()
    {
        Log = Logger;
        Log.LogInfo($"Plugin {Name} is loaded! Better Airport Trash Bin Collision");
    }

    private void ReplaceTrashColliders(GameObject obj)
    {
        BoxCollider collider = obj.GetComponent<BoxCollider>();

        if (collider != null)
        {
            Vector3 center = collider.center;
            Vector3 size = collider.size;

            Object.Destroy(collider);

            // Front
            BoxCollider frontCollider = obj.AddComponent<BoxCollider>();
            frontCollider.center = new Vector3(center.x, center.y + (size.y - binThickness) * 0.5f, center.z);
            frontCollider.size = new Vector3(size.x, binThickness, size.z);

            // Back
            BoxCollider backCollider = obj.AddComponent<BoxCollider>();
            backCollider.center = new Vector3(center.x, center.y - (size.y - binThickness) * 0.5f, center.z);
            backCollider.size = new Vector3(size.x, binThickness, size.z);

            // Left
            BoxCollider leftCollider = obj.AddComponent<BoxCollider>();
            leftCollider.center = new Vector3(center.x - (size.x - binThickness) * 0.5f, center.y, center.z);
            leftCollider.size = new Vector3(binThickness, size.y - binThickness * 2, size.z);

            // Right
            BoxCollider rightCollider = obj.AddComponent<BoxCollider>();
            rightCollider.center = new Vector3(center.x + (size.x - binThickness) * 0.5f, center.y, center.z);
            rightCollider.size = new Vector3(binThickness, size.y - binThickness * 2, size.z);
        }
    }

    private void Update()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string name = activeScene.name;

        if (name != prevSceneName)
        {
            prevSceneName = name;

            if ("Airport" == name)
            {
                foreach (GameObject obj in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
                {
                    if (obj.name.StartsWith("Trash"))
                    {
                        ReplaceTrashColliders(obj);
                    }
                }
            }
        }
    }
}
