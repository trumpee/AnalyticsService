using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AnalyticsService.Infrastructure.Persistence.Mongo.Serializers;

public class CustomDateTimeOffsetSerializer : SerializerBase<DateTimeOffset>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
    {
        context.Writer.WriteString(value.ToString("O"));
    }

    public override DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var stringValue = context.Reader.ReadString();
        return DateTimeOffset.Parse(stringValue);
    }
}
