using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ScintillaNET;

namespace ClashOfEditors
{
    /// <summary>
    /// Interaction logic for AvalonEditor.xaml
    /// </summary>
    public partial class AvalonEditor : UserControl
    {
        public bool SuspendEditorUpdate { get; private set; }


        public AvalonEditor()
        {
            InitializeComponent();
            InitializeEditor();
        }

        private void InitializeEditor()
        {
            Editor.ShowLineNumbers = true;
            Editor.TextChanged += Editor_TextChanged;
        }

        protected void InitializeFolding(AbstractFoldingStrategy foldingStrategy)
        {
            var foldingManager = FoldingManager.Install(Editor.TextArea);
            foldingStrategy.UpdateFoldings(foldingManager, Editor.Document);

            var foldingUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            foldingUpdateTimer.Tick += (o, args) => foldingStrategy.UpdateFoldings(foldingManager, Editor.Document);

            foldingUpdateTimer.Start();
        }

        void Editor_TextChanged(object sender, EventArgs e)
        {
            Debug.Print("RazorAvalonEditor: Editor_TextChanged");
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
            Debug.Print("RazorAvalonEditor: OnTextChanged");
            var razorEditor = sender as AvalonEditor;

            if (!razorEditor.SuspendEditorUpdate)
                razorEditor.Editor.Text = (e.NewValue ?? string.Empty).ToString();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(String), typeof(AvalonEditor), new FrameworkPropertyMetadata(string.Empty, OnTextChanged)
            {
                BindsTwoWayByDefault = true,
            });
    }
}
