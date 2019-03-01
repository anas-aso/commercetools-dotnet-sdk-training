using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products;
using commercetools.Sdk.Domain.Query;
using System.Threading.Tasks;

namespace Training
{
    /// <summary>
    /// Add Product to Category Exercise
    /// </summary>
    public class Exercise5 : IExercise
    {
        private readonly IClient _commercetoolsClient;
        
        private static Random random = new Random();
        
        public Exercise5(IClient commercetoolsClient)
        {
            this._commercetoolsClient =
                commercetoolsClient ?? throw new ArgumentNullException(nameof(commercetoolsClient));
        }
        public void Execute()
        {
            AddProductToCategory();
        }
        
        /// <summary>
        /// Bad Solution
        /// </summary>
        private void AddProductToCategory()
        {
            string productKey = "Product1-Key-123";
            string categoryKey = "Category1-Key-123";
            
            
            //retrieve category by key
            Category category =
                _commercetoolsClient.ExecuteAsync(new GetByKeyCommand<Category>(categoryKey)).Result;
            
            //retrieve product by key
            Product product =
                _commercetoolsClient.ExecuteAsync(new GetByKeyCommand<Product>(productKey)).Result;
            
            //Create AddToCategoryUpdateAction
            AddToCategoryUpdateAction addToCategoryUpdateAction = new AddToCategoryUpdateAction()
            {
                OrderHint = this.RandomSortOrder(),
                Category =  new ResourceIdentifier() { Key = categoryKey}
            };
            
            List<UpdateAction<Product>> updateActions = new List<UpdateAction<Product>>();
            updateActions.Add(addToCategoryUpdateAction);
            
            //Add the category to the product
            Product retrievedProduct = _commercetoolsClient
                .ExecuteAsync(new UpdateByKeyCommand<Product>(productKey, product.Version, updateActions))
                .Result;
            
            //show product categories
            foreach (var cat in retrievedProduct.MasterData.Current.Categories)
            {
                Console.WriteLine($"Category ID {cat.Id}");
            }
        }
        
        /// <summary>
        /// String representing a number which is greater than 0 and smaller than 1. It should start with “0.” and should not end with “0”.
        /// </summary>
        /// <returns></returns>
        public string RandomSortOrder()
        {
            int append = 5;//hack to not have a trailing 0 which is not accepted in sphere
            return "0." + random.Next() + append;
        }
    }
}