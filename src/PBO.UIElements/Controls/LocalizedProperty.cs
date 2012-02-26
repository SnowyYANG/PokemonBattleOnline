using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;
using LightStudio.Tactic.Globalization;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
    public class LocalizedProperty : Behavior<DependencyObject>, IWeakEventListener
    {
        public DependencyProperty Property
        { get; set; }

        public IDomainStringService StringService
        { get; set; }

        #region Key

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(LocalizedProperty),
            new UIPropertyMetadata(null, OnKeyChanged));

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            if (StringService == null) StringService = DataService.String;
            PropertyChangedEventManager.AddListener(StringService, this, string.Empty);
            UpdateProperty();
        }

        private void UpdateProperty()
        {
          if (string.IsNullOrEmpty(Key)) AssociatedObject.SetValue(Property, null);
          else AssociatedObject.SetValue(Property, StringService[Key]);
        }

        private static void OnKeyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var localizedProperty = sender as LocalizedProperty;
            if (localizedProperty.AssociatedObject != null)
                localizedProperty.UpdateProperty();
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(PropertyChangedEventManager))
            {
                UpdateProperty();
                return true;
            }
            return false;
        }
    }
}
