using System.ComponentModel;

namespace Host.Data
{
    public class Converter
    {
        [TypeConverter(typeof(DateTimeConverter))]
        public int Time { get; set; }
    }

    public class TestConverter : TypeConverter
    {

    }
}
