
#define DEBUG_PUSH
using System.Collections;

using UnityEngine;

namespace AppsMinistry
{

    public sealed class FPSDisplay : MonoBehaviour
    {

        public int v_TargetFrameRate = 60;
        private float updateInterval = 0.5f;
        private int x_location = 100;
        private int y_location = 20;

        private float lastInterval;
        private float frames = 0;
        private float fps;
        private float v_Timer;
        private float v_StartTime;

        private string _buildTimestamp;

        private float[] fpsAll;
        private float fpsAlltemp;
        private byte f;
        private byte t;
        private byte c;

        private void Awake()
        {
            useGUILayout = false;
            fpsAll = new float[100];
            Application.targetFrameRate = 60;
        }
#if DEBUG_PUSH
		private void OnGUI()
		{
			GUI.color = Color.white;
			GUI.Label(new Rect(Screen.width - x_location, Screen.height - y_location - 15, 100, 30), string.Format("FPS: {0:F2}", fps));
			GUI.Label(new Rect(Screen.width - x_location, Screen.height - y_location, 100, 30), string.Format("FPS_A: {0:F2}", fpsAlltemp));

			GUI.Label(new Rect(Screen.width - x_location - 100, Screen.height - y_location, 100, 30), string.Format("Time: {0:F2}", v_Timer));

			if (Debug.isDebugBuild)
				GUI.Label(new Rect(Screen.width - x_location - 250, Screen.height - y_location, 150, 30), string.Format("Memory: {0:F2}mb", Profiler.usedHeapSize / (1024.0f * 1024.0f)));

			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(new Rect(10, Screen.height - y_location * 3, 280, 30), string.Format("Screen resolution: {0}x{1}", Screen.width, Screen.height));
			//GUI.Label(new Rect(10, Screen.height - y_location * 2, 280, 30), string.Format("Build info: {0} {1}", AppConfiguration.Name, _buildTimestamp));
			GUI.Label(new Rect(10, Screen.height - y_location, 280, 30), string.Format("Scene: {0}", Application.loadedLevelName));
		}
#endif

        private void Start()
        {
            lastInterval = Time.realtimeSinceStartup;
            v_StartTime = lastInterval;
            frames = 0;
            Application.targetFrameRate = v_TargetFrameRate;


            TextAsset buildTimestampAsset = (TextAsset)Resources.Load("BuildTimestamp", typeof(TextAsset));
            if (buildTimestampAsset != null)
            {
                _buildTimestamp = buildTimestampAsset.text;
            }
        }


        private void Update()
        {
            ++frames;

            float timeNow = Time.realtimeSinceStartup;
            v_Timer = timeNow - v_StartTime;
            if (timeNow > lastInterval + updateInterval)
            {
                fps = frames / (timeNow - lastInterval);
                frames = 0;
                lastInterval = timeNow;

                fpsAll[f] = fps;
                if (c != 100)
                {
                    c = 0;
                    for (t = 0; t < fpsAll.Length; t++)
                    {
                        fpsAlltemp += fpsAll[t];

                        if (fpsAll[t] != 0.0f)
                        {
                            c++;
                        }

                    }
                    fpsAlltemp /= (float)c;
                }
                else
                {
                    for (t = 0; t < fpsAll.Length; t++)
                    {
                        fpsAlltemp += fpsAll[t];
                    }
                    fpsAlltemp /= 100f;
                }

                f++;

                if (f == fpsAll.Length)
                {
                    f = 0;
                }

            }
        }
    }
}