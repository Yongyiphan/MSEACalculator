﻿#pragma checksum "C:\Users\edgar\Documents\Personal Progamming\MSEACalculator\EventRes\Event.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "92897344BE7984CDFDA5C485CF48ECF2DD69ADD49B055DA7D928F769AD0EC7BF"
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
            case 2: // EventRes\Event.xaml line 13
                {
                    this.Navigation = (global::Windows.UI.Xaml.Controls.ColumnDefinition)(target);
                }
                break;
            case 3: // EventRes\Event.xaml line 14
                {
                    this.MainBod = (global::Windows.UI.Xaml.Controls.ColumnDefinition)(target);
                }
                break;
            case 4: // EventRes\Event.xaml line 39
                {
                    this.EventDuration = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 5: // EventRes\Event.xaml line 69
                {
                    this.TestBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6: // EventRes\Event.xaml line 70
                {
                    this.testButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.testButton).Click += this.testButton_Click;
                }
                break;
            case 7: // EventRes\Event.xaml line 61
                {
                    this.DaysofWeeks = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 8: // EventRes\Event.xaml line 44
                {
                    this.startDatePicker = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                    ((global::Windows.UI.Xaml.Controls.CalendarDatePicker)this.startDatePicker).DateChanged += this.startDatePicker_DateChanged;
                }
                break;
            case 9: // EventRes\Event.xaml line 48
                {
                    this.endDatePicker = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                    ((global::Windows.UI.Xaml.Controls.CalendarDatePicker)this.endDatePicker).DateChanged += this.endDatePicker_DateChanged;
                }
                break;
            case 10: // EventRes\Event.xaml line 52
                {
                    this.DaysLeft = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11: // EventRes\Event.xaml line 53
                {
                    this.updateEventRecords = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.updateEventRecords).Click += this.updateEventRecords_Click;
                }
                break;
            case 12: // EventRes\Event.xaml line 33
                {
                    this.toHomeBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.toHomeBtn).Click += this.toHomeBtn_Click;
                }
                break;
            case 13: // EventRes\Event.xaml line 34
                {
                    this.toScrollBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.toScrollBtn).Click += this.toScrollBtn_Click;
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

