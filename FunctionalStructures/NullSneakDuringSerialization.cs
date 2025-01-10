using System.Text.Json;
using System.Text.Json.Serialization;
using FunctionalStructures.FLib;
using static FunctionalStructures.FLib.FLibHelper;

namespace FunctionalStructures;

public static class NullSneakDuringSerialization
{
    private class OptionJsonConverter<T> : JsonConverter<Option<T>>
    {
        public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return None;
            }
            
            var value = JsonSerializer.Deserialize<T>(ref reader, options);
            return Some(value!);
        }

        public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
        {
            value.Match(
                some: wrappedValue => JsonSerializer.Serialize(writer, wrappedValue, options),
                none: writer.WriteNullValue
            );
        }
    }
    public static void Run()
    {
        string json = """{"Name":"Enrico", "Email":null}""";
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new OptionJsonConverter<string>() // Example for Option<string>
            }
        };
        
        UserRegistration? subscriber = JsonSerializer.Deserialize<UserRegistration>(json, options);
        
        if(subscriber is not null)
            Console.WriteLine(subscriber.Email.ToString().ToLower());
            
    }
}