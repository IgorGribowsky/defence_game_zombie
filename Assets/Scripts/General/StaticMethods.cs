using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking.Types;
using static UnityEditor.Progress;

public static class StaticMethods
{
    public static GameObject GetNearestObject(GameObject destGameObject, GameObject[] gameObjects, out float distance, bool ignoreHeight = false)
    {
        distance = Mathf.Infinity;
        GameObject nearestObject = null;

        var destPos = destGameObject.transform.position;
        if (ignoreHeight)
        {
            destPos.y = 0f;
        }
        //Поиск ближайшего противника
        foreach (GameObject o in gameObjects)
        {
            var oPos = o.transform.position;
            if (ignoreHeight)
            {
                oPos.y = 0f;
            }

            float curDistance = (oPos - destPos).sqrMagnitude;
            if (curDistance < distance)
            {
                nearestObject = o;
                distance = curDistance;
            }
        }

        return nearestObject;
    }

    public static bool HasWallsBetween(GameObject o1, GameObject o2)
    {

        var fromPosition = o1.transform.position;
        var toPosition = o2.transform.position;
        var direction = toPosition - fromPosition;

        var hits = Physics.RaycastAll(fromPosition, direction, direction.magnitude);
        return hits.Any(h =>
        {
            var obj = h.collider.gameObject;

            return obj.HasTag() && obj.GetComponent<Tags>().Wall;
        });
    }

}

public static class GameObjectExtensions
{
    public static bool HasTag(this GameObject obj)
    {
        return obj.tag == "WithTag";
    }
} 