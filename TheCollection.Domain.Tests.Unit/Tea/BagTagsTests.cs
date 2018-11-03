namespace TheCollection.Domain.Tests.Unit.Tea {
    using System;
    using System.Linq;
    using NodaTime;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;
    using Xunit;

    [Trait(nameof(Bag), "Tags are created")]
    public class BagTagsTests {
        private Bag Bag { get; }
        private RefValue BagTypeRef { get; }
        private RefValue BrandRef { get; }
        private RefValue CountryRef { get; }
        private Searchable SearchableBag { get; }

        public BagTagsTests() {
            BagTypeRef = new RefValue(System.Guid.NewGuid().ToString(), "Paper");
            BrandRef = new RefValue(System.Guid.NewGuid().ToString(), "Twinning");
            CountryRef = new RefValue(System.Guid.NewGuid().ToString(), "Norway");
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
                InsertDate = new LocalDate(1985, 08, 22),
                ImageId = "imageid",
                OwnerId = Guid.NewGuid().ToString()
            };
            SearchableBag = new Searchable(Bag);
        }

        [Fact(DisplayName = "Tags are distinct")]
        public void TagsAreDistinct() {
            Assert.Equal(SearchableBag.Tags.Count(), SearchableBag.Tags.Distinct().Count());
        }

        [Fact(DisplayName = "Tag contains main id")]
        public void ContainsMainId() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == $"{Bag.MainID}"));
        }

        [Fact(DisplayName = "Tag contains bagtype name in lowercase")]
        public void ContainsBagTypeNameLowerCase() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == BagTypeRef.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag contains brand name in lowercase")]
        public void ContainsBrandNameLowerCase() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == BrandRef.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag contains country name in lowercase")]
        public void ContainsCountryNameLowerCase() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == CountryRef.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId() {
            Assert.Null(SearchableBag.Tags.FirstOrDefault(tag => tag == Bag.Id));
            Assert.Null(SearchableBag.Tags.FirstOrDefault(tag => tag == Bag.Id.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain insert date")]
        public void NotContainsInsertDate() {
            var localDateTags = Tags.Generate(Bag.InsertDate.ToString());
            Assert.Null(SearchableBag.Tags.FirstOrDefault(tag => localDateTags.Any(localDateTag => tag == localDateTag)));
        }

        [Fact(DisplayName = "Tag does not contain image id")]
        public void NotContainsImageId() {
            Assert.Null(SearchableBag.Tags.FirstOrDefault(tag => tag == Bag.ImageId));
            Assert.Null(SearchableBag.Tags.FirstOrDefault(tag => tag == Bag.ImageId.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain user id")]
        public void NotContainsUserId() {
            Assert.Null(SearchableBag.Tags.FirstOrDefault(tag => tag == Bag.OwnerId));
            Assert.Null(SearchableBag.Tags.FirstOrDefault(tag => tag == Bag.OwnerId.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain stopwords except for words with accents parsed to stop words")]
        public void NotContainsStopWords() {
            var stopWords = new[] { "as", "at", "was" };
            Assert.True(SearchableBag.Tags.None(tag => stopWords.Any(stopword => stopword == tag)));
        }

        [Fact(DisplayName = "Tag does flavour words that are not stop words in lowercase")]
        public void ContainsFlavourWordsThatAreNotStopWordsInLowerCase() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "earl"));
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "grey"));
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "the"));
        }

        [Fact(DisplayName = "Tag does words from series in lowercase that are not stop words")]
        public void ContainsSerieWordsThatAreNotStopWords() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "some"));
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "series"));
        }

        [Fact(DisplayName = "Tag does words from hallmark in lowercase that are not stop words")]
        public void ContainsHallmarkWordsThatAreNotStopWords() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "some"));
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "hallmark"));
        }

        [Fact(DisplayName = "Tag does words from serial number in lowercase that are not stop words")]
        public void ContainsSerialNumberWordsThatAreNotStopWords() {
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "some"));
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "serial"));
            Assert.NotNull(SearchableBag.Tags.FirstOrDefault(tag => tag == "number"));
        }
    }
}
