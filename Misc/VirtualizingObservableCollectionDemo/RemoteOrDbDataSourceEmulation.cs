
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using VirtualizingObservableCollectionDemo.filtersort;

namespace VirtualizingObservableCollectionDemo
{
    using SortDescription = filtersort.SortDescription;

    /// <summary>
    /// Emulate a remote data repository (list of item + sort & filter values)
    /// </summary>
    public class RemoteOrDbDataSourceEmulation : IFilteredSortedSourceProviderAsync
    {
        #region statics

        //private static RemoteOrDbDataSourceEmulation _instance;
        private static readonly object _syncRoot = new Object();

        #endregion

        #region fields

        private readonly FilterDescriptionList _filterDescriptionList = new FilterDescriptionList();
        private readonly List<RemoteOrDbDataItem> _items = new List<RemoteOrDbDataItem>();
        private readonly List<RemoteOrDbDataItem> _orderedItems = new List<RemoteOrDbDataItem>();
        private readonly SortDescriptionList _sortDescriptionList = new SortDescriptionList();
        private bool _isFilteredItemsValid;
        private string _orderByLinqExpression = "";
        private string _whereLinqExpression = "";

        #endregion

        public RemoteOrDbDataSourceEmulation()
        {
            for (int i = 0; i < 100000; i++)
            {
                this._items.Add(new RemoteOrDbDataItem("Name_" + i, "Str1_" + i, "Str1_" + i, i, i));
            }

            this._sortDescriptionList.CollectionChanged += this.SortDescriptionListOnCollectionChanged;
            this._filterDescriptionList.CollectionChanged += this.FilterDescriptionListOnCollectionChanged;
        }

        public int Count => this._items.Count;

        #region properties

        public IList<RemoteOrDbDataItem> FilteredOrderedItems
        {
            get
            {
                if (!this._isFilteredItemsValid)
                {
                    lock (this)
                    {
                        this._orderedItems.Clear();

                        try
                        {
                            if (string.IsNullOrWhiteSpace(this.WhereLinqExpression) && string.IsNullOrWhiteSpace(this.OrderByLinqExpression)) this._orderedItems.AddRange(this._items);
                            else if (!string.IsNullOrWhiteSpace(this.WhereLinqExpression) && string.IsNullOrWhiteSpace(this.OrderByLinqExpression)) this._orderedItems.AddRange(this._items.Where(this.WhereLinqExpression));
                            else if (string.IsNullOrWhiteSpace(this.WhereLinqExpression) && !string.IsNullOrWhiteSpace(this.OrderByLinqExpression)) this._orderedItems.AddRange(this._items.OrderBy(this.OrderByLinqExpression));
                            else if (!string.IsNullOrWhiteSpace(this.WhereLinqExpression) && !string.IsNullOrWhiteSpace(this.OrderByLinqExpression)) this._orderedItems.AddRange(this._items.Where(this.WhereLinqExpression).OrderBy(this.OrderByLinqExpression));
                        }
                        catch (Exception)
                        {
                            Debugger.Break();
                        }
                        this._isFilteredItemsValid = true;
                    }
                }
                return this._orderedItems;
            }
        }

        public string OrderByLinqExpression
        {
            get { return this._orderByLinqExpression; }
            set
            {
                if (!string.Equals(this._orderByLinqExpression, value))
                {
                    this._orderByLinqExpression = value;
                    this._isFilteredItemsValid = false;
                }
            }
        }

        public string WhereLinqExpression
        {
            get { return this._whereLinqExpression; }
            set
            {
                if (!string.Equals(this._whereLinqExpression, value))
                {
                    this._whereLinqExpression = value;
                    this._isFilteredItemsValid = false;
                }
            }
        }

        #endregion

        #region public members

        public void OrderBy(string orderByExpression)
        {
            if (!string.Equals(orderByExpression, this.OrderByLinqExpression)) this.OrderByLinqExpression = orderByExpression;
        }

        public void Where(string whereExpression)
        {
            if (!string.Equals(whereExpression, this.WhereLinqExpression)) this.WhereLinqExpression = whereExpression;
        }

        #endregion

        #region filter & sort Descrioption list

        public SortDescriptionList SortDescriptionList => this._sortDescriptionList;

        public FilterDescriptionList FilterDescriptionList => this._filterDescriptionList;

        private void SortDescriptionListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            string sort = "";

