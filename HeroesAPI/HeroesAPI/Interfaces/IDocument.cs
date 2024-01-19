using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HeroesAPI.Interfaces
{

    public interface IDocument
    {
       
        string Id { get; set; }

        
    }
}
