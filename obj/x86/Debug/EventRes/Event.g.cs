﻿#pragma checksum "C:\Users\edgar\Documents\Personal Programming\MSEACalculator\EventRes\Event.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E47B4428B16997971F8ECF6736E6F88994C6D74C3FB05E91AB294150614C129D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSEACalculator.EventRes
{
    partial class Event : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // EventRes\Event.xaml line 14
                {
                    this.EventDuration = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // EventRes\Event.xaml line 44
                {
                    this.TestBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4: // EventRes\Event.xaml line 45
                {
                    this.testButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.testButton).Click += this.testButton_Click;
                }
                break;
            case 5: // EventRes\Event.xaml line 36
                {
                    this.DaysofWeeks = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 6: // EventRes\Event.xaml line 19
                {
                    this.startDatePicker = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                    ((global::Windows.UI.Xaml.Controls.CalendarDatePicker)this.startDatePicker).DateChanged += this.startDatePicker_DateChanged;
                }
                break;
            case 7: // EventRes\Event.xaml line 23
                {
                    this.endDatePicker = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                    ((global::Windows.UI.Xaml.Controls.CalendarDatePicker)this.endDatePicker).DateChanged += this.endDatePicker_DateChanged;
                }
                break;
            case 8: // EventRes\Event.xaml line 27
                {
                    this.DaysLeft = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9: // EventRes\Event.xaml line 28
                {
                    this.updateEventRecords = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.updateEventRecords).Click += this.updateEventRecords_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

