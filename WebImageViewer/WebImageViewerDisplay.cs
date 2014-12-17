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

    using System.Collections;
    using UnityEngine;

    #endregion

    public class WebImageViewerDisplay : MonoBehaviour
    {
        #region Fields

        private Rect screenRect = new Rect(0.0f, 0.0f, 300.0f, 0.0f);
        private Texture2D texture;

        #endregion

        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void OnDestroy()
        {
            if (this.texture != null)
            {
                Destroy(this.texture);
            }
        }

        public void OnGUI()
        {
            this.screenRect = GUILayout.Window(this.GetInstanceID(), this.screenRect, this.OnWindow, "Web Image Viewer", HighLogic.Skin.window);
        }

        public void SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Destroy(this);
                return;
            }

            this.StartCoroutine(this.Process(url));
        }

        private void OnWindow(int windowId)
        {
            GUI.skin = HighLogic.Skin;
            if (this.texture == null)
            {
                GUILayout.Label("Downloading...");
            }
            else
            {
                GUILayout.Box(this.texture, new[] { GUILayout.MaxWidth(Screen.width * 0.75f), GUILayout.MaxHeight(Screen.height * 0.75f) });
            }

            if (GUILayout.Button("CLOSE"))
            {
                Destroy(this);
            }
            GUI.DragWindow();
            GUI.skin = null;
        }

        private IEnumerator Process(string url)
        {
            var www = new WWW(url);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                print(www.error);
                Destroy(this);
            }

            this.texture = www.texture;
        }
    }
}