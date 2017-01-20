﻿using Mono.Cecil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AspectInjector.BuildTask.Models.Converters
{
    internal class TypeReferenceConverter : JsonConverter
    {
        private readonly ModuleDefinition _reference;

        public TypeReferenceConverter(ModuleDefinition reference)
        {
            _reference = reference;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TypeReference).IsAssignableFrom(objectType) && objectType != typeof(TypeDefinition);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jt = JToken.Load(reader);
            var fqn = FQN.FromString(jt.ToObject<string>());

            var tr = fqn?.ToTypeReference(_reference);

            if (objectType == typeof(TypeDefinition))
                return tr.Resolve();

            return tr;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var tr = (TypeReference)value;

            var fqn = FQN.FromTypeReference(tr).ToString();

            JToken.FromObject(fqn, serializer).WriteTo(writer);
        }
    }
}