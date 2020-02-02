using UnityEngine;
using DG.Tweening;
namespace Items
{
    public class ItemProduce : ItemBase
    {
        float spawnTime;
        float lifeSpan = 10;
        public PlantType plantType;
        public bool isCurrentlyInteractable { get; }
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
        }
        public void Update() {
            if(spawnTime + lifeSpan < Time.time) {
                if(heldBy!=null)
                    heldBy.ThrowHeld(0.1f);
                transform.DOScale(0, 0.5f).onComplete = () => { Destroy(gameObject); };
                Destroy(this);
            }
        }
    }
}