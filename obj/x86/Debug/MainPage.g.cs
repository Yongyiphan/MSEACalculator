﻿#pragma checksum "C:\Users\edgar\Documents\Personal Progamming\MSEACalculator\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "90399C07281FE560E5DE16F88CA59F864BFB89BDF574148E502434C10ECEC8AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSEACalculator
{
    partial class MainPage : 
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
            case 2: // MainPage.xaml line 13
                {
                    this.Navigation = (global::Windows.UI.Xaml.Controls.ColumnDefinition)(target);
                }
                break;
            case 3: // MainPage.xaml line 14
                {
                    this.MainBod = (global::Windows.UI.Xaml.Controls.ColumnDefinition)(target);
                }
                break;
            case 4: // MainPage.xaml line 19
                {
                    this.toHomeBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.toHomeBtn).Click += this.toHomeBtn_Click;
                }
                break;
            case 5: // MainPage.xaml line 20
                {
                    this.toScrollBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.toScrollBtn).Click += this.toScrollBtn_Click;
                }
                break;
            case 6: // MainPage.xaml line 21
                {
                    this.toMesoBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.toMesoBtn).Click += this.toMesoBtn_Click;
                }
                break;
            case 7: // MainPage.xaml line 22
                {
                    this.toEventBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.toEventBtn).Click += this.toEventBtn_Click;
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

