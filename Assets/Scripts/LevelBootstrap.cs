using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class LevelBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        var player = Object.FindAnyObjectByType<PlayerController>();
        if (player != null) player.gameObject.tag = "Player";

        var existingLight = Object.FindAnyObjectByType<Light2D>();
        if (existingLight == null)
        {
            var lightGo = new GameObject("Global Light 2D");
            var light = lightGo.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;
            light.intensity = 1f;
        }

        

        var gm = Object.FindAnyObjectByType<GameManager>();
        if (gm == null)
        {
            var gmGo = new GameObject("GameManager");
            gm = gmGo.AddComponent<GameManager>();
        }
        var hud = Object.FindAnyObjectByType<HUD>();
        if (hud == null)
        {
            var hudGo = new GameObject("HUD");
            hudGo.AddComponent<HUD>();
        }

        var left = new GameObject("LeftBoundary");
        var leftCol = left.AddComponent<BoxCollider2D>();
        left.transform.position = new Vector3(-9f, 0f, 0f);
        leftCol.size = new Vector2(0.5f, 20f);

        var exit = new GameObject("ExitDoor");
        var exitSr = exit.AddComponent<SpriteRenderer>();
        exitSr.sortingOrder = 90;
        var dw = 64;
        var dh = 128;
        var dtex = new Texture2D(dw, dh, TextureFormat.RGBA32, false);
        for (int y = 0; y < dh; y++)
        {
            for (int x = 0; x < dw; x++)
            {
                var c = new Color(0.2f, 0.8f, 0.2f, 1f);
                dtex.SetPixel(x, y, c);
            }
        }
        dtex.Apply(false, true);
        var dsp = Sprite.Create(dtex, new Rect(0, 0, dw, dh), new Vector2(0.5f, 0.5f), 100f);
        exitSr.sprite = dsp;
        exit.transform.position = new Vector3(21.5f, -1.5f, 0f);
        var exitCol = exit.AddComponent<BoxCollider2D>();
        exitCol.isTrigger = true;
        if (exitSr.sprite != null)
        {
            var s = exitSr.sprite.bounds.size;
            exitCol.size = s;
        }
        exit.AddComponent<ExitDoor>();

        var right = new GameObject("RightBoundary");
        var rightCol = right.AddComponent<BoxCollider2D>();
        right.transform.position = new Vector3(22.5f, 0f, 0f);
        rightCol.size = new Vector2(0.5f, 20f);

        var srs = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
        var minX = float.MaxValue;
        var maxX = float.MinValue;
        foreach (var r in srs)
        {
            var b = r.bounds;
            if (b.min.x < minX) minX = b.min.x;
            if (b.max.x > maxX) maxX = b.max.x;
        }
        if (minX < float.MaxValue && maxX > float.MinValue)
        {
            left.transform.position = new Vector3(minX - 0.5f, 0f, 0f);
            right.transform.position = new Vector3(maxX + 0.5f, 0f, 0f);
            exit.transform.position = new Vector3(maxX - 0.5f, exit.transform.position.y, 0f);
        }

        

        
    }
}
