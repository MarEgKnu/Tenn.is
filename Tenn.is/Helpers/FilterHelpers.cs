using Tennis.Models;

namespace Tennis.Helpers
{
    public static class FilterHelpers
    {
        public static List<T> GetItemsOnConditions<T>(List<Predicate<T>> conditions, List<T> items) where T: class
        {
            if (items == null)
            {
                return new List<T>();
            }
            else if (conditions == null || conditions.Count == 0)
            {
                return items;
            }
            foreach (Predicate<T> condition in conditions)
            {
                if (condition == null)
                {
                    continue;
                }
                items = items.FindAll(condition);
                if (items.Count == 0)
                {
                    return items;
                }
            }
            return items;
        }
    }
}
