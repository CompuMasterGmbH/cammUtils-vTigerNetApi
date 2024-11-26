/*
Copyright 2011, 2017 CompuMaster GmbH
Authors: Björn Zeutzheim + Jochen Wezel

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VTigerApi
{
    public class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString().ToLower();
                return value == "1" || value == "true" || value == "t";
            }
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32() != 0;
            }
            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                return reader.GetBoolean();
            }
            throw new JsonException("Invalid JSON value for boolean conversion.");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value ? "1" : "0");
        }
    }

    public class Int32Converter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();

                if (string.IsNullOrEmpty(value))
                {
                    return 0; // Leere Strings werden als 0 interpretiert
                }

                if (int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int result))
                {
                    return result;
                }

                throw new JsonException($"Invalid string value for integer conversion: {value}");
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32(); // Direkte Zahl-Werte
            }

            throw new JsonException($"Unexpected token type {reader.TokenType} for integer conversion.");
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value); // Standardmäßig wird der Wert als Zahl geschrieben
        }
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();
                if (DateTime.TryParse(value, out DateTime result))
                {
                    return result;
                }
            }
            throw new JsonException("Invalid JSON value for DateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("s"));
        }
    }

    public class EnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();
                if (Enum.TryParse(typeToConvert, value, true, out object result))
                {
                    return (T)result;
                }
            }
            throw new JsonException($"Invalid value for enum {typeToConvert.Name}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public class EmailAdressesConverter : JsonConverter<EmailAdresses>
    {
        public override EmailAdresses Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();
                return new EmailAdresses { Adresses = value.Split(',') };
            }
            throw new JsonException("Invalid value for EmailAdresses.");
        }

        public override void Write(Utf8JsonWriter writer, EmailAdresses value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(string.Join(",", value.Adresses ?? Array.Empty<string>()));
        }
    }
}