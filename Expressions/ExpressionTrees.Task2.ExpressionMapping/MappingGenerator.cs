using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        private Dictionary<string, string> _customMapping = new Dictionary<string, string>();

        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var newDestinationExpression = Expression.New(typeof(TDestination));
            var sourceParameter = Expression.Parameter(typeof(TSource));
            var memberInitExpr = Expression.MemberInit(newDestinationExpression, CreateMemberBindings(sourceParameter, typeof(TDestination)));

            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(memberInitExpr, sourceParameter);
            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        public MappingGenerator AddCustomMapping(Dictionary<string, string> customMapping)
        {
            _customMapping = customMapping;
            return this;
        }

        private List<MemberBinding> CreateMemberBindings(ParameterExpression sourceParameter, Type destinationType)
        {
            var result = new List<MemberBinding>();

            var sourceProperties = sourceParameter.Type.GetProperties();
            var destinationProperties = destinationType.GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var propertyInfo = _customMapping.TryGetValue(sourceProperty.Name, out var destinationPropertyName) ?
                    destinationProperties.FirstOrDefault(p => p.Name == destinationPropertyName) :
                    destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);

                if (propertyInfo is null)
                    continue;

                var access = Expression.MakeMemberAccess(sourceParameter, sourceProperty);
                var assign = Expression.Bind(propertyInfo, access);
                result.Add(assign);
            }

            return result;
        }
    }
}