namespace TheCollection.Web.Extensions {

    using System.Collections.Generic;
    using System.Linq;
    using TheCollection.Web.Translators;

    public static class ITranslatorExtensions {

        public static TDestination Translate<TSource, TDestination>(this ITranslator<TSource, TDestination> translator, TSource source) where TDestination : new() {
            var destination = new TDestination();
            translator.Translate(source, destination);
            return destination;
        }

        public static IEnumerable<TDestination> Translate<TSource, TDestination>(this ITranslator<TSource, TDestination> translator, IEnumerable<TSource> source) where TDestination : new() {
            return source.Select(x => translator.Translate(x));
        }
    }
}
