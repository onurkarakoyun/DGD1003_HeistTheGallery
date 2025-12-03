using UnityEditor;
using UnityEngine;

public static class ColumnsTools
{
    [MenuItem("Tools/Columns/Normalize Width (x0.6)")]
    static void NormalizeWidth()
    {
        var targets = Selection.transforms;
        if (targets == null || targets.Length == 0)
            targets = GameObject.FindObjectsOfType<Transform>();

        foreach (var t in targets)
        {
            var n = t.gameObject.name.ToLowerInvariant();
            if (!n.Contains("kolon")) continue;
            var s = t.localScale;
            t.localScale = new Vector3(s.x * 0.6f, s.y, s.z);
            var c = t.GetComponent<BoxCollider2D>();
            if (c != null)
            {
                var cs = c.size;
                c.size = new Vector2(cs.x * 0.6f, cs.y);
                EditorUtility.SetDirty(c);
            }
            EditorUtility.SetDirty(t);
        }
    }

    [MenuItem("Tools/Columns/Align Paintings To Nearest Columns")]
    static void AlignPaintings()
    {
        var paintings = GameObject.FindObjectsOfType<StealablePainting>();
        var columns = GameObject.FindObjectsOfType<Transform>();
        foreach (var p in paintings)
        {
            var px = p.transform.position.x;
            float bestDx = float.MaxValue; Transform best = null;
            foreach (var t in columns)
            {
                var n = t.gameObject.name.ToLowerInvariant();
                if (!n.Contains("kolon")) continue;
                var dx = Mathf.Abs(t.position.x - px);
                if (dx < bestDx) { bestDx = dx; best = t; }
            }
            if (best != null)
            {
                var pos = p.transform.position;
                p.transform.position = new Vector3(best.position.x, pos.y, pos.z);
                EditorUtility.SetDirty(p.transform);
            }
        }
    }
}
