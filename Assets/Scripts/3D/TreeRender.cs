using System.Collections.Generic;
using DataObjects;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace _3D
{
    /**
     * 
     */
    public class TreeRender : MonoBehaviour
    {
        //SINGLETON
        private static TreeRender treeRender;

        private GameObject ConiferousTree;
        private GameObject LeafyTree;


        /**
         * Singleton for tree render
         */
        public static TreeRender Get()
        {
            if (treeRender == null)
                treeRender = (new GameObject("TreeRender")).AddComponent<TreeRender>();
            return treeRender;
        }

        /**
         * Create trees on the terrain
         */
        public void GenerateTrees(LatLngObject middleMapPoint, List<TreeObject> trees)
        {
            Vector3 middleMapXyz;
            ConiferousTree = Resources.Load("3DObjects/Trees/Prefabs/Fir_Tree", typeof(GameObject)) as GameObject;
            LeafyTree = Resources.Load("3DObjects/Trees/Prefabs/Poplar_Tree", typeof(GameObject)) as GameObject;
            //default position, if map does not contain any building or road
            if (middleMapPoint.Equals(new LatLngObject()))
                middleMapXyz = Vector3.zero;
            else
                middleMapXyz = Converter.ConvertLatLngToXyz(middleMapPoint);

            var treeCount = Main.Settings.NumberOfTrees;
            const float treeScale = 0.06f;
            var terrain = TerrainRender.Get().Terrain;
            var random = new Random();
            //GENERATE RANDOM TREE POSITIONS
            if (trees.Count == 0)
            {
                for (var i = 0; i < treeCount; i++)
                {
                    var decimalRandomPartX = random.NextDouble();
                    var decimalRandomPartZ = random.NextDouble();
                    const int limitX = (TerrainUtils.MapWidth / 2) - 2;
                    const int limitZ = (TerrainUtils.MapHeight / 2) - 2;
                    var randomX = random.Next((int) (middleMapXyz.x - limitX), (int) (middleMapXyz.x + limitX));
                    // + 2 a + 1 kvuli korekcim umisteni na hranice terenu
                    var randomZ = random.Next((int) (middleMapXyz.z - limitZ + 2), (int) (middleMapXyz.z + limitZ + 1));
                    var randomDecX = (float) (randomX + decimalRandomPartX);
                    var randomDecZ = (float) (randomZ + decimalRandomPartZ);
                    var randomPos = new Vector3(randomDecX, 0, randomDecZ);
                    var terrainY = terrain.SampleHeight(randomPos);
                    randomPos.y = terrainY;

                    var randNumberTree = random.Next(0, 2);
                    if (!IsPointInCollision(randomPos))
                    {
                        GameObject tree;
                        switch (randNumberTree)
                        {
                            case 0:
                                tree = Instantiate(LeafyTree);
                                break;
                            default:
                                tree = Instantiate(ConiferousTree);
                                break;
                        }

                        var treeTransform = tree.transform;
                        treeTransform.position = randomPos;
                        treeTransform.localScale = new Vector3(treeScale, treeScale, treeScale);
                    }
                    else
                    {
                        --i;
                    }
                }
            }
            //CUSTOM TREE POSITIONS
            else
            {
                foreach (var currentTree in trees)
                {
                    var treePos = Converter.ConvertLatLngToXyz(currentTree.LatLngCoordinate);
                    //if tree is outside the map
                    if (TerrainUtils.IsObjectOutsideMap(treePos, middleMapXyz))
                        continue;

                    //set y position from terrain
                    var terrainY = terrain.SampleHeight(treePos);
                    treePos.y = terrainY;

                    var randNumberTree = random.Next(0, 2);
                    GameObject tree;
                    switch (randNumberTree)
                    {
                        case 0:
                            tree = Instantiate(LeafyTree);
                            break;
                        default:
                            tree = Instantiate(ConiferousTree);
                            break;
                    }

                    var treeTransform = tree.transform;
                    treeTransform.position = treePos;
                    treeTransform.localScale = new Vector3(treeScale, treeScale, treeScale);
                }
            }
        }

        /**
         * Check if object at the position is in collision with any other objects
         */
        private bool IsPointInCollision(Vector3 randomPoint)
        {
            //radius around object for collision detect
            const float radius = 0.09f;

            //DEBUG: START
            //SHOW RADIUS
//            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            sphere.transform.localScale = new Vector3(radius, radius, radius);
//            sphere.transform.position = randomPoint;
            //DEBUG: END

            var colliders = Physics.OverlapSphere(randomPoint, radius);
            if (colliders.Length > 0)
                return true;

            return false;
        }
    }
}