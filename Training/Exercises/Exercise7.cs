using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Carts.UpdateActions;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Query;

namespace Training
{
    /// <summary>
    /// Add Product to Cart
    /// </summary>
    public class Exercise7 : IExercise
    {
        private readonly IClient _commercetoolsClient;
        
        public Exercise7(IClient commercetoolsClient)
        {
            this._commercetoolsClient =
                commercetoolsClient ?? throw new ArgumentNullException(nameof(commercetoolsClient));
        }
        public void Execute()
        {
            AddProductToCart();
        }

        private void AddProductToCart()
        {
            string productVariantSku = "test123";
            string cartID = "2825e31e-7bd1-4b68-ace3-e8252e2a062c";

            // retrieve cart by Id
            Cart cart =
                _commercetoolsClient.ExecuteAsync(new GetByIdCommand<Cart>(new Guid(cartID))).Result;
            
            // get lineItemDraft and create AddLineItemUpdateAction
            var lineItemDraft = this.GetLineItemDraft(productVariantSku, 3);
            AddLineItemUpdateAction addLineItemUpdateAction = new AddLineItemUpdateAction()
            {
                LineItem = lineItemDraft
            };
            
            List<UpdateAction<Cart>> updateActions = new List<UpdateAction<Cart>>();
            updateActions.Add(addLineItemUpdateAction);

            Cart retrievedCart = _commercetoolsClient
                .ExecuteAsync(new UpdateByIdCommand<Cart>(new Guid(cart.Id),
                    cart.Version, updateActions))
                .Result;
            
            foreach (var lineItem in retrievedCart.LineItems)
            {
                Console.WriteLine($"LineItem Name: {lineItem.Name["en"]}, Quantity: {lineItem.Quantity}");
            }
            
        }
        public LineItemDraft GetLineItemDraft(string productVariantSku, int quantity = 1)
        {
            LineItemDraft lineItemDraft = new LineItemDraft();
            lineItemDraft.Sku = productVariantSku;
            lineItemDraft.Quantity = quantity;
            return lineItemDraft;
        }
    }
}