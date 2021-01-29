using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SMARTII.Domain.Types;

namespace SMARTII.Assist.Converter
{
    public class JsonConverter<T> : CustomCreationConverter<T>
    {
        public override T Create(Type objectType)
        {
            return (T)Activator.CreateInstance(objectType.TryGetConcreteType());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            T obj = Create(objectType);
            serializer.Populate(reader, obj);
            return obj;
        }
    }
}