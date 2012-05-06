using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScintillaNET;
using UserControl = System.Windows.Controls.UserControl;

namespace ClashOfEditors
{
    /// <summary>
    /// Interaction logic for RazorEditor.xaml
    /// </summary>
    public partial class RazorEditor : UserControl
    {
        public Scintilla Editor { get; private set; }

        public bool SuspendEditorUpdate { get; private set; }

        public RazorEditor()
        {
            InitializeComponent();
            InitializeEditor();

        }

        private void InitializeEditor()
        {
            Editor = (Scintilla)ScintillaHost.Child;
            Editor.ConfigurationManager.Language = "cs";
            Editor.Indentation.ShowGuides = true;
            Editor.Indentation.SmartIndentType = SmartIndent.CPP2;
            Editor.Folding.IsEnabled = true;
            Editor.MatchBraces = true;
            Editor.Margins.Margin0.Width = 20;
            Editor.Margins.Margin2.Width = 16;
            Editor.Whitespace.Mode = WhitespaceMode.VisibleAfterIndent;
            Editor.TextInserted += editor_TextInserted;
            Editor.TextDeleted += editor_TextDeleted;

        }

        void editor_TextDeleted(object sender, TextModifiedEventArgs e)
        {
            Debug.Print("editor_TextDeleted");
        }

        void editor_TextInserted(object sender, TextModifiedEventArgs e)
        {
            Debug.Print("editor_TextInserted");
            SuspendEditorUpdate = true;
            Text = Editor.Text;
            SuspendEditorUpdate = false;
        }


        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        public static void OnTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.Print("OnTextChanged");
            var razorEditor = sender as RazorEditor;

            if (!razorEditor.SuspendEditorUpdate)
                razorEditor.Editor.Text = (e.NewValue ?? string.Empty).ToString();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(String), typeof(RazorEditor), new FrameworkPropertyMetadata(string.Empty, OnTextChanged)
            {
                BindsTwoWayByDefault = true,
            });

    }
}
