using System;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Api.Client;
using commercetools.Api.Models.Products;
using commercetools.Base.Client;

namespace Training
{
    /// <summary>
    /// PageQueryRequests Optimized, Query all Products in optimized way
    /// https://docs.commercetools.com/http-api#paging
    /// </summary>
    public class Task06B : IExercise
    {
        private readonly IClient _client;
        
        public Task06B(IClient client)
        {
            this._client = client;
        }

        public async Task ExecuteAsync()
        {
            string lastId = null ;int pageSize = 2;int currentPage = 1; bool lastPage = false;
            ProductPagedQueryResponse response = null;

            while (!lastPage)
            {
                var where = lastId != null ? $"id>\"{lastId}\"" : null;

                Console.WriteLine($"Show Results of Page {currentPage}");
                foreach (var product in response.Results)
                {
                    Console.WriteLine($"{product.MasterData.Current.Name["en"]}");
                }
                Console.WriteLine("///////////////////////");
                currentPage++;
                lastId = response.Results.Last().Id;
                lastPage = response.Results.Count < pageSize;
            }
        }
    }
}