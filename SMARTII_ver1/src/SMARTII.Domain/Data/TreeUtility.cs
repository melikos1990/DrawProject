using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace SMARTII.Domain.Data
{
    public class TreeStorage<T>
    {
        public List<T> FlattenList { get; set; } = new List<T>();

        public Stack<T> TemporaryStack { get; set; } = new Stack<T>();

        public int Right { get; set; }

        public int Left { get; set; }

        public string ParentPath { get; set; }

        public int ParentLocator { get; set; }

        public int Level { get; set; } = 1;
    }

    public static class TreeUtility
    {
        public static IEnumerable<T> FlattenNSM<T>(this T tree) where T : INSMNestedModel
        {
            try
            {
                var temporary = new TreeStorage<T>();

                FlattenNSM(tree, temporary);

                temporary.TemporaryStack.Clear();

                return temporary.FlattenList;
            }
            catch (Exception ex)
            {
                throw new IndexOutOfRangeException($"結點計算異常,請洽系統管理員 , 原因:({ex.Message})");
            }
        }

        private static void FlattenNSM<T>(T tree, TreeStorage<T> temporary) where T : INSMNestedModel
        {
            temporary.Left += 1;

            tree.ParentLocator = temporary.ParentLocator;
            tree.ParentPath = temporary.ParentPath;
            tree.Level = temporary.Level;

            //如果有子成員
            if (tree.Children != null && tree.Children.Any())
            {
                tree.LeftBoundary = temporary.Left;
                tree.RightBoundary = 0;

                //放入堆疊
                temporary.TemporaryStack.Push(tree);

                //尋覽底下的
                foreach (T child in tree.Children)
                {
                    temporary.Level = tree.Level + 1;
                    temporary.ParentPath = $"{tree.ParentPath}/{tree.ID.ToString()}";
                    temporary.ParentLocator = tree.ID;
                    FlattenNSM(child, temporary);
                }

                //尋覽完畢後,要回根節點
                var temp = temporary.TemporaryStack.Pop();

                //右邊節點
                temporary.Right += 1;

                //更新right
                temp.RightBoundary = temporary.Right;

                temporary.FlattenList.Add(temp);
            }
            else  //如果沒有子成員
            {
                //右邊節點
                temporary.Right = temporary.Left + 1;

                tree.LeftBoundary = temporary.Left;
                tree.RightBoundary = temporary.Right;

                temporary.FlattenList.Add(tree);
            }

            //左邊節點遞加
            temporary.Left = temporary.Right;
        }

        public static T AsNestedNSM<T>(this IEnumerable<T> flattenList) where T : INSMNestedModel
        {
            try
            {
                if (!flattenList.Any(x => x.LeftBoundary == 1))
                    throw new NullReferenceException("根結點尚未被建立");

                Stack<T> stack = new Stack<T>();

                T node = default(T);

                T root = default(T);

                flattenList.OrderBy(x => x.LeftBoundary)
                           .ToList()
                           .ForEach(x =>
                {
                    if (node != null)
                    {
                        while (x.RightBoundary.CompareTo(node.RightBoundary) == 1)
                        {
                            node = stack.Pop();
                        }

                        x.ParentPath = $"{node.ParentPath.ToString()}/{node.ID.ToString()}";
                        var parents = x.ParentPath.Split('/');
                        x.Level = parents.Length + 1;
                        x.ParentLocator = string.IsNullOrEmpty(parents?.Last()) ? default(int?) : int.Parse(parents.Last());
                        node.Children.Add(x);
                        stack.Push(node);

                        node = x;
                    }
                    else
                    {
                        root = x;
                        node = x;
                        node.Level = 1;
                        node.ParentPath = string.Empty;
                    }
                    stack.Push(node);
                });

                stack.Clear();

                return root;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<T> AsNested<T>(this IEnumerable<T> flattenList, T parent = default(T)) where T : IRecursivelyModel
        {
            var roots = flattenList.Where(x => x.ParentLocator == null);

            roots.ForEach(root =>
            {
                var subChildren = flattenList.Where(x => x.ParentLocator == root.ID);

                subChildren?.ForEach(x =>
                {
                    var next = _AsNested(flattenList, x) as IRecursivelyModel;

                    root.Children.Add(next);
                });
            });

            return roots;
        }

        public static T _AsNested<T>(this IEnumerable<T> flattenList, T parent = default(T)) where T : IRecursivelyModel
        {
            var children = flattenList.Where(x => x.ParentLocator == parent.ID);

            children?.ForEach(x =>
            {
                parent.Children.Add(_AsNested(flattenList, x) as IRecursivelyModel);
            });

            return parent;
        }
    }
}
