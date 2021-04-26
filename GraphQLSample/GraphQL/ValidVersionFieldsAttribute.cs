using GraphQLSample.Models;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace GraphQLSample.GraphQL
{
    public sealed class ValidVersionFieldsAttribute : ObjectFieldDescriptorAttribute
    {
        private static readonly Type stringType = typeof(string);
        private static readonly JsonElement _schemaExpirarionConfig;

        static ValidVersionFieldsAttribute()
        {
            using var stream = typeof(ValidVersionFieldsAttribute).Assembly.GetManifestResourceStream("GraphQLSample.Resources.SchemaExpirationConfig.json");
            _schemaExpirarionConfig = JsonDocument.Parse(stream).RootElement;
        }

        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
            => descriptor.Use(Handle);

        private FieldDelegate Handle(FieldDelegate next)
            => async context =>
            {
                await next(context);

                if (context.Result is IEntity entity)
                {
                    var time = GetMinimumExpirationPeriod(context.Operation.SelectionSet);
                    var expiresOn = entity.Version.Add(time);
                    if (DateTimeOffset.Now <= expiresOn)
                        context.Result = entity;
                    else
                    {
                        var type = context.Field.RuntimeType;
                        var person = Activator.CreateInstance(type);
                        foreach (var item in context.Operation.SelectionSet.Selections.Cast<FieldNode>().SelectMany(x => x.SelectionSet.Selections).Cast<FieldNode>())
                        {
                            var propertyInfo = type.GetProperty(ToUpperFirst(item.Name.Value));
                            if (propertyInfo.PropertyType == stringType || (!propertyInfo.PropertyType.IsClass && !propertyInfo.PropertyType.IsGenericType))
                                propertyInfo.SetValue(person, $"{item.Name.Value} field");
                        }
                        context.Result = person;
                    }
                }
            };

        private static string ToUpperFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("There is no first letter");

            Span<char> a = stackalloc char[s.Length];
            s.AsSpan(1).CopyTo(a[1..]);
            a[0] = char.ToUpper(s[0]);
            return new string(a);
        }

        private static TimeSpan GetMinimumExpirationPeriod(SelectionSetNode node, string parentName = null)
        {
            TimeSpan? time = null;
            foreach (var field in node.Selections.OfType<FieldNode>())
            {
                var propName = ToUpperFirst(field.Name.Value);
                if (field.SelectionSet is null)
                {
                    var propExpiration = TimeSpan.FromTicks(_schemaExpirarionConfig.GetProperty($"{parentName}{propName}").GetInt64());

                    if (time is null)
                        time = propExpiration;
                    else if (propExpiration < time)
                        time = propExpiration;
                }
                else
                {
                    var newTime = GetMinimumExpirationPeriod(field.SelectionSet, $"{parentName}{propName}.");

                    if (newTime != TimeSpan.Zero && (newTime < time || time is null))
                        time = newTime;
                }
            }
            return time ?? TimeSpan.Zero;
        }
    }
}
