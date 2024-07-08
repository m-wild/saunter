﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LEGO.AsyncAPI.Models;
using Saunter.SharedKernel.Interfaces;

namespace Saunter.SharedKernel
{
    internal class AsyncApiSchemaGenerator : IAsyncApiSchemaGenerator
    {
        public AsyncApiSchema? Generate(Type? type)
        {
            if (type is null)
            {
                return null;
            }

            var typeInfo = type.GetTypeInfo();

            var schema = new AsyncApiSchema
            {
                Title = typeInfo.Name,
                Type = MapJsonTypeToSchemaType(typeInfo),
                Properties = typeInfo.DeclaredProperties.ToDictionary(
                    prop => prop.Name,
                    prop => BuildSchema(prop.PropertyType.GetTypeInfo())
                )
            };

            return schema;
        }

        private static readonly TypeInfo s_stringTypeInfo = typeof(string).GetTypeInfo();
        private static readonly TypeInfo s_boolTypeInfo = typeof(bool).GetTypeInfo();

        private static readonly TypeInfo[] s_intergerTypeInfos = new TypeInfo[]
        {
            typeof(byte).GetTypeInfo(),
            typeof(short).GetTypeInfo(),
            typeof(int).GetTypeInfo(),
            typeof(long).GetTypeInfo(),
            typeof(uint).GetTypeInfo(),
            typeof(ushort).GetTypeInfo(),
            typeof(ulong).GetTypeInfo(),
        };

        private static readonly TypeInfo[] s_floatTypeInfos = new TypeInfo[]
        {
            typeof(float).GetTypeInfo(),
            typeof(decimal).GetTypeInfo(),
            typeof(double).GetTypeInfo(),
        };

        private static SchemaType? MapJsonTypeToSchemaType(TypeInfo typeInfo)
        {
            if (typeInfo == s_stringTypeInfo)
            {
                return SchemaType.String;
            }

            if (typeInfo == s_boolTypeInfo)
            {
                return SchemaType.Boolean;
            }

            if (s_intergerTypeInfos.Contains(typeInfo))
            {
                return SchemaType.Integer;
            }

            if (s_floatTypeInfos.Contains(typeInfo))
            {
                return SchemaType.Number;
            }

            if (typeInfo.IsArray || typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return SchemaType.Array;
            }

            return SchemaType.Object;
        }

        private AsyncApiSchema? BuildSchema(TypeInfo typeInfo)
        {
            return Generate(typeInfo);
        }
    }
}
