using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TheCollection.Web.Models;
using TheCollection.Web.Services;

namespace TheCollection.Web.Controllers
{
    [Route("api/[controller]")]
    public class BagsController : Controller
    {
        private readonly DocumentClient documentDbClient;

        public BagsController(DocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        [HttpGet()]
        public async Task<SearchResult<Bag>> Bags([FromQuery] string searchterm = "")
        {
            var bagsRepository = new DocumentDBRepository<Bag>(documentDbClient, "TheCollection", "Bags");
            IEnumerable<Bag> bags;
            if (searchterm != "")
            {
                //var searchterms = searchterm.ToLower().Split(' ').Select(term => $"ARRAY_CONTAINS(bag.tags, '{term}')").ToArray();
                //bags = await bagsRepository.GetItemsAsync(BagFilters.TagContainsAll(searchterms));
                //bags = await bagsRepository.GetItemsAsync(null, bag => bag.Tags.Where(tag => tag.Contains(searchterm)).Select(tag => bag));
                //var sql = $"SELECT TOP 100 VALUE bag FROM Bags bag WHERE {searchterms.Aggregate((current, next) => current + " AND " + next)}";
                bags = await bagsRepository.GetItemsAsync<Bag>(searchterm);
                //bags = await bagsRepository.GetItemsAsync(BagFilters.TagContains2(searchterms));
            }
            else
            {
                bags = await bagsRepository.GetItemsAsync();
            }

            return new SearchResult<Bag>
            {
                count = await bagsRepository.GetRowCountAsync<Bag>(searchterm),
                data = bags.OrderBy(bag => bag.Brand.Name)
                        .ThenBy(bag => bag.Hallmark)
                        .ThenBy(bag => bag.Serie)
                        .ThenBy(bag => bag.Flavour)
            };
        }

        [HttpGet("{id}")]
        public async Task<Bag> Bag(string id)
        {
            var bagsRepository = new DocumentDBRepository<Bag>(documentDbClient, "TheCollection", "Bags");
            return await bagsRepository.GetItemAsync(id);
        }
    }

    public class SearchResult<T>
    {
        public long count { get; set; }
        public IEnumerable<T> data { get; set; }
    }

    public static class BagFilters
    {
        public static Expression<Func<Bag, bool>> TagContainsAll(string[] searchterms)
        {
            return (bag => (bool)UserDefinedFunctionProvider.Invoke("containsLikeAll", bag.Tags, searchterms));
        }

        public static Expression<Func<Bag, IEnumerable<Bag>>> TagContains(string[] searchterms)
        {
            //return (bag => bag.Tags.Where(tag => tag.Contains(searchterms[0])).Select(tag => bag));
            return (bag => bag.Tags.Where(tag => searchterms.Contains(tag)).Select(tag => bag));
        }

        public static IEnumerable<Expression<Func<Bag, IEnumerable<Bag>>>> TagContains3(string[] searchterms)
        {
            //return (bag => bag.Tags.Where(tag => tag.Contains(searchterms[0])).Select(tag => bag));
            for (int i = 0; i < searchterms.Length; i++)
                yield return (bag => bag.Tags.Where(tag => tag == searchterms[i]).Select(tag => bag));
        }

        public static Expression<Func<Bag, bool>> TagContains2(string[] searchterms)
        {
            return (bag => searchterms.All(searchterm => bag.Tags.Contains(searchterm)));
        }
    }
}
