﻿#pragma checksum "..\..\CreateTask.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A422726BD9EDFE2EE1E951DD9EEE49714C1789009A8B32CC9EA17AA899F0E47F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ContextSwitcher;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ContextSwitcherTest {
    
    
    /// <summary>
    /// CreateTask
    /// </summary>
    public partial class CreateTask : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox MenuComboBox;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Title;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Details;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PluginType1;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Plugin1Path;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PluginType2;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Plugin2Path;
        
        #line default
        #line hidden
        
        
        #line 150 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PluginType3;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Plugin3Path;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PluginType4;
        
        #line default
        #line hidden
        
        
        #line 186 "..\..\CreateTask.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Plugin4Path;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ContextSwitcher;component/createtask.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CreateTask.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MenuComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 27 "..\..\CreateTask.xaml"
            this.MenuComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.MenuComboBox_OnSelected);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Title = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Details = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            
            #line 67 "..\..\CreateTask.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Create_OnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PluginType1 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.Plugin1Path = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 114 "..\..\CreateTask.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenFileButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.PluginType2 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.Plugin2Path = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            
            #line 139 "..\..\CreateTask.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenFileButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this.PluginType3 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 12:
            this.Plugin3Path = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            
            #line 163 "..\..\CreateTask.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenFileButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 14:
            this.PluginType4 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 15:
            this.Plugin4Path = ((System.Windows.Controls.TextBox)(target));
            return;
            case 16:
            
            #line 187 "..\..\CreateTask.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenFileButton_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
