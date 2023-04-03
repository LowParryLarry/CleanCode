namespace WebShopCleanCode.Static;
using WebShop;

public static class QuickSort
{
    private static int Partition(List<Product> products, string sortBy, bool ascending, int l, int r)
    {
        var pivot = products[r];
        var swapMarker = l - 1;

        for (var current = l; current < r; current++)
        {
            bool shouldSwap;

            if (sortBy == "Name")
            {
                int comparison = products[current].Name.CompareTo(pivot.Name);
                shouldSwap = ascending ? comparison < 0 : comparison > 0;
            }
            else 
            {
                shouldSwap = ascending ? products[current].Price < pivot.Price : products[current].Price > pivot.Price;
            }

            if (shouldSwap)
            {
                swapMarker++;
                (products[swapMarker], products[current]) = (products[current], products[swapMarker]);
            }
        }
        (products[swapMarker + 1], products[r]) = (products[r], products[swapMarker + 1]);

        return swapMarker + 1;
    }

    private static void Sort(List<Product> products, string sortBy, bool ascending, int l, int r)
    {
        if (l >= r)
        {
            return;
        }

        var partitioned = Partition(products, sortBy, ascending, l, r);
        Sort(products, sortBy, ascending, l, partitioned - 1);
        Sort(products, sortBy, ascending, partitioned + 1, r);
    }
    
    public static void Sort(List<Product> products, string sortBy, bool ascending)
    {
        Sort(products, sortBy, ascending, 0, products.Count - 1);
    }
}