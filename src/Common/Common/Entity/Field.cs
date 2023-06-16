namespace Common.Entity;

public class Field
{
    public string OldValue { get; }
    public string NewValue { get; }
    public string FieldName { get; }

    public Field(string oldValue, string newValue, string fieldName)
    {
        OldValue = oldValue;
        NewValue = newValue;
        FieldName = fieldName;
    }
}