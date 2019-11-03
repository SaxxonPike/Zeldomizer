using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Zeldomizer.Randomization;
using Zeldomizer.Randomization.Interfaces;
using Zeldomizer.UI.Controllers;
using Zeldomizer.UI.Models;

namespace Zeldomizer.UI.Presenters
{
    public class MainPresenter : IDisposable
    {
        private readonly MainController _controller;
        private readonly MainModel _model;
        private readonly Font _defaultFont;
        private readonly Font _boldFont;

        public MainPresenter(MainController controller, MainModel model)
        {
            _controller = controller;
            _model = model;
            _defaultFont = new Font(SystemFonts.DefaultFont, FontStyle.Regular);
            _boldFont = new Font(_defaultFont, FontStyle.Bold);
            
            BuildForm();

            controller.ModuleSelected += (sender, module) => PopulateList(module);
            controller.ParameterValueChanged += (sender, parameter) => UpdateList();
            controller.ParameterEnableTypeChanged += (sender, parameter) => UpdateList();
        }

        protected FlowLayoutPanel List { get; private set; }
        protected Label ListLabel { get; private set; }
        protected IEnumerable<Control> ListValueControls { get; private set; } = Enumerable.Empty<Control>();
        protected ListView Tree { get; private set; }
        protected Label TreeLabel { get; private set; }

        public Form Form { get; private set; }

        private static CheckState GetNextCheckState(CheckState currentCheckState)
        {
            return (CheckState) (((int) currentCheckState + 1) % 3);
        }

        private static RandomizerParameterEnableType MapCheckStateToEnableType(CheckState checkState)
        {
            switch (checkState)
            {
                case CheckState.Checked:
                    return RandomizerParameterEnableType.Enabled;
                case CheckState.Unchecked:
                    return RandomizerParameterEnableType.Disabled;
                default:
                    return RandomizerParameterEnableType.Randomized;
            }
        }

        private static CheckState MapEnableTypeToCheckState(RandomizerParameterEnableType enableType)
        {
            switch (enableType)
            {
                case RandomizerParameterEnableType.Disabled:
                    return CheckState.Unchecked;
                case RandomizerParameterEnableType.Enabled:
                    return CheckState.Checked;
                default:
                    return CheckState.Indeterminate;
            }
        }

        private void SetParameterValueFromCheckState(IRandomizerParameter parameter, CheckState checkState)
        {
            switch (checkState)
            {
                case CheckState.Checked:
                    _controller.SetParameterValue(parameter, true);
                    break;
                case CheckState.Unchecked:
                    _controller.SetParameterValue(parameter, false);
                    break;
                default:
                    _controller.SetParameterValue(parameter, parameter.GetDefaultValue<bool>());
                    break;
            }
        }

        private void UpdateList()
        {
            _controller.Suspend();

            try
            {
                foreach (var control in ListValueControls)
                {
                    if (control.Tag is IRandomizerParameter p)
                    {
                        control.Enabled = p.EnableType != RandomizerParameterEnableType.Disabled;
                        
                        if (control is TextBox textBox)
                        {
                            textBox.Text = p.GetEffectiveValue<string>();
                        }
                        else if (control is NumericUpDown numeric)
                        {
                            numeric.Value = p.GetEffectiveValue<int>();
                        }
                    }
                }
            }
            finally
            {
                _controller.Resume();
            }
        }

        private void PopulateList(IRandomizerModule module)
        {
            if (module == null)
                return;

            ListValueControls = Enumerable.Empty<Control>();

            var valueControlList = new List<Control>();

            List.SuspendLayout();
            List.Controls.Clear();
            List.Controls.AddRange(module.Parameters
                .OrderBy(p => p.Name)
                .Select(p =>
                {
                    var panel = new TableLayoutPanel
                    {
                        AutoSize = true,
                        ColumnCount = 3,
                        ColumnStyles =
                        {
                            new ColumnStyle(SizeType.AutoSize),
                            new ColumnStyle(SizeType.AutoSize),
                            new ColumnStyle(SizeType.Percent, 100)
                        },
                        Dock = DockStyle.Top,
                        Margin = new Padding(0, 5, 0, 5),
                        Padding = new Padding(0),
                        RowCount = 2,
                        RowStyles =
                        {
                            new RowStyle(SizeType.AutoSize),
                            new RowStyle(SizeType.AutoSize)
                        }
                    };
                    
                    var nameLabel = new Label
                    {
                        AutoSize = true,
                        Dock = DockStyle.Top,
                        Margin = new Padding(0),
                        Padding = new Padding(0, 4, 0, 0),
                        Text = p.Name ?? Resources.MissingParameterNameText
                    };

                    var enableCheckBox = new CheckBox
                    {
                        AutoCheck = false,
                        AutoSize = true,
                        Padding = new Padding(0, 4, 0, 0),
                        Margin = new Padding(0),
                        ThreeState = true,
                        CheckState = MapEnableTypeToCheckState(p.EnableType),
                        Tag = p
                    };

                    enableCheckBox.Click += (sender, args) =>
                        enableCheckBox.CheckState = GetNextCheckState(enableCheckBox.CheckState);

                    enableCheckBox.CheckStateChanged += (sender, args) =>
                        _controller.SetParameterEnableType(p, MapCheckStateToEnableType(enableCheckBox.CheckState));

                    nameLabel.MouseDown += (sender, args) =>
                    {
                        if (args.Button == MouseButtons.Left)
                            enableCheckBox.CheckState = GetNextCheckState(enableCheckBox.CheckState);
                    };

                    Control nameValue;

                    if (p.Type == typeof(int))
                    {
                        var numeric = new NumericUpDown
                        {
                            Maximum = int.MaxValue,
                            Minimum = int.MinValue,
                            Margin = new Padding(0),
                            Padding = new Padding(0),
                            Tag = p
                        };
                        valueControlList.Add(numeric);
                        numeric.TextChanged += (sender, args) => _controller.SetParameterValue(p, (int)numeric.Value);

                        nameValue = numeric;
                    }
                    else if (p.Type == typeof(bool))
                    {
                        enableCheckBox.CheckStateChanged += (sender, args) =>
                            SetParameterValueFromCheckState(p, enableCheckBox.CheckState);

                        nameValue = new Panel {Visible = false};
                    }
                    else
                    {
                        var text = new TextBox
                        {
                            Padding = new Padding(0, 0, 0, 0),
                            Margin = new Padding(0),
                            Tag = p
                        };
                        valueControlList.Add(text);
                        text.TextChanged += (sender, args) => _controller.SetParameterValue(p, text.Text);
                        
                        nameValue = text;
                    }

                    var descriptionLabel = new Label
                    {
                        AutoSize = true,
                        Dock = DockStyle.Top,
                        ForeColor = SystemColors.ControlDarkDark,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        Text = p.Description ?? Resources.MissingDescriptionNameText
                    };
                    
                    panel.Controls.AddRange(new[]{ enableCheckBox, nameLabel, nameValue, descriptionLabel });
                    panel.SetColumnSpan(descriptionLabel, 3);

                    return (Control) panel;
                })
                .ToArray());

            ListLabel.Text = module.Name;
            List.ResumeLayout();

            ListValueControls = valueControlList;
            
            UpdateList();
        }

