namespace SMARTII.Assist.Mapper
{
    public static class EntryProfile
    {
        public static void Initialize(params Domain.Mapper.IProfileExpression[] profileExpressions)
        {
            // 迭代並註冊到Automapper
            // ※該作法是靜態註冊
            AutoMapper.Mapper.Initialize(x =>
            {
                foreach (var expression in profileExpressions)
                {
                    expression.AddProfile(x);
                }
            });
        }
    }
}