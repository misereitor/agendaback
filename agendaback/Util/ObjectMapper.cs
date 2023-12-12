namespace agendaback.Util
{
    public class ObjectMapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source) where TTarget : new()
        {
            var target = new TTarget();

            var sourceProperties = typeof(TSource).GetProperties();
            var targetProperties = typeof(TTarget).GetProperties()
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var sourceProperty in sourceProperties)
            {
                if (targetProperties.TryGetValue(sourceProperty.Name, out var targetProperty))
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    targetProperty.SetValue(target, sourceValue);
                }
            }

            return target;
        }

        public static List<TTarget> MapList<TSource, TTarget>(List<TSource> sourceList) where TTarget : new()
        {
            return sourceList.Select(source => Map<TSource, TTarget>(source)).ToList();
        }
    }
}