        private void BuildForm()
        {
            var listContainer = BuildList();
            var treeContainer = BuildTree();
            
            var panel = new SplitContainer
            {
                Dock = DockStyle.Fill,
                FixedPanel = FixedPanel.Panel1,
                Orientation = Orientation.Vertical
            };

            var form = new Form
            {
                Height = 480,
                Padding = new Padding(6),
                Text = Resources.MainTitle,
                Width = 640
            };

            panel.SuspendLayout();
            panel.Panel1.Controls.Add(treeContainer);
            panel.Panel2.Controls.Add(listContainer);
            panel.ResumeLayout();
            
            form.FormClosing += (sender, args) => _controller.CloseForm(args);
            form.Controls.Add(panel);

            panel.Panel1MinSize = 150;
            panel.SplitterDistance = panel.Panel1MinSize;

            Form = form;
        }

        private Control BuildList()
        {
            List = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            ListLabel = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Font = _boldFont,
                Padding = new Padding(0, 5, 0, 3),
                Text = string.Empty,
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            var listContainer = new TableLayoutPanel
            {
                ColumnCount = 1,
                ColumnStyles =
                {
                    new ColumnStyle { SizeType = SizeType.Percent, Width = 100 }
                },
                Dock = DockStyle.Fill,
                RowCount = 2,
                RowStyles =
                {
                    new RowStyle { SizeType = SizeType.AutoSize },
                    new RowStyle { SizeType = SizeType.Percent, Height = 100 }
                }
            };

            listContainer.Controls.AddRange(new Control[] {ListLabel, List});

            return listContainer;
        }

        private Control BuildTree()
        {
            Tree = new ListView
            {
                AllowColumnReorder = true,
                FullRowSelect = true,
                CheckBoxes = true,
                Dock = DockStyle.Fill,
                View = View.List
            };

            Tree.Items.AddRange(_model.Randomizer.Modules
                .OrderBy(m => m.Name)
                .Select(m =>
                {
                    var moduleNode = new ListViewItem
                    {
                        Tag = m,
                        Text = m.Name,
                        ToolTipText = m.Description
                    };

                    return moduleNode;
                })
                .ToArray());
            
            Tree.SelectedIndexChanged += (sender, args) => 
                _controller.SelectModule((IRandomizerModule) Tree.SelectedItems.OfType<ListViewItem>().FirstOrDefault()?.Tag);
            
            Tree.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            TreeLabel = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Font = _boldFont,
                Padding = new Padding(0, 5, 0, 3),
                Text = Resources.ModulesTitle,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var treeContainer = new TableLayoutPanel
            {
                ColumnCount = 1,
                ColumnStyles =
                {
                    new ColumnStyle { SizeType = SizeType.Percent, Width = 100 }
                },
                Dock = DockStyle.Fill,
                RowCount = 2,
                RowStyles =
                {
                    new RowStyle { SizeType = SizeType.AutoSize },
                    new RowStyle { SizeType = SizeType.Percent, Height = 100 }
                }
            };

            treeContainer.Controls.AddRange(new Control[] {TreeLabel, Tree});

            return treeContainer;
        }

        public void Dispose()
        {
            _defaultFont?.Dispose();
            _boldFont?.Dispose();
            List?.Dispose();
            ListLabel?.Dispose();
            Tree?.Dispose();
            TreeLabel?.Dispose();
            Form?.Dispose();
        }
    }
}
