using PixelCrushers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Game.Scripts.Common.SaveData
{
    public class GameMetaDataSaver : Saver
    {
        private byte[] _lastScreenshot;

        public override string RecordData()
        {
            var data = new GameMetaData();

            data.SceneName = SceneManager.GetActiveScene().name;

            data.TimeStamp = DateTime.Now.ToString("dddd, dd MMMM yyyy");

            data.Picture = _lastScreenshot;

            return SaveSystem.Serialize(data);
        }

        public override void ApplyData(string s)
        {
            // Do nothing. We only record the data for the save/load menu.
            // There's no need to re-apply it when loading a saved game.
        }

        private byte[] GetScreenshotBytes()
        {
            //var screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
            var screenshotTexture = new Texture2D(Screen.width, Screen.height);

            screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            
            screenshotTexture.Apply();

            return ImageConversion.EncodeToJPG(screenshotTexture);
        }

        public void TakeScreenshot()
        {
            _lastScreenshot = GetScreenshotBytesWithoutUI();
        }

        private byte[] GetScreenshotBytesWithoutUI(Camera targetCamera = null)
        {
            if (targetCamera == null) targetCamera = Camera.main;

            RenderTexture rt = new RenderTexture(
                Screen.width,
                Screen.height,
                24,
                RenderTextureFormat.ARGB32
            );

            RenderTexture currentRT = targetCamera.targetTexture;
            RenderTexture currentActive = RenderTexture.active;

            targetCamera.targetTexture = rt;
            targetCamera.Render();

            RenderTexture.active = rt;

            Texture2D screenshotTexture = new Texture2D(
                rt.width,
                rt.height,
                TextureFormat.RGB24,
                false
            );

            screenshotTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            screenshotTexture.Apply();

            targetCamera.targetTexture = currentRT;
            RenderTexture.active = currentActive;

            byte[] bytes = ImageConversion.EncodeToJPG(screenshotTexture);

            Destroy(rt);
            Destroy(screenshotTexture);

            return bytes;
        }
    }
}
