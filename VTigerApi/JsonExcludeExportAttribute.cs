/*
Copyright 2011 Björn Zeutzheim

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
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Jayrock.Json.Conversion
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class JsonExcludeExportAttribute : Attribute, IPropertyDescriptorCustomization, IObjectMemberExporter
    {
        public void Apply(PropertyDescriptor property)
        {
            var services = (IServiceContainer)property;
            services.AddService(typeof(IObjectMemberExporter), this);
        }

        void IObjectMemberExporter.Export(ExportContext context, JsonWriter writer, object source)
        {
            //writer.WriteMember(_property.Name);
            //context.Export(_property.GetValue(source), writer);
        }
    }
}
