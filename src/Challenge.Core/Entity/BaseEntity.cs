using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Core.Entity
{
    public abstract class BaseEntity<TKey> : IHasKey<TKey>, ITrackable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public TKey Id { get; set; }

        [JsonIgnore]
        public virtual DateTime CreatedDateTime { get; set; }

        [JsonIgnore]
        public virtual DateTime? UpdatedDateTime { get; set; }

        [JsonIgnore]
        public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;
    }
}