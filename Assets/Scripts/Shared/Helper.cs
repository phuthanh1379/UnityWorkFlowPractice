using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static GameObject SpawnPrefab(string path, Vector3 position, Quaternion rotation, Transform parent = null) {
        GameObject spawned = GameObject.Instantiate(Resources.Load<GameObject>(path), position, rotation);

        if (parent != null) {
            spawned.transform.SetParent(parent);
        }

        return spawned;
    }
}
