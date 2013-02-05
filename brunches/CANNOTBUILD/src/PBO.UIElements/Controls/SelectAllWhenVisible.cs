using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Controls;

namespace PokemonBattleOnline.PBO.UIElements
{
    public class SelectAllWhenVisible : Behavior<TextBox>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.IsVisibleChanged -= AssociatedObject_IsVisibleChanged;
        }

        public void AssociatedObject_IsVisibleChanged(object sender, 
            DependencyPropertyChangedEventArgs e)
        {
            if (AssociatedObject.IsVisible)
            {
                AssociatedObject.Focus();
                AssociatedObject.SelectAll();
            }
        }
    }
}