            bool sortFound = false;
            foreach (SortDescription sortDescription in this._sortDescriptionList)
            {
                if (sortFound)
                    sort += ", ";

                sortFound = true;

                sort += sortDescription.PropertyName;
                sort += (sortDescription.Direction == ListSortDirection.Ascending) ? " ASC" : " DESC";
            }

            //if ((!sortFound) && (!string.IsNullOrWhiteSpace( primaryKey )))
            //  sort += primaryKey + " ASC";

            this.OrderByLinqExpression = sort;
        }

        private void FilterDescriptionListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                string filter = "";

                bool filterFound = false;
                foreach (FilterDescription filterDescription in this._filterDescriptionList)
                {
                    string subFilter = GetLinqQueryString(filterDescription);
                    if (!string.IsNullOrWhiteSpace(subFilter))
                    {
                        if (filterFound)
                            filter += " and ";
                        filterFound = true;
                        filter += " " + subFilter + " ";
                    }
                }

                this.WhereLinqExpression = filter;
            }
        }

        #region query builder

        private static readonly Regex _regexSplit = new Regex(@"(and)|(or)|(==)|(<>)|(!=)|(<=)|(>=)|(&&)|(\|\|)|(=)|(>)|(<)|(\*[\-_a-zA-Z0-9]+)|([\-_a-zA-Z0-9]+\*)|([\-_a-zA-Z0-9]+)",
                                                               RegexOptions.IgnoreCase);

        private static readonly Regex _regexOp = new Regex(@"(and)|(or)|(==)|(<>)|(!=)|(<=)|(>=)|(&&)|(\|\|)|(=)|(>)|(<)", RegexOptions.IgnoreCase);
        private static readonly Regex _regexComparOp = new Regex(@"(==)|(<>)|(!=)|(<=)|(>=)|(=)|(>)|(<)", RegexOptions.None);

        private static string GetLinqQueryString(FilterDescription filterDescription)
        {
            string ret = "";

            if (!string.IsNullOrWhiteSpace(filterDescription.Filter))
            {
                // using user str + linq.dynamic
                try
                {
                    // xceed syntax : empty (contains), AND (uppercase), OR (uppercase), <>, * (end with), =, >, >=, <, <=, * (start with)
                    //    see http://doc.xceedsoft.com/products/XceedWpfDataGrid/Filter_Row.html 
                    // linq.dynamic syntax : =, ==, <>, !=, <, >, <=, >=, &&, and, ||, or, x.m(�) (where x is the attrib and m the function (ex: Contains, StartsWith, EndsWith ...)
                    //    see D:\DevC#\VirtualisingCollectionTest1\DynamicQuery\Dynamic Expressions.html 
                    // ex : RemoteOrDbDataSourceEmulation.Instance.Items.Where( "Name.Contains(\"e_1\") or Name.Contains(\"e_2\")" );

                    string exp = filterDescription.Filter;

                    // arrange expression

                    bool previousTermIsOperator = false;
                    foreach (Match match in _regexSplit.Matches(exp))
                    {
                        if (match.Success)
                        {
                            //TODO processing results
                            if (_regexOp.IsMatch(match.Value))
                            {
                                if (_regexComparOp.IsMatch(match.Value))
                                {
                                    // simple operator >, <, ==, != ...
                                    ret += " " + filterDescription.PropertyName + " " + match.Value;
                                    previousTermIsOperator = true;
                                }
                                else
                                {
                                    // and, or ...
                                    ret += " " + match.Value;
                                    previousTermIsOperator = false;
                                }
                            }
                            else
                            {
                                // Value
                                if (previousTermIsOperator)
                                {
                                    ret += " " + match.Value;
                                    previousTermIsOperator = false;
                                }
                                else
                                {
                                    if (match.Value.StartsWith("*"))
                                        ret += " " + filterDescription.PropertyName + ".EndsWith( \"" + match.Value.Substring(1) + "\" )";
                                    else if (match.Value.EndsWith("*"))
                                        ret += " " + filterDescription.PropertyName + ".StartsWith( \"" + match.Value.Substring(0, match.Value.Length - 1) + "\" )";
                                    else
                                        ret += " " + filterDescription.PropertyName + ".Contains( \"" + match.Value + "\" )";
                                    previousTermIsOperator = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                { }
            }

            return ret;
        }

        #endregion query builder

        #endregion filter & sort Descrioption list
    }
}