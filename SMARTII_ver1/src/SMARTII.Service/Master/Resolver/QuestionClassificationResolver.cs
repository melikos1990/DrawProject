using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SMARTII.Domain.Master;

namespace SMARTII.Service.Master.Resolver
{
    public class QuestionClassificationResolver
    {
        private readonly IMasterAggregate _MasterAggregate;

        public QuestionClassificationResolver(IMasterAggregate MasterAggregate)
        {
            _MasterAggregate = MasterAggregate;
        }

        public IEnumerable<T> ResolveCollection<T>(IEnumerable<T> data) where T : IQuestionClassificationRelationship, new()
        {
            IDictionary<string, QuestionClassification> dist = new Dictionary<string, QuestionClassification>();

            var group = data.GroupBy(x => new
            {
                ID = x.QuestionClassificationID
            });

            group.ForEach(pair =>
            {
                QuestionClassification classification = _MasterAggregate.VWQuestionClassification_QuestionClassification_
                                                                        .Get(x => x.ID == pair.Key.ID);

                dist.Add($"{pair.Key.ID}", classification);
            });

            data.ForEach(item =>
            {
                var classification = dist[$"{item.QuestionClassificationID}"];

                item.QuestionClassificationName = classification?.Name;
                item.QuestionClassificationParentNames = classification?.ParentNamePath;
                item.QuestionClassificationParentPath = classification?.ParentPath;
                item.QuestionClassificationParentNamesByArray = classification?.ParentNamePathByArray;
                item.QuestionClassificationParentPathByArray = classification?.ParentPathByArray;
            });

            return data;
        }

        public T Resolve<T>(T data) where T : IQuestionClassificationRelationship, new()
        {
            QuestionClassification classification = _MasterAggregate.VWQuestionClassification_QuestionClassification_
                                                                    .Get(x => x.ID == data.QuestionClassificationID);

            data.QuestionClassificationName = classification?.Name;
            data.QuestionClassificationParentNames = classification?.ParentNamePath;
            data.QuestionClassificationParentPath = classification?.ParentPath;
            data.QuestionClassificationParentNamesByArray = classification?.ParentNamePathByArray;
            data.QuestionClassificationParentPathByArray = classification?.ParentPathByArray;

            return data;
        }

        public IEnumerable<T> ResolveNullableCollection<T>(IEnumerable<T> data) where T : INullableQuestionClassificationRelationship, new()
        {
            IDictionary<string, QuestionClassification> dist = new Dictionary<string, QuestionClassification>();

            var group = data.Where(x => x.QuestionClassificationID.HasValue)
                            .GroupBy(x => new
                            {
                                ID = x.QuestionClassificationID
                            });

            group.ForEach(pair =>
            {
                QuestionClassification classification = _MasterAggregate.VWQuestionClassification_QuestionClassification_
                                                                        .Get(x => x.ID == pair.Key.ID);

                dist.Add($"{pair.Key.ID}", classification);
            });

            data.ForEach(item =>
            {
                if (item.QuestionClassificationID.HasValue)
                {
                    var classification = dist[$"{item.QuestionClassificationID}"];

                    item.QuestionClassificationName = classification?.Name;
                    item.QuestionClassificationParentNames = classification?.ParentNamePath;
                    item.QuestionClassificationParentPath = classification?.ParentPath;
                    item.QuestionClassificationParentNamesByArray = classification?.ParentNamePathByArray;
                    item.QuestionClassificationParentPathByArray = classification?.ParentPathByArray;
                }
            });

            return data;
        }

        public T ResolveNullable<T>(T data) where T : INullableQuestionClassificationRelationship, new()
        {
            if (data.QuestionClassificationID.HasValue)
            {
                QuestionClassification classification = _MasterAggregate.VWQuestionClassification_QuestionClassification_
                                                             .Get(x => x.ID == data.QuestionClassificationID);

                data.QuestionClassificationName = classification?.Name;
                data.QuestionClassificationParentNames = classification?.ParentNamePath;
                data.QuestionClassificationParentPath = classification?.ParentPath;
                data.QuestionClassificationParentNamesByArray = classification?.ParentNamePathByArray;
                data.QuestionClassificationParentPathByArray = classification?.ParentPathByArray;
            }

            return data;
        }
    }
}
