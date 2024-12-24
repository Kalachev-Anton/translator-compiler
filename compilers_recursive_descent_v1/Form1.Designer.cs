namespace recursive_descent_translator_app
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCsharpMain = new System.Windows.Forms.TextBox();
            this.txtRPNMain = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstResultsMain = new System.Windows.Forms.ListBox();
            this.labelK4 = new System.Windows.Forms.Label();
            this.labelK2 = new System.Windows.Forms.Label();
            this.labelK3 = new System.Windows.Forms.Label();
            this.labelK1 = new System.Windows.Forms.Label();
            this.labelInputText = new System.Windows.Forms.Label();
            this.txtInputMain = new System.Windows.Forms.TextBox();
            this.lstKeywordsMain = new System.Windows.Forms.ListBox();
            this.lstSeparatorsMain = new System.Windows.Forms.ListBox();
            this.lstVariablesMain = new System.Windows.Forms.ListBox();
            this.lstConstantsMain = new System.Windows.Forms.ListBox();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.tabLexicalAnalysis = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNoComments = new System.Windows.Forms.TextBox();
            this.txtDescriptor = new System.Windows.Forms.TextBox();
            this.txtRecovered = new System.Windows.Forms.TextBox();
            this.lstKeywordsLex = new System.Windows.Forms.ListBox();
            this.lstSeparatorsLex = new System.Windows.Forms.ListBox();
            this.lstVariablesLex = new System.Windows.Forms.ListBox();
            this.lstConstantsLex = new System.Windows.Forms.ListBox();
            this.tabSyntaxAnalysis = new System.Windows.Forms.TabPage();
            this.buttonCopyListToBox = new System.Windows.Forms.Button();
            this.richTextSyntax = new System.Windows.Forms.RichTextBox();
            this.lstSyntax = new System.Windows.Forms.ListBox();
            this.tabSemanticAnalysis = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.lstSemantics = new System.Windows.Forms.ListBox();
            this.tabCodeGeneration = new System.Windows.Forms.TabPage();
            this.txtCsharpGen = new System.Windows.Forms.TextBox();
            this.txtRPNgen = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabLexicalAnalysis.SuspendLayout();
            this.tabSyntaxAnalysis.SuspendLayout();
            this.tabSemanticAnalysis.SuspendLayout();
            this.tabCodeGeneration.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabMain);
            this.tabControl.Controls.Add(this.tabLexicalAnalysis);
            this.tabControl.Controls.Add(this.tabSyntaxAnalysis);
            this.tabControl.Controls.Add(this.tabSemanticAnalysis);
            this.tabControl.Controls.Add(this.tabCodeGeneration);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1150, 625);
            this.tabControl.TabIndex = 0;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.label11);
            this.tabMain.Controls.Add(this.label10);
            this.tabMain.Controls.Add(this.txtCsharpMain);
            this.tabMain.Controls.Add(this.txtRPNMain);
            this.tabMain.Controls.Add(this.label2);
            this.tabMain.Controls.Add(this.lstResultsMain);
            this.tabMain.Controls.Add(this.labelK4);
            this.tabMain.Controls.Add(this.labelK2);
            this.tabMain.Controls.Add(this.labelK3);
            this.tabMain.Controls.Add(this.labelK1);
            this.tabMain.Controls.Add(this.labelInputText);
            this.tabMain.Controls.Add(this.txtInputMain);
            this.tabMain.Controls.Add(this.lstKeywordsMain);
            this.tabMain.Controls.Add(this.lstSeparatorsMain);
            this.tabMain.Controls.Add(this.lstVariablesMain);
            this.tabMain.Controls.Add(this.lstConstantsMain);
            this.tabMain.Controls.Add(this.btnAnalyze);
            this.tabMain.Controls.Add(this.btnLoadFile);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Size = new System.Drawing.Size(1142, 599);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Главная";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(636, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(137, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Сгенерированный код C#";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(306, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "ПолИЗ / ОПЗ";
            // 
            // txtCsharpMain
            // 
            this.txtCsharpMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCsharpMain.Location = new System.Drawing.Point(639, 59);
            this.txtCsharpMain.Multiline = true;
            this.txtCsharpMain.Name = "txtCsharpMain";
            this.txtCsharpMain.ReadOnly = true;
            this.txtCsharpMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCsharpMain.Size = new System.Drawing.Size(500, 309);
            this.txtCsharpMain.TabIndex = 24;
            // 
            // txtRPNMain
            // 
            this.txtRPNMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtRPNMain.Location = new System.Drawing.Point(309, 59);
            this.txtRPNMain.Multiline = true;
            this.txtRPNMain.Name = "txtRPNMain";
            this.txtRPNMain.ReadOnly = true;
            this.txtRPNMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRPNMain.Size = new System.Drawing.Size(324, 309);
            this.txtRPNMain.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(412, 371);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Результаты работы разных блоков анализа";
            // 
            // lstResultsMain
            // 
            this.lstResultsMain.FormattingEnabled = true;
            this.lstResultsMain.Location = new System.Drawing.Point(415, 387);
            this.lstResultsMain.Name = "lstResultsMain";
            this.lstResultsMain.Size = new System.Drawing.Size(724, 212);
            this.lstResultsMain.TabIndex = 21;
            // 
            // labelK4
            // 
            this.labelK4.AutoSize = true;
            this.labelK4.Location = new System.Drawing.Point(206, 485);
            this.labelK4.Name = "labelK4";
            this.labelK4.Size = new System.Drawing.Size(20, 13);
            this.labelK4.TabIndex = 20;
            this.labelK4.Text = "K4";
            // 
            // labelK2
            // 
            this.labelK2.AutoSize = true;
            this.labelK2.Location = new System.Drawing.Point(206, 371);
            this.labelK2.Name = "labelK2";
            this.labelK2.Size = new System.Drawing.Size(20, 13);
            this.labelK2.TabIndex = 19;
            this.labelK2.Text = "K2";
            // 
            // labelK3
            // 
            this.labelK3.AutoSize = true;
            this.labelK3.Location = new System.Drawing.Point(3, 485);
            this.labelK3.Name = "labelK3";
            this.labelK3.Size = new System.Drawing.Size(20, 13);
            this.labelK3.TabIndex = 18;
            this.labelK3.Text = "K3";
            // 
            // labelK1
            // 
            this.labelK1.AutoSize = true;
            this.labelK1.Location = new System.Drawing.Point(3, 371);
            this.labelK1.Name = "labelK1";
            this.labelK1.Size = new System.Drawing.Size(20, 13);
            this.labelK1.TabIndex = 17;
            this.labelK1.Text = "K1";
            // 
            // labelInputText
            // 
            this.labelInputText.AutoSize = true;
            this.labelInputText.Location = new System.Drawing.Point(3, 43);
            this.labelInputText.Name = "labelInputText";
            this.labelInputText.Size = new System.Drawing.Size(146, 13);
            this.labelInputText.TabIndex = 16;
            this.labelInputText.Text = "Исодный текст программы";
            // 
            // txtInputMain
            // 
            this.txtInputMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtInputMain.Location = new System.Drawing.Point(3, 59);
            this.txtInputMain.Multiline = true;
            this.txtInputMain.Name = "txtInputMain";
            this.txtInputMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInputMain.Size = new System.Drawing.Size(300, 309);
            this.txtInputMain.TabIndex = 10;
            this.txtInputMain.Text = "Введите или загрузите код для анализа.";
            // 
            // lstKeywordsMain
            // 
            this.lstKeywordsMain.FormattingEnabled = true;
            this.lstKeywordsMain.HorizontalScrollbar = true;
            this.lstKeywordsMain.Location = new System.Drawing.Point(3, 387);
            this.lstKeywordsMain.Name = "lstKeywordsMain";
            this.lstKeywordsMain.Size = new System.Drawing.Size(200, 95);
            this.lstKeywordsMain.TabIndex = 12;
            // 
            // lstSeparatorsMain
            // 
            this.lstSeparatorsMain.FormattingEnabled = true;
            this.lstSeparatorsMain.HorizontalScrollbar = true;
            this.lstSeparatorsMain.Location = new System.Drawing.Point(209, 387);
            this.lstSeparatorsMain.Name = "lstSeparatorsMain";
            this.lstSeparatorsMain.Size = new System.Drawing.Size(200, 95);
            this.lstSeparatorsMain.TabIndex = 13;
            // 
            // lstVariablesMain
            // 
            this.lstVariablesMain.FormattingEnabled = true;
            this.lstVariablesMain.HorizontalScrollbar = true;
            this.lstVariablesMain.Location = new System.Drawing.Point(3, 501);
            this.lstVariablesMain.Name = "lstVariablesMain";
            this.lstVariablesMain.Size = new System.Drawing.Size(200, 95);
            this.lstVariablesMain.TabIndex = 14;
            // 
            // lstConstantsMain
            // 
            this.lstConstantsMain.FormattingEnabled = true;
            this.lstConstantsMain.HorizontalScrollbar = true;
            this.lstConstantsMain.Location = new System.Drawing.Point(209, 501);
            this.lstConstantsMain.Name = "lstConstantsMain";
            this.lstConstantsMain.Size = new System.Drawing.Size(200, 95);
            this.lstConstantsMain.TabIndex = 15;
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(109, 10);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(100, 30);
            this.btnAnalyze.TabIndex = 8;
            this.btnAnalyze.Text = "Анализ";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(3, 10);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(100, 30);
            this.btnLoadFile.TabIndex = 9;
            this.btnLoadFile.Text = "Загрузить файл";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // tabLexicalAnalysis
            // 
            this.tabLexicalAnalysis.Controls.Add(this.label9);
            this.tabLexicalAnalysis.Controls.Add(this.label8);
            this.tabLexicalAnalysis.Controls.Add(this.label7);
            this.tabLexicalAnalysis.Controls.Add(this.label6);
            this.tabLexicalAnalysis.Controls.Add(this.label5);
            this.tabLexicalAnalysis.Controls.Add(this.label4);
            this.tabLexicalAnalysis.Controls.Add(this.label3);
            this.tabLexicalAnalysis.Controls.Add(this.txtNoComments);
            this.tabLexicalAnalysis.Controls.Add(this.txtDescriptor);
            this.tabLexicalAnalysis.Controls.Add(this.txtRecovered);
            this.tabLexicalAnalysis.Controls.Add(this.lstKeywordsLex);
            this.tabLexicalAnalysis.Controls.Add(this.lstSeparatorsLex);
            this.tabLexicalAnalysis.Controls.Add(this.lstVariablesLex);
            this.tabLexicalAnalysis.Controls.Add(this.lstConstantsLex);
            this.tabLexicalAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabLexicalAnalysis.Name = "tabLexicalAnalysis";
            this.tabLexicalAnalysis.Size = new System.Drawing.Size(1142, 599);
            this.tabLexicalAnalysis.TabIndex = 1;
            this.tabLexicalAnalysis.Text = "Лексический анализ";
            this.tabLexicalAnalysis.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(864, 367);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "K4";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(580, 367);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "K3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(284, 367);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "K2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 367);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "K1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 268);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(245, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Востановленный код из дескрипторого текста";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Дескрипторный текст";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Исходный текст без комментариев";
            // 
            // txtNoComments
            // 
            this.txtNoComments.Location = new System.Drawing.Point(3, 16);
            this.txtNoComments.Multiline = true;
            this.txtNoComments.Name = "txtNoComments";
            this.txtNoComments.ReadOnly = true;
            this.txtNoComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNoComments.Size = new System.Drawing.Size(1136, 150);
            this.txtNoComments.TabIndex = 1;
            // 
            // txtDescriptor
            // 
            this.txtDescriptor.Location = new System.Drawing.Point(3, 185);
            this.txtDescriptor.Multiline = true;
            this.txtDescriptor.Name = "txtDescriptor";
            this.txtDescriptor.ReadOnly = true;
            this.txtDescriptor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescriptor.Size = new System.Drawing.Size(1136, 80);
            this.txtDescriptor.TabIndex = 2;
            // 
            // txtRecovered
            // 
            this.txtRecovered.Location = new System.Drawing.Point(3, 284);
            this.txtRecovered.Multiline = true;
            this.txtRecovered.Name = "txtRecovered";
            this.txtRecovered.ReadOnly = true;
            this.txtRecovered.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecovered.Size = new System.Drawing.Size(1136, 80);
            this.txtRecovered.TabIndex = 3;
            // 
            // lstKeywordsLex
            // 
            this.lstKeywordsLex.FormattingEnabled = true;
            this.lstKeywordsLex.Location = new System.Drawing.Point(6, 383);
            this.lstKeywordsLex.Name = "lstKeywordsLex";
            this.lstKeywordsLex.Size = new System.Drawing.Size(275, 212);
            this.lstKeywordsLex.TabIndex = 4;
            // 
            // lstSeparatorsLex
            // 
            this.lstSeparatorsLex.FormattingEnabled = true;
            this.lstSeparatorsLex.Location = new System.Drawing.Point(287, 383);
            this.lstSeparatorsLex.Name = "lstSeparatorsLex";
            this.lstSeparatorsLex.Size = new System.Drawing.Size(275, 212);
            this.lstSeparatorsLex.TabIndex = 5;
            // 
            // lstVariablesLex
            // 
            this.lstVariablesLex.FormattingEnabled = true;
            this.lstVariablesLex.Location = new System.Drawing.Point(583, 383);
            this.lstVariablesLex.Name = "lstVariablesLex";
            this.lstVariablesLex.Size = new System.Drawing.Size(275, 212);
            this.lstVariablesLex.TabIndex = 6;
            // 
            // lstConstantsLex
            // 
            this.lstConstantsLex.FormattingEnabled = true;
            this.lstConstantsLex.Location = new System.Drawing.Point(864, 383);
            this.lstConstantsLex.Name = "lstConstantsLex";
            this.lstConstantsLex.Size = new System.Drawing.Size(275, 212);
            this.lstConstantsLex.TabIndex = 7;
            // 
            // tabSyntaxAnalysis
            // 
            this.tabSyntaxAnalysis.Controls.Add(this.label14);
            this.tabSyntaxAnalysis.Controls.Add(this.label13);
            this.tabSyntaxAnalysis.Controls.Add(this.label12);
            this.tabSyntaxAnalysis.Controls.Add(this.buttonCopyListToBox);
            this.tabSyntaxAnalysis.Controls.Add(this.richTextSyntax);
            this.tabSyntaxAnalysis.Controls.Add(this.lstSyntax);
            this.tabSyntaxAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabSyntaxAnalysis.Name = "tabSyntaxAnalysis";
            this.tabSyntaxAnalysis.Size = new System.Drawing.Size(1142, 599);
            this.tabSyntaxAnalysis.TabIndex = 2;
            this.tabSyntaxAnalysis.Text = "Синтаксический анализ";
            this.tabSyntaxAnalysis.UseVisualStyleBackColor = true;
            // 
            // buttonCopyListToBox
            // 
            this.buttonCopyListToBox.Location = new System.Drawing.Point(913, 17);
            this.buttonCopyListToBox.Name = "buttonCopyListToBox";
            this.buttonCopyListToBox.Size = new System.Drawing.Size(226, 56);
            this.buttonCopyListToBox.TabIndex = 3;
            this.buttonCopyListToBox.Text = "Копировать содержимое\r\nиз listBox\r\nв textBox";
            this.buttonCopyListToBox.UseVisualStyleBackColor = true;
            this.buttonCopyListToBox.Click += new System.EventHandler(this.buttonCopyListToBox_Click);
            // 
            // richTextSyntax
            // 
            this.richTextSyntax.Location = new System.Drawing.Point(3, 336);
            this.richTextSyntax.Name = "richTextSyntax";
            this.richTextSyntax.Size = new System.Drawing.Size(901, 260);
            this.richTextSyntax.TabIndex = 2;
            this.richTextSyntax.Text = "";
            // 
            // lstSyntax
            // 
            this.lstSyntax.FormattingEnabled = true;
            this.lstSyntax.HorizontalScrollbar = true;
            this.lstSyntax.Location = new System.Drawing.Point(3, 19);
            this.lstSyntax.Name = "lstSyntax";
            this.lstSyntax.Size = new System.Drawing.Size(904, 251);
            this.lstSyntax.TabIndex = 0;
            // 
            // tabSemanticAnalysis
            // 
            this.tabSemanticAnalysis.Controls.Add(this.label1);
            this.tabSemanticAnalysis.Controls.Add(this.lstSemantics);
            this.tabSemanticAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabSemanticAnalysis.Name = "tabSemanticAnalysis";
            this.tabSemanticAnalysis.Size = new System.Drawing.Size(1142, 599);
            this.tabSemanticAnalysis.TabIndex = 3;
            this.tabSemanticAnalysis.Text = "Семантический анализ";
            this.tabSemanticAnalysis.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(621, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(438, 91);
            this.label1.TabIndex = 2;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // lstSemantics
            // 
            this.lstSemantics.FormattingEnabled = true;
            this.lstSemantics.HorizontalScrollbar = true;
            this.lstSemantics.Location = new System.Drawing.Point(3, 3);
            this.lstSemantics.Name = "lstSemantics";
            this.lstSemantics.Size = new System.Drawing.Size(612, 589);
            this.lstSemantics.TabIndex = 1;
            // 
            // tabCodeGeneration
            // 
            this.tabCodeGeneration.Controls.Add(this.label16);
            this.tabCodeGeneration.Controls.Add(this.label15);
            this.tabCodeGeneration.Controls.Add(this.txtCsharpGen);
            this.tabCodeGeneration.Controls.Add(this.txtRPNgen);
            this.tabCodeGeneration.Location = new System.Drawing.Point(4, 22);
            this.tabCodeGeneration.Name = "tabCodeGeneration";
            this.tabCodeGeneration.Size = new System.Drawing.Size(1142, 599);
            this.tabCodeGeneration.TabIndex = 4;
            this.tabCodeGeneration.Text = "Генерация кода";
            this.tabCodeGeneration.UseVisualStyleBackColor = true;
            // 
            // txtCsharpGen
            // 
            this.txtCsharpGen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCsharpGen.Location = new System.Drawing.Point(589, 16);
            this.txtCsharpGen.Multiline = true;
            this.txtCsharpGen.Name = "txtCsharpGen";
            this.txtCsharpGen.ReadOnly = true;
            this.txtCsharpGen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCsharpGen.Size = new System.Drawing.Size(550, 580);
            this.txtCsharpGen.TabIndex = 25;
            // 
            // txtRPNgen
            // 
            this.txtRPNgen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtRPNgen.Location = new System.Drawing.Point(3, 16);
            this.txtRPNgen.Multiline = true;
            this.txtRPNgen.Name = "txtRPNgen";
            this.txtRPNgen.ReadOnly = true;
            this.txtRPNgen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRPNgen.Size = new System.Drawing.Size(550, 580);
            this.txtRPNgen.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(510, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Промежуточные вычисления синтаксического анализатора методом рекурсивного спуска " +
    "(listBox)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(910, 76);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(229, 403);
            this.label13.TabIndex = 10;
            this.label13.Text = resources.GetString("label13.Text");
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 320);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(515, 13);
            this.label14.TabIndex = 11;
            this.label14.Text = "Промежуточные вычисления синтаксического анализатора методом рекурсивного спуска " +
    "(textBox)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(279, 13);
            this.label15.TabIndex = 26;
            this.label15.Text = "Промежуточные вычисления ОПЗ (ПолИЗ) и его стэк";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(586, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(233, 13);
            this.label16.TabIndex = 27;
            this.label16.Text = "Промежуточные этапы генерации кода в C#";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.Text = "Транслятор с рекурсивным спуском";
            this.tabControl.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            this.tabLexicalAnalysis.ResumeLayout(false);
            this.tabLexicalAnalysis.PerformLayout();
            this.tabSyntaxAnalysis.ResumeLayout(false);
            this.tabSyntaxAnalysis.PerformLayout();
            this.tabSemanticAnalysis.ResumeLayout(false);
            this.tabSemanticAnalysis.PerformLayout();
            this.tabCodeGeneration.ResumeLayout(false);
            this.tabCodeGeneration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.TabPage tabLexicalAnalysis;
        private System.Windows.Forms.TabPage tabSyntaxAnalysis;
        private System.Windows.Forms.TabPage tabSemanticAnalysis;
        private System.Windows.Forms.TabPage tabCodeGeneration;
        private System.Windows.Forms.TextBox txtNoComments;
        private System.Windows.Forms.TextBox txtDescriptor;
        private System.Windows.Forms.TextBox txtRecovered;
        private System.Windows.Forms.ListBox lstKeywordsLex;
        private System.Windows.Forms.ListBox lstSeparatorsLex;
        private System.Windows.Forms.ListBox lstVariablesLex;
        private System.Windows.Forms.ListBox lstConstantsLex;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.TextBox txtInputMain;
        private System.Windows.Forms.ListBox lstKeywordsMain;
        private System.Windows.Forms.ListBox lstSeparatorsMain;
        private System.Windows.Forms.ListBox lstVariablesMain;
        private System.Windows.Forms.ListBox lstConstantsMain;
        private System.Windows.Forms.Label labelK4;
        private System.Windows.Forms.Label labelK2;
        private System.Windows.Forms.Label labelK3;
        private System.Windows.Forms.Label labelK1;
        private System.Windows.Forms.Label labelInputText;
        public System.Windows.Forms.ListBox lstResultsMain;
        public System.Windows.Forms.ListBox lstSyntax;
        private System.Windows.Forms.Button buttonCopyListToBox;
        private System.Windows.Forms.RichTextBox richTextSyntax;
        private System.Windows.Forms.ListBox lstSemantics;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRPNMain;
        private System.Windows.Forms.TextBox txtRPNgen;
        private System.Windows.Forms.TextBox txtCsharpMain;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCsharpGen;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
    }
}
