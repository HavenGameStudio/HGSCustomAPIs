using FeedBacks;
using UnityEngine;

namespace HGS.LastBastion.Feedbacks
{
    public class GameObjectFeedback : Feedback
    {
        public enum FeedBackType { Instantiate, Disable, Destroy, Enable }

        [SerializeField] private FeedBackType feedBackType;
        [SerializeField] private GameObject prefabToInstantiate;
        [SerializeField] private Transform parentOfTheInstantiatedGO;
        [SerializeField] private Transform transformOfTheInstantiatedGO;

        [SerializeField] private GameObject targetGameObject;
        //CREATE A BOOL OR A ENUM CONDITION WHEATHER THE INSTANTIATED GO WILL PARENT OR JUST COPY THE POSITION
        public override void Play()
        {
            switch (feedBackType)
            {
                case FeedBackType.Instantiate:
                    InstantiateGameObj();
                    break;
                case FeedBackType.Disable:
                    DisableObj();
                    break;
                default:
                    break;
            }
        }


        private void InstantiateGameObj()
        {
            GameObject spawnedPrefab = Instantiate(prefabToInstantiate);
            if (transformOfTheInstantiatedGO == null)
            {
                transformOfTheInstantiatedGO = transform;
            }
            spawnedPrefab.transform.position = transformOfTheInstantiatedGO.position;
            spawnedPrefab.name.Replace("Clone", "");
            //SET THE PARENT OF THE INSTANTIATED 
            //SET THE POSITION OF THE INSTANTIATED GO

        }

        void DisableObj()
        {
            if (targetGameObject == null) return;

            targetGameObject.SetActive(false);
        }

        void EnableObj()
        {
            if (targetGameObject == null) return;

            targetGameObject.SetActive(true);
        }

        void DestroyObj()
        {
            if (targetGameObject == null) return;

            Destroy(targetGameObject);
        }


    }
}
