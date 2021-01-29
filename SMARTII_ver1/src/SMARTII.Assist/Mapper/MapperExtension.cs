using AutoMapper;

namespace SMARTII.Assist.Mapper
{
    public static class MapperExtension
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllUnmapped<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllOtherMembers(opt => opt.Ignore());

            return expression;
        }
    }
}