using BlazorApp.Models.Entities;
using BlazorApp.Models.Models;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Web.Components.Pages.Product
{
    public partial class CreateProduct
    {
        public ProductModel Model { get; set; } = new();
        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public async Task Submit()
        {
            var res = await ApiClient.PostAsync<BaseResponseModel, ProductModel>("/api/Product/create-new-product", Model);

            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Create product successfully");
                NavigationManager.NavigateTo("/product");
            }
        }
    }
}
