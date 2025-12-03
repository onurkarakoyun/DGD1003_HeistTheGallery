using UnityEditor;
using UnityEngine;

public static class CreatePaintingPrefab
{
    [MenuItem("Tools/Create Painting Prefab")]
    static void Create()
    {
        var go = new GameObject("Painting");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 100;

        var tex = Resources.Load<Texture2D>("painting");
        if (tex != null)
        {
            var sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
            sr.sprite = sp;
        }
        else
        {
            var w = 128; var h = 128; var t = new Texture2D(w, h, TextureFormat.RGBA32, false);
            for (int y = 0; y < h; y++) for (int x = 0; x < w; x++) t.SetPixel(x, y, ((x / 16 + y / 16) % 2 == 0) ? new Color(0.85f,0.85f,0.95f,1f) : new Color(0.15f,0.5f,0.9f,1f));
            t.Apply(false, true);
            var sp = Sprite.Create(t, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 64f);
            sr.sprite = sp;
        }

        if (sr.sprite != null)
        {
            var desiredWidth = 2.0f;
            var currentWidth = sr.sprite.bounds.size.x;
            var scale = desiredWidth / currentWidth;
            go.transform.localScale = new Vector3(scale, scale, 1f);
        }

        var bc = go.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        if (sr.sprite != null)
        {
            bc.size = sr.sprite.bounds.size;
            bc.offset = Vector2.zero;
        }

        go.AddComponent<StealablePainting>();

        System.IO.Directory.CreateDirectory("Assets/Prefabs");
        var path = "Assets/Prefabs/Painting.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        AssetDatabase.Refresh();
    }
}
