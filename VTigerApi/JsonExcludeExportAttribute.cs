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
using System.Text.Json.Serialization;
using System.Text.Json;

namespace VTigerApi.Json.Conversion
{
    /// <summary>
    /// Custom attribute to exclude a property or field from JSON serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class JsonExcludeExportAttribute : JsonConverterAttribute
    {
        /// <inheritdoc/>
        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            // Use the custom ExcludeConverter
            return new ExcludeConverter();
        }

        /// <summary>
        /// The actual converter that prevents serialization of the property or field.
        /// </summary>
        private class ExcludeConverter : JsonConverter<object>
        {
            public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException("This field is excluded from serialization.");
            }

            public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            {
                // Do nothing, exclude the field from serialization
            }
        }
    }
}
