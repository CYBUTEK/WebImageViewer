// 
//     Copyright (C) 2014 CYBUTEK
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

namespace WebImageViewer
{
    #region Using Directives

    using UnityEngine;

    #endregion

    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class WebImageViewer : MonoBehaviour
    {
        #region Fields

        private static string url = string.Empty;
        private ApplicationLauncherButton button;
        private bool hasCentred;
        private Rect screenRect = new Rect(0, 0, 300.0f, 0.0f);
        private bool visible;

        #endregion

        public void AddButton()
        {
            if (this.button == null)
            {
                this.button = ApplicationLauncher.Instance.AddModApplication(this.OnTrue, this.OnFalse, null, null, null, null, ApplicationLauncher.AppScenes.ALWAYS, GameDatabase.Instance.GetTexture("WebImageViewer/Textures/Icon", false));
            }
        }

        public void Awake()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(this.AddButton);
        }

        public void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(this.AddButton);
            if (this.button != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.button);
            }
        }

        public void OnFalse()
        {
            this.visible = false;
        }

        public void OnGUI()
        {
            if (!this.visible) return;

            this.screenRect = GUILayout.Window(this.GetInstanceID(), this.screenRect, this.OnWindow, "Web Image Viewer");
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER && !this.hasCentred && this.screenRect.width > 0.0f && this.screenRect.height > 0.0f)
            {
                this.hasCentred = true;
                this.screenRect.center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            }
        }

        public void OnTrue()
        {
            this.visible = true;
        }

        private void OnWindow(int windowId)
        {
            url = GUILayout.TextField(url);
            if (GUILayout.Button("View Image"))
            {
                var display = this.gameObject.AddComponent<WebImageViewerDisplay>();
                display.SetUrl(url);
            }
            GUI.DragWindow();
        }
    }
}