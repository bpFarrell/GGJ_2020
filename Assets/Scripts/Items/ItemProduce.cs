using UnityEngine;
using DG.Tweening;
namespace Items
{
    public class ItemProduce : ItemBase
    {
        float spawnTime;
        float lifeSpan = 20;
        public PlantType plantType;
        public GameObject progressRing;
        public Material ringMat;
        public bool isCurrentlyInteractable { get; }
        private void Awake()
        {
            //Quick hack to not have selection outline show up on progress bar.
            Vector3 scale = progressRing.transform.localScale;
            progressRing.transform.localScale = Vector3.zero;
            base.Awake();
            progressRing.transform.localScale = scale;
        }
        public void Interact(PlayerController player)
        {
            throw new System.NotImplementedException();
        }

        public void EnterRange(PlayerController player)
        {
            throw new System.NotImplementedException();
        }

        public void LeaveRange(PlayerController player)
        {
            throw new System.NotImplementedException();
        }
        public void Start() {
            spawnTime = Time.time;
            MeshRenderer mr = progressRing.GetComponent<MeshRenderer>();
            ringMat = new Material(mr.material);
            mr.material = ringMat;
            if (CarManager.onFirstCar) spawnTime = 9999;
        }
        public void Update() {
            float t = (Time.time - spawnTime) / lifeSpan;
            progressRing.transform.eulerAngles = new Vector3(90, 0, 0);
            progressRing.transform.localPosition = new Vector3(0, 0.15f, 0);
            ringMat.SetFloat("_T", 1 - t - 1);
            if(spawnTime + lifeSpan < Time.time) {
                if(heldBy!=null)
                    heldBy.ThrowHeld(0.1f);
                transform.DOScale(0, 0.5f).onComplete = () => { FinishClear(); };
                //Destroy(this);
            }
        }
        public void FinishClear() {
            Destroy(gameObject);
        }
    }
}