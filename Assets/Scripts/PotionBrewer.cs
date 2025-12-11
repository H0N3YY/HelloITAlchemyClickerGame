using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PotionBrewer : MonoBehaviour
{
    [Header("Sloty składników")]
    public Transform potSlot1;
    public Transform potSlot2;

    [Header("Prefab itemu wynikowego")]
    public GameObject draggableItemPrefab;

    [Header("Ekwipunek docelowy")]
    public Transform inventoryParent;
    [Header("Warunki craftingu")]
    public bool fireActive = false;
    public float fireDuration = 30f;
    private float fireTimer = 0f;
    public magicBallScript manaSource;
    public int fireManaCost = 50;
    public AudioSource boil;

    [SerializeField] private AnimatorToggler fireAnimatorToggler;

    [Header("Lista przepisów")]
    public List<Recipe> recipes;

    
    private void Update()
    {
        if (fireActive)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                boil.Stop();
                fireActive = false;
                fireAnimatorToggler.DisableAnimator();
                Debug.Log("Ognisko zgasło");
            }
        }
    }
    public void StartFire()
    {
        if (manaSource == null)
        {
            Debug.LogError("Brak przypiętego magicBallScript!");
            return;
        }
        int baseFireCost = 50;
        int adjustedCost = Mathf.CeilToInt(baseFireCost * manaSource.GetPriceMultiplier());
        if (manaSource.SpendMana(adjustedCost))
        {
            fireActive = true;
            fireAnimatorToggler.EnableAnimator();
            boil.Play();
            fireTimer = fireDuration;
            Debug.Log("Ognisko rozpalone! Mana pobrana");
        }
        else
        {
            Debug.Log($"Za mało many. Aktualnie: {manaSource.GetCurrentMana()} / {adjustedCost}");
        }
    }



    private void TryBrew()
    {
        DragableItem item1 = potSlot1.GetComponentInChildren<DragableItem>();
        DragableItem item2 = potSlot2.GetComponentInChildren<DragableItem>();

        if (item1 == null || item2 == null) return;

        ScriptableItem s1 = item1.item;
        ScriptableItem s2 = item2.item;

        foreach (Recipe recipe in recipes)
        {
            if ((recipe.ingredient1 == s1 && recipe.ingredient2 == s2) ||
                (recipe.ingredient1 == s2 && recipe.ingredient2 == s1))
            {
                Debug.Log($"Ugotowano: {s1.name} + {s2.name} == {recipe.result.name}");

                SpawnPotion(recipe.result);

                Destroy(item1.gameObject);
                Destroy(item2.gameObject);
                return;
            }
        }

        Debug.Log("Brak pasującego przepisu");
        item1.ReturnToInventory();
        item2.ReturnToInventory();
    }
    public void StirPot()
    {
        if (!fireActive)
        {
            Debug.Log("Ognisko zgasło. Nie można craftować!");
            return;
        }


        if (potSlot1.childCount > 0 && potSlot2.childCount > 0)
        {
            TryBrew();
        }
        else
        {
            Debug.Log("Potrzebne dwa składniki!");
        }
    }


    private void SpawnPotion(ScriptableItem resultItem)
    {
        foreach (Transform slot in inventoryParent)
        {
            if (slot.childCount == 0)
            {
                GameObject newItem = Instantiate(draggableItemPrefab, slot);
                newItem.transform.localPosition = Vector3.zero;

                DragableItem di = newItem.GetComponent<DragableItem>();
                if (di != null)
                {
                    di.item = resultItem;
                    di.count = 1;
                    di.InitialiseItem(resultItem);
                }

                Debug.Log("Potka trafiła do ekwipunku.");
                break;
            }
        }
    }

    // Wewnętrzna klasa Recipe — dostępna z Inspectora
    [System.Serializable]
    public class Recipe
    {
        public ScriptableItem ingredient1;
        public ScriptableItem ingredient2;
        public ScriptableItem result;
    }
}
