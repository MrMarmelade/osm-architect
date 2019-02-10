using DataObjects;
using UnityEngine;
using Utils;
using _3D;

namespace Controls
{
    /** 
     * Listenery pro kliknutí a ukázání na budovu
     *
     * Connected into unity editor in parent object of the building(roof)
     */
    public class GameObjectClick : MonoBehaviour
    {
        /**
         * Calls after click on the building roof
         */
        private void OnMouseDown()
        {
            RemoveBuilding();
        }

        /**
         * Mouse is pointing on the building roof
         */
        private void OnMouseOver()
        {
            //remove building with gamepad button
            if (Input.GetKeyUp(KeyCode.Joystick1Button4))
                RemoveBuilding();
        }

        /**
         * Remove building from the map
         */
        private void RemoveBuilding()
        {
            var buildings = BuildingRender.Get().Buildings;
            var batchedBuildings = BuildingRender.Get().BatchedBuildings;
            var index = buildings.FindIndex(threeDimObject => threeDimObject.Name.Equals(gameObject.name));
            Main.RemovedObjects.Add(buildings[index]);
            batchedBuildings.Remove(gameObject);
            Destroy(gameObject);
            MenuController.Get().ChangeAddressBarVisibility(false);
        }

        /**
         *  Pokud ukáže myš na objekt, změní objekt barvu
         */
        private void OnMouseEnter()
        {
            var buildingAddress = new AddressObject();
            foreach (var building in BuildingRender.Get().Buildings)
            {
                if (building.Name.Equals(gameObject.name))
                    buildingAddress = building.Address;
            }

            MenuController.Get().SetAddressBarText(buildingAddress);
            MenuController.Get().ChangeAddressBarVisibility(true);
            BuildingRender.Get().ChangeBuildingSurfaceColor(gameObject, ColorUtils.OnMouse);
        }

        /**
         * Pokud myš zmizí z objekty, barva objektu se změní na původní
         */
        private void OnMouseExit()
        {
            MenuController.Get().ChangeAddressBarVisibility(false);
            BuildingRender.Get().ChangeBuildingSurfaceColor(gameObject, ColorUtils.DefaultBuilding);
        }
    }
}