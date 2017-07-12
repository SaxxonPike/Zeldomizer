using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Zeldomizer.Randomization;
using Zeldomizer.Randomization.Interfaces;
using Zeldomizer.UI.Controllers;
using Zeldomizer.UI.Models;

namespace Zeldomizer.UI.Presenters
{
    public class MainPresenter
    {
        private readonly MainController _controller;
        private readonly MainModel _model;

        public MainPresenter(MainController controller, MainModel model)
        {
            _controller = controller;
            _model = model;
            BuildForm();

            controller.ModuleSelected += (sender, module) => PopulateList(module);
        }

        protected FlowLayoutPanel List { get; private set; }
        protected Label ListLabel { get; private set; }
        protected ListView Tree { get; private set; }
        protected Label TreeLabel { get; private set; }

        public Form Form { get; private set; }

        private CheckState GetNextCheckState(CheckState currentCheckState)
        {
            return (CheckState) (((int) currentCheckState + 1) % 3);
        }

        private void PopulateList(IRandomizerModule module)
        {
            if (module == null)
                return;
            
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
                        Text = p.Name ?? "(no name)"
                    };

                    var enableCheckBox = new CheckBox
                    {
                        AutoCheck = false,
                        AutoSize = true,
                        Padding = new Padding(0, 4, 0, 0),
                        Margin = new Padding(0),
                        ThreeState = true
                    };

                    switch (p.EnableType)
                    {
                        case RandomizerParameterEnableType.Disabled:
                            enableCheckBox.CheckState = CheckState.Unchecked;
                            break;
                        case RandomizerParameterEnableType.Enabled:
                            enableCheckBox.CheckState = CheckState.Checked;
                            break;
                        default:
                            enableCheckBox.CheckState = CheckState.Indeterminate;
                            break;
                    }

                    enableCheckBox.Click += (sender, args) =>
                        enableCheckBox.CheckState = GetNextCheckState(enableCheckBox.CheckState);

                    enableCheckBox.CheckStateChanged += (sender, args) =>
                    {
                        switch (enableCheckBox.CheckState)
                        {
                            case CheckState.Checked:
                                _controller.SetParameterEnableType(p, RandomizerParameterEnableType.Enabled);
                                break;
                            case CheckState.Unchecked:
                                _controller.SetParameterEnableType(p, RandomizerParameterEnableType.Disabled);
                                break;
                            default:
                                _controller.SetParameterEnableType(p, RandomizerParameterEnableType.Randomized);
                                break;
                        }
                    };

                    nameLabel.MouseDown += (sender, args) =>
                    {
                        if (args.Button == MouseButtons.Left)
                            enableCheckBox.CheckState = GetNextCheckState(enableCheckBox.CheckState);
                    };

                    Control nameValue;

                    if (p.Type == typeof(int))
                    {
                        nameValue = new NumericUpDown
                        {
                            Maximum = int.MaxValue,
                            Minimum = int.MinValue,
                            Margin = new Padding(0),
                            Padding = new Padding(0),
                            Value = p.GetValue<int>()
                        };
                    }
                    else if (p.Type == typeof(bool))
                    {
                        enableCheckBox.CheckStateChanged += (sender, args) =>
                        {
                            switch (enableCheckBox.CheckState)
                            {
                                case CheckState.Checked:
                                    _controller.SetParameterValue(p, true);
                                    break;
                                case CheckState.Unchecked:
                                    _controller.SetParameterValue(p, false);
                                    break;
                                default:
                                    _controller.SetParameterValue(p, p.GetDefaultValue<bool>());
                                    break;
                            }
                        };

                        nameValue = new Panel {Visible = false};
                    }
                    else
                    {
                        var c = new TextBox
                        {
                            Padding = new Padding(0, 0, 0, 0),
                            Margin = new Padding(0),
                            Text = p.GetValue<string>()
                        };
                        nameValue = c;
                    }

                    var descriptionLabel = new Label
                    {
                        AutoSize = true,
                        Dock = DockStyle.Top,
                        ForeColor = SystemColors.ControlDarkDark,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        Text = p.Description ?? "(no description)"
                    };
                    
                    panel.Controls.AddRange(new[]{ enableCheckBox, nameLabel, nameValue, descriptionLabel });
                    panel.SetColumnSpan(descriptionLabel, 3);

                    return (Control) panel;
                })
                .ToArray());

            ListLabel.Text = module.Name;
            List.ResumeLayout();
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
                Text = "Zeldomizer",
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
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold),
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
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold),
                Padding = new Padding(0, 5, 0, 3),
                Text = "Modules",
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
    }
}
