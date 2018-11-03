using System.Threading.Tasks;

namespace TheCollection.Domain.Core.Contracts {
    public interface ITranslator<in TSource, out TDestination> where TDestination : class {
        TDestination Translate(TSource source);
    }

    public interface IAsyncTranslator<in TSource, TDestination> where TDestination : class {
        Task<TDestination> Translate(TSource source);
    }

    public interface ITranslator<in TSource, in TDestinationSource, out TDestination> where TDestination : class {
        TDestination Translate(TSource source, TDestinationSource destinationSource);
    }
}
