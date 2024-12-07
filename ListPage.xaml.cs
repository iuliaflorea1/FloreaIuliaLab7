using FloreaIuliaLab7.Models;

namespace FloreaIuliaLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}


    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });

    }
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var sl = (ShopList)BindingContext;
        Product p;
        if (listView.SelectedItem != null)
        {
            p = listView.SelectedItem as Product;
            var lp = new ListProduct()
            {
                ShopListID = sl.ID,
                ProductID = p.ID
            };
            await App.Database.SaveListProductAsync(lp);
            p.ListProducts = new List<ListProduct> { lp };

            await Navigation.PopAsync();
        }
    }

    async void OnRemoveButtonClicked(object sender, EventArgs e)
    {
        var product = listView.SelectedItem as Product;
        if (product != null)
        {
            var confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to remove ?", "Yes", "No");
            if (confirm)
            {
                await App.Database.DeleteProductAsync(product);
                listView.ItemsSource = await App.Database.GetProductsAsync();
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select an item to remove.", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }

}