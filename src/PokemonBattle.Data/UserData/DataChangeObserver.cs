using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows;
using System.ComponentModel;

namespace LightStudio.PokemonBattle.Data
{
    internal class DataChangeObserver : IWeakEventListener
    {
        private object observedObj;

        private bool _changed;
        public bool Changed
        {
            get
            {
                return _changed;
            }
            private set
            {
                if (_changed != value)
                {
                    _changed = value;
                    if (_changed)
                        OnDataChanged();
                }
            }
        }

        public DataChangeObserver(object data)
        {
            this.observedObj = data;
            AddObserveHandler(data);
        }

        public void SetUnchanged()
        {
            Changed = false;
        }

        private void AddObserveHandler(object obj)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();
            if (obj is IList)
            {
                if (obj is INotifyCollectionChanged)
                    CollectionChangedEventManager.AddListener(obj as INotifyCollectionChanged, this);
                foreach (object item in (obj as IList))
                    AddObserveHandler(item);
            }
            else
            {
                if (obj is INotifyPropertyChanged)
                {
                    PropertyChangedEventManager.AddListener(obj as INotifyPropertyChanged, this,
                        string.Empty);
                }
                foreach (PropertyInfo propInfo in type.GetProperties())
                    AddPropertyObserveHandler(obj, propInfo);
            }
        }

        private void RemoveObserveHandler(object obj)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();
            if (obj is INotifyPropertyChanged)
            {
                PropertyChangedEventManager.RemoveListener(obj as INotifyPropertyChanged, this,
                    string.Empty);
                foreach (PropertyInfo propInfo in type.GetProperties())
                    RemovePropertyObserveHandler(obj, propInfo);
            }
            if (obj is INotifyCollectionChanged)
            {
                CollectionChangedEventManager.RemoveListener(obj as INotifyCollectionChanged, this);
            }
            if (obj is IList)
            {
                foreach (object item in (obj as IList))
                    RemoveObserveHandler(item);
            }
        }

        private void AddPropertyObserveHandler(object obj, PropertyInfo propInfo)
        {
            if (!propInfo.CanRead || propInfo.GetIndexParameters().Length > 0 ||
                propInfo.PropertyType.IsValueType)
                return;
            object propValue = propInfo.GetValue(obj, null);
            AddObserveHandler(propValue);
        }

        private void RemovePropertyObserveHandler(object obj, PropertyInfo propInfo)
        {
            if (!propInfo.CanRead || propInfo.GetIndexParameters().Length > 0 ||
                !propInfo.PropertyType.IsClass)
                return;
            object propValue = propInfo.GetValue(obj, null);
            RemoveObserveHandler(propValue);
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Changed = true;
            PropertyInfo propInfo = sender.GetType().GetProperty(e.PropertyName);
            AddPropertyObserveHandler(sender, propInfo);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Changed = true;
            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                    RemoveObserveHandler(item);
            }
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                    AddObserveHandler(item);
            }
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(PropertyChangedEventManager))
            {
                PropertyChanged(sender, e as PropertyChangedEventArgs);
                return true;
            }
            else if (managerType == typeof(CollectionChangedEventManager))
            {
                CollectionChanged(sender, e as NotifyCollectionChangedEventArgs);
                return true;
            }
            return false;
        }

        public event EventHandler DataChanged = delegate { };
        private void OnDataChanged()
        {
            DataChanged(this, EventArgs.Empty);
        }
    }
}
