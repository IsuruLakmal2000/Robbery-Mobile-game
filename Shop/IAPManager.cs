using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    public static string MONEY_PACK = "money_pack_1";
    public static string GEM_PACK = "gem_pack_1";
    public static string REMOVE_ADS = "remove_ads";

    void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(MONEY_PACK, ProductType.Consumable);
        builder.AddProduct(GEM_PACK, ProductType.Consumable);
        builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    // Buy Methods
    public void BuyMoneyPack() => BuyProductID(MONEY_PACK);
    public void BuyGemPack() => BuyProductID(GEM_PACK);
    public void BuyRemoveAds() => BuyProductID(REMOVE_ADS);

    void BuyProductID(string productId)
    {
        if (storeController != null && storeController.products != null)
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("Product not found or not available.");
            }
        }
        else
        {
            Debug.Log("StoreController not initialized.");
        }
    }

    // Callbacks
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error) =>
        Debug.Log("IAP Initialization Failed: " + error);

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"IAP Initialization Failed: {error}, Message: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == MONEY_PACK)
        {
            Debug.Log("Money pack purchased!");
            int totalCoinCount = PlayerPrefs.GetInt("total_money", 0);
            totalCoinCount += 100000000;
            // Add 100m coins

            // Update Firebase asynchronously
            string userId = PlayerPrefs.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                UpdateNetworthAsync(userId, totalCoinCount); // Call an async void helper method
            }

            PlayerPrefs.SetInt("total_money", totalCoinCount);
            PlayerPrefs.Save();
            Debug.Log("total_money: " + totalCoinCount);
        }
        else if (args.purchasedProduct.definition.id == GEM_PACK)
        {
            Debug.Log("Gem pack purchased!");
            int totalGemCount = PlayerPrefs.GetInt("total_gem", 0);
            totalGemCount += 400; // Add 400 gems
            PlayerPrefs.SetInt("total_gem", totalGemCount);
            PlayerPrefs.Save();
            Debug.Log("Total gem count: " + totalGemCount);
        }
        else if (args.purchasedProduct.definition.id == REMOVE_ADS)
        {
            Debug.Log("Ads removed!");
            PlayerPrefs.SetInt("ads_removed", 1);

            int totalGemCount = PlayerPrefs.GetInt("total_gem", 0);
            totalGemCount += 400;
            PlayerPrefs.SetInt("total_gem", totalGemCount);

            int totalCoinCount = PlayerPrefs.GetInt("total_money", 0);
            totalCoinCount += 100000000;
            PlayerPrefs.SetInt("total_money", totalCoinCount);

            PlayerPrefs.Save();
        }

        return PurchaseProcessingResult.Complete;
    }

    // Helper method to handle Firebase updates asynchronously
    private async void UpdateNetworthAsync(string userId, int totalCoinCount)
    {
        try
        {
            await FirebaseController.instance.UpdateCurrentNetworth(userId, totalCoinCount);
            Debug.Log("Firebase net worth updated successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error updating Firebase net worth: " + ex.Message);
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed: {product.definition.id}, Reason: {failureReason}");
    }

    public static bool IsAdsRemoved()
    {
        return PlayerPrefs.GetInt("ads_removed", 0) == 1;
    }
}
