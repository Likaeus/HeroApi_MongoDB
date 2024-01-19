using MongoDB.Bson.Serialization.Attributes;



namespace HeroesAPI.Models
{
    [BsonCollection("Heroes")]
    public class Hero : Document
    {
        [BsonElement("Name")]
        public string HeroName { get; set; }


    }
}


