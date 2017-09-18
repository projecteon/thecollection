using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheCollection.Business.Tea;
using Xunit;

namespace TheCollection.Business.Tests.Unit.Tea
{
    [Trait("Bag", "Tags are created")]
    public class BagTagsTests
    {
        Bag Bag { get; }
        RefValue BagTypeRef { get; }
        RefValue BrandRef { get; }
        RefValue CountryRef { get; }
        Searchable SearchableBag { get; }
        

        public BagTagsTests()
        {
            BagTypeRef = new RefValue { Id = System.Guid.NewGuid().ToString(), Name = "Paper" };
            BrandRef = new RefValue { Id = System.Guid.NewGuid().ToString(), Name = "Twinning" };
            CountryRef = new RefValue { Id = System.Guid.NewGuid().ToString(), Name = "Norway" };
            Bag = new Bag {
                Id = System.Guid.NewGuid().ToString(),
                MainID = 999,
                Flavour = "The as At Was Earl Grey Thé",
                Serie = "was some Series",
                Hallmark = "was some Hallmark",
                SerialNumber = "was some serial number",
                BagType = BagTypeRef,
                Brand = BrandRef,
                Country = CountryRef,
                InsertDate = "1985-08-22",
                ImageId = "imageid"
            };
            SearchableBag = new Searchable(Bag);
        }

        [Fact(DisplayName = "Tags are distinct")]
        public void TagsAreDistinct()
        {
            Assert.Equal(SearchableBag.Tags.Count(), SearchableBag.Tags.Distinct().Count());
        }

        [Fact(DisplayName = "Tag contains main id")]
        public void ContainsMainId()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == $"{Bag.MainID}"));
        }

        [Fact(DisplayName = "Tag contains bagtype name in lowercase")]
        public void ContainsBagTypeNameLowerCase()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == BagTypeRef.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag contains brand name in lowercase")]
        public void ContainsBrandNameLowerCase()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == BrandRef.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag contains country name in lowercase")]
        public void ContainsCountryNameLowerCase()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == CountryRef.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId()
        {
            Assert.False(SearchableBag.Tags.Any(tag => tag == Bag.Id));
            Assert.False(SearchableBag.Tags.Any(tag => tag == Bag.Id.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain insert date")]
        public void NotContainsInsertDate()
        {
            Assert.False(SearchableBag.Tags.Any(tag => tag == Bag.InsertDate));
            Assert.False(SearchableBag.Tags.Any(tag => tag == Bag.InsertDate.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain image id")]
        public void NotContainsImageId()
        {
            Assert.False(SearchableBag.Tags.Any(tag => tag == Bag.ImageId));
            Assert.False(SearchableBag.Tags.Any(tag => tag == Bag.ImageId.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain stopwords except for words with accents parsed to stop words")]
        public void NotContainsStopWords()
        {
            var stopWords = new[] { "as", "at", "was" };
            Assert.True(SearchableBag.Tags.None(tag => stopWords.Any(stopword => stopword == tag)));
        }

        [Fact(DisplayName = "Tag does flavour words that are not stop words in lowercase")]
        public void ContainsFlavourWordsThatAreNotStopWordsInLowerCase()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == "earl"));
            Assert.True(SearchableBag.Tags.Any(tag => tag == "grey"));
            Assert.True(SearchableBag.Tags.Any(tag => tag == "the"));
        }

        [Fact(DisplayName = "Tag does words from series in lowercase that are not stop words")]
        public void ContainsSerieWordsThatAreNotStopWords()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == "some"));
            Assert.True(SearchableBag.Tags.Any(tag => tag == "series"));
        }

        [Fact(DisplayName = "Tag does words from hallmark in lowercase that are not stop words")]
        public void ContainsHallmarkWordsThatAreNotStopWords()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == "some"));
            Assert.True(SearchableBag.Tags.Any(tag => tag == "hallmark"));
        }

        [Fact(DisplayName = "Tag does words from serial number in lowercase that are not stop words")]
        public void ContainsSerialNumberWordsThatAreNotStopWords()
        {
            Assert.True(SearchableBag.Tags.Any(tag => tag == "some"));
            Assert.True(SearchableBag.Tags.Any(tag => tag == "serial"));
            Assert.True(SearchableBag.Tags.Any(tag => tag == "number"));
        }   
    }
}
