using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class MathHelper : MonoBehaviour
    {
        public static Vector2Int RoundVector2ToInt(Vector2 start)
        {
            return new Vector2Int(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y));
        }


    }
}
