using MongoDB.Driver.Linq;
using MongoDB.Driver;


namespace AK7PDMAUI.Extensions
{
    //zdroj https://stackoverflow.com/questions/40915209/mongodb-net-driver-and-text-search
    public static class MongoQueryableFullTextExtensions
    {
        public static IMongoQueryable<T> WhereText<T>(this IMongoQueryable<T> query, string search)
        {
            var filter = Builders<T>.Filter.Text(search);
            return query.Where(_ => filter.Inject());
        }
    }
}
