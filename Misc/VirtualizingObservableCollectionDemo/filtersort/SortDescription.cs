using System.ComponentModel;

namespace VirtualizingObservableCollectionDemo.filtersort
{
    public class SortDescription : IFilterOrderDescription
  {
    public SortDescription( string propertyName, ListSortDirection? direction )
    {
        this.Direction = direction;
        this.PropertyName = propertyName;
    }

    public ListSortDirection? Direction { get; set; }
    public string PropertyName { get; set; }
  }
}