using AutoMapper;
using Core.Common;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        public static IMappingExpression<TSource, TDest> IgnoreId<TSource, TDest>(this IMappingExpression<TSource, TDest> map)
            where TSource : class
            where TDest : EfBaseModel
        {
            return map.ForMember(m => m.Id, o => o.Ignore());
        }

        public static IMappingExpression<TSource, TDest> IgnoreResultId<TSource, TDest>(this IMappingExpression<TSource, TDest> map)
            where TSource : class
            where TDest : BaseResultModel
        {
            return map.ForMember(m => m.Id, o => o.Ignore());
        }

        public static IMappingExpression<TSource, TDest> MapDefaultValuesForInsert<TSource, TDest>(this IMappingExpression<TSource, TDest> map)
            where TSource : class
            where TDest : EfBaseModel
        {
            return map.ForMember(m => m.Id, o => o.MapFrom(f => Guid.NewGuid()))
                .ForMember(m => m.CreatedDate, o => o.MapFrom(f => DateTime.Now))
                .ForMember(m => m.ModifiedDate, o => o.MapFrom(f => DateTime.Now));
        }

        public static IMappingExpression<TSource, TDest> MapDefaultValuesForUpdate<TSource, TDest>(this IMappingExpression<TSource, TDest> map)
            where TSource : class
            where TDest : EfBaseModel
        {
            return map.ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.ModifiedDate, o => o.MapFrom(f => DateTime.Now));
        }

        public static IQueryable<T> GetActiveOnly<T>(this DbSet<T> model)
            where T : EfBaseModel
        {
            return model.Where(w => !w.IsDeleted.Value);
        }
    }
}
