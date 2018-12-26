using AutoMapper;
using Core.Common;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Resources;
using System;

namespace Core.Extensions
{
    public static class CoreExtension
    {
        public static IRuleBuilderOptions<T, TProperty> WithGlobalMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, string errorMessage)
        {
            foreach (var item in (rule as RuleBuilder<T, TProperty>).Rule.Validators)
                item.Options.ErrorMessageSource = new StaticStringSource(errorMessage);

            return rule;
        }

        public static IMappingExpression<TSource, TDest> SetId<TSource, TDest>(this IMappingExpression<TSource, TDest> map)
            where TSource : class
            where TDest : EfBaseModel
        {
            return map.ForMember(m => m.Id, o => o.MapFrom(f => Guid.NewGuid()));
        }
    }
}
