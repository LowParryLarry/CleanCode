namespace WebShopCleanCode.SortingAlgorithms;
using WebShop;

public static class QuickSort
{
    private static int Partition(List<Product> products, string sortBy, bool ascending, int l, int r)
    {
        var pivot = products[r];
        var i = l - 1;

        for (var j = l; j < r; j++)
        {
            bool shouldSwap;

            if (sortBy == "Name")
            {
                int comparison = products[j].Name.CompareTo(pivot.Name);
                shouldSwap = ascending ? comparison < 0 : comparison > 0;
            }
            else 
            {
                shouldSwap = ascending ? products[j].Price < pivot.Price : products[j].Price > pivot.Price;
            }

            if (shouldSwap)
            {
                i++;
                (products[i], products[j]) = (products[j], products[i]);
            }
        }
        (products[i + 1], products[r]) = (products[r], products[i + 1]);

        return i + 1;
    }

    private static void Sort(List<Product> products, string sortBy, bool ascending, int l, int r)
    {
        if (l >= r)
        {
            return;
        }

        var p = Partition(products, sortBy, ascending, l, r);
        Sort(products, sortBy, ascending, l, p - 1);
        Sort(products, sortBy, ascending, p + 1, r);
    }
    
    public static void Sort(List<Product> products, string sortBy, bool ascending)
    {
        Sort(products, sortBy, ascending, 0, products.Count - 1);
    }
}