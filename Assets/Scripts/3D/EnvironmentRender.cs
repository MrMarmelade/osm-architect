using UnityEngine;

namespace _3D
{
    /**
     * Methods for SkyBox managing
     */
    public class EnvironmentRender : MonoBehaviour
    {
        //SINGLETON
        private static EnvironmentRender environmentRender;


        private bool EasterEggActivated;


        /**
         * Singleton for SkyBox render
         */
        public static EnvironmentRender Get()
        {
            if (environmentRender == null)
                environmentRender = (new GameObject("EnvironmentRender")).AddComponent<EnvironmentRender>();
            return environmentRender;
        }

        /**
         * Change SkyBox
         */
        public void SetSkyBox()
        {
            if (Input.GetKeyUp(KeyCode.E) && !EasterEggActivated)
            {
                RoadRender.Get().ChangeRoadColors(false);
                var directionalLight = GameObject.Find("Directional Light").GetComponent(typeof(Light)) as Light;
                RenderSettings.skybox = Resources.Load("Textures/Skybox", typeof(Material)) as Material;
                directionalLight.intensity = 0f;
                EasterEggActivated = true;
                return;
            }

            if (Input.GetKeyUp(KeyCode.E) && EasterEggActivated)
            {
                RoadRender.Get().ChangeRoadColors(true);
                var directionalLight = GameObject.Find("Directional Light").GetComponent(typeof(Light)) as Light;
                RenderSettings.skybox = Resources.Load("Textures/Skybox_default", typeof(Material)) as Material;
                directionalLight.intensity = 1f;
                EasterEggActivated = false;
                return;
            }
        }
    }
}