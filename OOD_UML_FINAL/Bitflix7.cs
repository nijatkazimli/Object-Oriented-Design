namespace OOD_UML_FINAL
{
    public class GenericLT
    {
        public List<Tuple<string, object>> Fields { get; set; }
        public GenericLT()
        {
            Fields = new List<Tuple<string, object>>();
        }

        public void AddField(string fieldName, object value)
        {
            Fields.Add(new Tuple<string, object>(fieldName, value));
        }

        public object GetFieldValue(string fieldName)
        {
            foreach (var field in Fields)
            {
                if (field.Item1 == fieldName)
                {
                    return field.Item2;
                }
            }
            return null;
        }
    }
}
