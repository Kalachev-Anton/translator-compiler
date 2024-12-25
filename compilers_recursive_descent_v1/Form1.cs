using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Text;


namespace recursive_descent_translator_app
{
    public partial class Form1 : Form
    {
        private static List<string> keywords = new List<string> { "int", "float", "bool", "if", "then", "else", "for", "to", "do", "while", "do", "read", "write", "true", "false", "ass" }; // K1
        private static List<string> separators = new List<string> { "{", "}", "+", "-", "or", ">=", "<=", "<>", "=", "<", ">", "*", "/", "and", "not", ";", ",", "(", ")", "." }; // K2

        private static List<string> uniqueKeywords = new List<string>();
        private static List<string> uniqueSeparators = new List<string>();
        private static List<string> uniqueVariables = new List<string>();
        private static List<string> uniqueConstants = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void lstSyntax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                System.Text.StringBuilder copy_buffer = new System.Text.StringBuilder();
                foreach (object item in lstSyntax.SelectedItems)
                    copy_buffer.AppendLine(item.ToString());
                if (copy_buffer.Length > 0)
                    Clipboard.SetText(copy_buffer.ToString());
            }
        }
        private void lstSemantics_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                System.Text.StringBuilder copy_buffer = new System.Text.StringBuilder();
                foreach (object item in lstSyntax.SelectedItems)
                    copy_buffer.AppendLine(item.ToString());
                if (copy_buffer.Length > 0)
                    Clipboard.SetText(copy_buffer.ToString());
            }
        }

        //Скопировать содержимое из listBox в textBox для более удобной работы
        private void buttonCopyListToBox_Click(object sender, EventArgs e)
        {
            // Собираем все элементы из ListBox в одну строку
            string allItems = string.Join(Environment.NewLine, lstSyntax.Items.Cast<string>());

            // Устанавливаем собранный текст в TextBox
            richTextSyntax.Text = allItems;
        }
        
        // Главная кнопка трансляции и анализа
        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            bool hasError = false; // Флаг для отслеживания ошибок

            // Очистка всех элементов перед началом анализа и трансляции
            lstKeywordsLex.Items.Clear();
            lstKeywordsMain.Items.Clear();
            lstSeparatorsLex.Items.Clear();
            lstSeparatorsMain.Items.Clear();
            lstVariablesLex.Items.Clear();
            lstVariablesMain.Items.Clear();
            lstConstantsLex.Items.Clear();
            lstConstantsMain.Items.Clear();
            lstResultsMain.Items.Clear();
            lstSyntax.Items.Clear();
            lstSemantics.Items.Clear();
            uniqueKeywords.Clear();
            uniqueSeparators.Clear();
            uniqueVariables.Clear();
            uniqueConstants.Clear();
            txtCsharpGen.Clear();

            // Начало лексического анализа
            try
            {
                string inputCode = txtInputMain.Text;

                // Удаляем комментарии
                string noCommentCode = RemoveComments(inputCode);
                txtNoComments.Text = noCommentCode;

                // Анализируем лексемы
                List<Tuple<string, int>> lexemes = AnalyzeLexemes(noCommentCode, this); // Передаем ссылку на текущую форму

                // Отображаем дескрипторный текст
                DisplayDescriptorText(lexemes);

                // Отображаем восстановленный код
                txtRecovered.Text = RecoverCode(lexemes);

                // Отображаем таблицы лексем
                DisplayLexemeTables();

                // Результаты текущей работы
                lstResultsMain.Items.Add("1. Лексический анализ завершен. Проверьте есть ли ошибки.");


                // Начало синтаксического анализа
                string input = noCommentCode;

                try
                {
                    // Включаем логирование для каждой итерации
                    lstSyntax.Items.Add("Начало синтаксического анализа...");

                    // Выполняем анализ
                    bool success = Parse(input);

                    if (success)
                    {
                        lstResultsMain.Items.Add("2. Синтаксический анализ завершен успешно.");
                    }
                    else
                    {
                        lstResultsMain.Items.Add("Ошибка синтаксического анализа.");
                    }

                    // Начало семантического анализа
                    inputCode = noCommentCode;
                    List<string> semantics = new List<string>(); // Список для промежуточных результатов
                    List<string> results = new List<string>();   // Список для результатов
                    hasError = false; // Флаг для отслеживания ошибок

                    try
                    {
                        // Этап 1: Проверка на правильность фигурных скобок
                        if (!inputCode.StartsWith("{") || !inputCode.EndsWith("}"))
                        {
                            hasError = true;
                            throw new Exception("Код должен начинаться с '{' и заканчиваться '}'");
                        }

                        // Этап 2: Парсинг программы
                        var blocks = ParseProgram(inputCode);
                        semantics.Add("Программа успешно распарсена");

                        // Этап 3: Семантический анализ
                        var analyzer = new SemanticAnalyzer(
                            lstKeywordsMain.Items.Cast<string>().ToList(),
                            lstSeparatorsMain.Items.Cast<string>().ToList(),
                            lstVariablesMain.Items.Cast<string>().ToList(),
                            lstConstantsMain.Items.Cast<string>().ToList()
                        );

                        foreach (var block in blocks)
                        {
                            analyzer.Analyze(block, semantics, results); // Передаем оба аргумента: semantics и results
                                                                         // Проверяем наличие ошибок после анализа каждого блока
                            if (results.Any(r => r.StartsWith("Ошибка:")))
                            {
                                hasError = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ошибка: добавляем её в оба списка
                        semantics.Add($"Ошибка: {ex.Message}");
                        results.Add($"Ошибка: {ex.Message}");
                        hasError = true; // Устанавливаем флаг ошибки
                    }

                    // Убираем DataSource, чтобы можно было добавлять элементы напрямую в ListBox
                    lstSemantics.DataSource = null;
                    lstResultsMain.DataSource = null;

                    lstSemantics.Items.AddRange(semantics.ToArray());
                    lstResultsMain.Items.AddRange(results.ToArray());

                    // Проверяем флаг hasError для отображения результата
                    if (!hasError)
                    {
                        lstResultsMain.Items.Add("3. Семантический анализ завершен успешно.");
                    }

                    // Генерация ПолИЗ'а / ОПЗ

                    List<string> intermediateSteps = new List<string>();

                    // Токенизация входного кода
                    List<string> tokens = Tokenize(noCommentCode);

                    // Генерация ОПЗ
                    string rpnResult = GenerateRPN(tokens, intermediateSteps);

                    // Отображаем результат в textBox
                    txtRPNMain.Text = rpnResult;

                    // Промежуточные вычисления
                    txtRPNgen.Text = string.Join(Environment.NewLine, intermediateSteps);

                    lstResultsMain.Items.Add("4. Формирование ПолИЗ'а (ОПЗ) завершено. Проверьте есть ли ошибки.");

                    // Генерация кода на C#
                    txtCsharpGen.Text += "Начало генерации кода.\n\r\n\r";

                    string ConvertToCSharp = noCommentCode;
                    txtCsharpGen.Text += "Добавляем библиотеки и описываем главный метод.\n\r\n\r";
                    ConvertToCSharp = ConvertToCSharp.Replace("}", "    Console.ReadLine();\r\n    }\r\n}");
                    ConvertToCSharp = ConvertToCSharp.Replace(" = ", "==");
                    ConvertToCSharp = ConvertToCSharp.Replace("<>", "!=");
                    ConvertToCSharp = ConvertToCSharp.Replace("ass", "=");
                    ConvertToCSharp = ConvertToCSharp.Replace("{", "using System;\r\n\n\r" +
                        "using System.IO;\r\n\r\n" +
                        "class Program\r\n" +
                        "{\r\n    " +
                        "static void Main()\r\n    " +
                        "{");
                    txtCsharpGen.Text += "Главный метод оформлен. Проставлены все { и }\n\r\n\r";
                    ConvertToCSharp = ConvertToCSharp.Replace("write (", "Console.WriteLine(");
                    txtCsharpGen.Text += "Трансформируем write в Console.WriteLine\n\r\n\r";
                    ConvertToCSharp = ConvertToCSharp.Replace("read (", "");
                    ConvertToCSharp = ConvertToCSharp.Replace(")  ;", "= int.Parse(Console.ReadLine());");
                    txtCsharpGen.Text += "Трансформируем read в int.Parse(Console.ReadLine())\n\r\n\r";
                    ConvertToCSharp = ConvertToCSharp.Replace("if", "if (");
                    ConvertToCSharp = ConvertToCSharp.Replace("then", ") ");
                    ConvertToCSharp = ConvertToCSharp.Replace("else", "; else ");
                    ConvertToCSharp = ConvertToCSharp.Replace("for", "for (;");
                    ConvertToCSharp = ConvertToCSharp.Replace("to", ";");
                    ConvertToCSharp = ConvertToCSharp.Replace("do", ") ");
                    ConvertToCSharp = ConvertToCSharp.Replace("while", "while (");
                    txtCsharpGen.Text += "Условие if then else сформировано.\n\r\n\r";
                    txtCsharpGen.Text += "Цикл for преобразован.\n\r\n\r";
                    txtCsharpGen.Text += "Цикл while преобразован.\n\r\n\r";

                    // Вывод результата
                    lstResultsMain.Items.Add("5. Генерация кода на C# завернеша. Проверьте есть ли ошибки.");
                    txtCsharpMain.Text = ConvertToCSharp;
                }
                catch (Exception ex)
                {
                    lstResultsMain.Items.Add("Произошла ошибка: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                lstResultsMain.Items.Add($"Произошла ошибка: {ex.Message}");
            }
        }

        // Разбиение кода на блоки
        private List<Block> ParseProgram(string inputCode)
        {
            // Разбиение кода на блоки по строкам
            var lines = inputCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var blocks = new List<Block>();

            foreach (var line in lines)
            {
                blocks.Add(new Block(line.Trim())); // Добавляем каждый блок
            }

            return blocks;
        }

        // Кнопка заглузки текста из файла
        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Выберите файл для анализа";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        string fileContent = File.ReadAllText(filePath);
                        txtInputMain.Text = fileContent;
                        lstResultsMain.Items.Add("Файл успешно загружен.");
                    }
                    catch (Exception ex)
                    {
                        lstResultsMain.Items.Add($"Ошибка при чтении файла: {ex.Message}");
                    }
                }
            }
        }


        // Начало блока лексического анализа
        // Убрать строки с комментариями
        private static string RemoveComments(string code)
        {
            return Regex.Replace(code, @"//.*?\n|/\*.*?\*/", "\n", RegexOptions.Singleline);
        }

        // Формирование таблиц K1-4
        private static List<Tuple<string, int>> AnalyzeLexemes(string code, Form1 form)
        {
            List<Tuple<string, int>> lexemes = new List<Tuple<string, int>>();
            string pattern = @"[a-zA-Z0-9]+|>=|<=|[;:{}()\[\]+\-*/=<>!,]";
            MatchCollection matches = Regex.Matches(code, pattern);

            foreach (Match match in matches)
            {
                string token = match.Value.Trim();
                if (string.IsNullOrWhiteSpace(token)) continue;

                int index = -1;

                // Проверяем, является ли токен ключевым словом
                if (keywords.Contains(token))
                {
                    index = GetOrAddLexeme(token, uniqueKeywords);
                    lexemes.Add(new Tuple<string, int>("K1", index));
                }
                // Проверяем, является ли токен разделителем
                else if (separators.Contains(token))
                {
                    index = GetOrAddLexeme(token, uniqueSeparators);
                    lexemes.Add(new Tuple<string, int>("K2", index));
                }
                // Проверяем, является ли токен десятичным числом
                else if (Regex.IsMatch(token, @"^\d+$")) // Десятичное число (например, 10)
                {
                    index = GetOrAddLexeme(token, uniqueConstants);
                    lexemes.Add(new Tuple<string, int>("K4", index));
                }
                // Проверяем, является ли токен десятичным числом с постфиксом
                else if (Regex.IsMatch(token, @"^[0-9]+[dDeE]$")) // Десятичное число (например, 10d или 10D)
                {
                    index = GetOrAddLexeme(token, uniqueConstants);
                    lexemes.Add(new Tuple<string, int>("K4", index));
                }
                // Проверяем, является ли токен восьмиричным числом с постфиксом
                else if (Regex.IsMatch(token, @"^[0-7]+[oO]$")) // Востмиричное число (например, 10o или 10o)
                {
                    index = GetOrAddLexeme(token, uniqueConstants);
                    lexemes.Add(new Tuple<string, int>("K4", index));
                }
                // двоичное
                else if (Regex.IsMatch(token, @"^[0-1]+[bB]$")) // Двоичное число (например, 10b или 10B)
                {
                    index = GetOrAddLexeme(token, uniqueConstants);
                    lexemes.Add(new Tuple<string, int>("K4", index));
                }
                // Проверяем, является ли токен шестнадцатеричным числом (например, 10h или 10H)
                else if (Regex.IsMatch(token, @"^[0-9A-Ea-e]+[hH]$"))
                {
                    index = GetOrAddLexeme(token, uniqueConstants);
                    lexemes.Add(new Tuple<string, int>("K4", index));
                }
                // Проверяем, является ли токен шестнадцатеричным числом (например, 10h или 10H)
                else if (Regex.IsMatch(token, @"^[0-9A-Ea-e]+[hH]$"))
                {
                    index = GetOrAddLexeme(token, uniqueConstants);
                    lexemes.Add(new Tuple<string, int>("K4", index));
                }
                // Проверяем, является ли токен числом с экспоненциальным порядком (например, 10e+12 или 98E-13)
                else if (Regex.IsMatch(token, @"^[0-9]+([eE][+-]?[0-9]+)$"))
                {
                    index = GetOrAddLexeme(token, uniqueConstants);
                    lexemes.Add(new Tuple<string, int>("K4", index));
                }

                // Проверяем, является ли токен переменной (начинается с буквы, далее буквы)
                else if (Regex.IsMatch(token, @"^[a-zA-Z]\w*$"))
                {
                    index = GetOrAddLexeme(token, uniqueVariables);
                    lexemes.Add(new Tuple<string, int>("K3", index));
                }
                // Если токен не соответствует ни одному из типов
                else
                {
                    // Добавляем ошибку в lstResultsMain
                    form.lstResultsMain.Items.Add($"Ошибка: Незафиксированный символ '{token}' на позиции {match.Index + 1}");
                }
            }

            // Проверяем на незарегистрированные символы, которые не попали в регулярку
            foreach (char c in code)
            {
                // Если символ не является буквой, цифрой или разделителем
                if (!Char.IsLetterOrDigit(c) && !separators.Contains(c.ToString()) && !Char.IsWhiteSpace(c))
                {
                    // Добавляем ошибку для каждого незарегистрированного символа
                    form.lstResultsMain.Items.Add($"Ошибка: Незафиксированный символ '{c}' на позиции {code.IndexOf(c) + 1}");
                }
            }
            return lexemes;
        }

        // Вспомогательный метод
        private static int GetOrAddLexeme(string lexeme, List<string> lexemeList)
        {
            int index = lexemeList.IndexOf(lexeme);
            if (index == -1)
            {
                lexemeList.Add(lexeme);
                return lexemeList.Count - 1;
            }
            return index;
        }

        // Восстановление текста из дескрипторов
        private static string RecoverCode(List<Tuple<string, int>> lexemes)
        {
            string recoveredCode = "";
            foreach (var lexeme in lexemes)
            {
                string lexemeValue = "";
                switch (lexeme.Item1)
                {
                    case "K1":
                        lexemeValue = uniqueKeywords[lexeme.Item2];
                        break;
                    case "K2":
                        lexemeValue = uniqueSeparators[lexeme.Item2];
                        break;
                    case "K3":
                        lexemeValue = uniqueVariables[lexeme.Item2];
                        break;
                    case "K4":
                        lexemeValue = uniqueConstants[lexeme.Item2];
                        break;
                }
                recoveredCode += lexemeValue + " ";
            }
            return recoveredCode.Trim();
        }

        // Отобразить дескрипторный текст
        private void DisplayDescriptorText(List<Tuple<string, int>> lexemes)
        {
            txtDescriptor.Clear();
            foreach (var lexeme in lexemes)
            {
                txtDescriptor.AppendText($"({lexeme.Item1}, {lexeme.Item2})\t");
            }
        }

        //Отображение таблиц K1-4 на вкладках Главная и Лексический анализ
        private void DisplayLexemeTables()
        {
            lstKeywordsLex.Items.Clear();
            lstSeparatorsLex.Items.Clear();
            lstVariablesLex.Items.Clear();
            lstConstantsLex.Items.Clear();

            // Вкладка Главная
            foreach (var keyword in uniqueKeywords)
                lstKeywordsMain.Items.Add(keyword);

            foreach (var separator in uniqueSeparators)
                lstSeparatorsMain.Items.Add(separator);

            foreach (var variable in uniqueVariables)
                lstVariablesMain.Items.Add(variable);

            foreach (var constant in uniqueConstants)
                lstConstantsMain.Items.Add(constant);

            // Вкладка Лексический анализ
            foreach (var keyword in uniqueKeywords)
                lstKeywordsLex.Items.Add(keyword);

            foreach (var separator in uniqueSeparators)
                lstSeparatorsLex.Items.Add(separator);

            foreach (var variable in uniqueVariables)
                lstVariablesLex.Items.Add(variable);

            foreach (var constant in uniqueConstants)
                lstConstantsLex.Items.Add(constant);
        }

        // Начало блока синтаксического анализа
        // Главная функция парсинга для PR
        private bool Parse(string input)
        {
            lstSyntax.Items.Add("Начинаем разбор...");
            return ParsePR(input);
        }

        // Разбор PR
        private bool ParsePR(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить PR → { BL1 } ...");
            if (input.StartsWith("{") && input.EndsWith("}"))
            {
                string body = input.Substring(1, input.Length - 2).Trim(); // Убираем { и }
                return ParseBL1(body);
            }
            lstSyntax.Items.Add("Ошибка: PR должен начинаться с { и заканчиваться }.");
            return false;
        }

        // Разбор BL1
        private bool ParseBL1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BL1 → DESC; BL2 | OPRT; BL2 ...");
            string[] statements = input.Split(';'); // Разделяем операторы
            foreach (string statement in statements)
            {
                string trimmed = statement.Trim();
                if (!string.IsNullOrEmpty(trimmed) && !ParseDESC(trimmed) && !ParseOPRT(trimmed))
                {
                    lstSyntax.Items.Add("Ошибка: BL1 не удалось распарсить.");
                    return false;
                }
            }
            lstSyntax.Items.Add("BL1 успешно распарсено.");
            return true;
        }

        // Разбор BL2
        private bool ParseBL2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BL2 → DESC; BL3 | OPRT; BL3 | eps ...");
            input = input.Trim();

            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("BL2 пусто, успешно завершено.");
                return true; // BL2 → eps
            }

            int semicolonIndex = input.IndexOf(';');
            if (semicolonIndex == -1)
            {
                lstSyntax.Items.Add("Ошибка: BL1 не содержит символа ';'.");
                return false;
            }


            string firstPart = input.Substring(0, semicolonIndex).Trim();
            string secondPart = input.Substring(semicolonIndex + 1).Trim();

            lstSyntax.Items.Add($"firstPart: '{firstPart}'");
            lstSyntax.Items.Add($"secondPart: '{secondPart}'");

            // Попытка парсинга DESC
            if (ParseDESC(firstPart) && ParseBL3(secondPart))
            {
                lstSyntax.Items.Add("BL2 успешно распарсено как DESC; BL3.");
                return true;
            }

            // Если DESC не удалась, пробуем OPRT
            if (ParseOPRT(firstPart) && ParseBL3(secondPart))
            {
                lstSyntax.Items.Add("BL2 успешно распарсено как OPRT; BL3.");
                return true;
            }

            throw new Exception("Ошибка синтаксического анализа в BL2.");
        }

        // Разбор BL3
        private bool ParseBL3(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BL3 → DESC; BL2 | OPRT; BL2 | eps ...");
            input = input.Trim();

            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("BL3 пусто, успешно завершено.");
                return true; // BL3 → eps
            }

            int semicolonIndex = input.IndexOf(';');
            if (semicolonIndex == -1)
            {
                throw new Exception("Ошибка: Ожидался символ ';' в BL3.");
            }

            string firstPart = input.Substring(0, semicolonIndex).Trim();
            string secondPart = input.Substring(semicolonIndex + 1).Trim();

            lstSyntax.Items.Add($"firstPart: '{firstPart}'");
            lstSyntax.Items.Add($"secondPart: '{secondPart}'");

            // Попытка парсинга DESC
            if (ParseDESC(firstPart) && ParseBL2(secondPart))
            {
                lstSyntax.Items.Add("BL3 успешно распарсено как DESC; BL2.");
                return true;
            }

            // Если DESC не удалась, пробуем OPRT
            if (ParseOPRT(firstPart) && ParseBL2(secondPart))
            {
                lstSyntax.Items.Add("BL3 успешно распарсено как OPRT; BL2.");
                return true;
            }

            throw new Exception("Ошибка синтаксического анализа в BL3.");
        }

        // Разбор DESC
        private bool ParseDESC(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить DESC → TYPE ID ID1...");

            // Разбиваем строку на тип и остаток
            string[] parts = input.Split(new[] { ' ' }, 2);
            if (parts.Length < 2) return false; // Если нет типа и переменной

            string type = parts[0].Trim();
            string rest = parts[1].Trim();

            if (ParseTYPE(type) && ParseIDList(rest)) // Сначала проверяем TYPE, затем список ID
            {
                lstSyntax.Items.Add("DESC успешно распарсено.");
                return true;
            }
            return false;
        }

        // Метод для парсинга типа
        private bool ParseTYPE(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить TYPE → int | float | bool ...");
            // Допустимые типы (например, int, bool и т.д.)
            return input == "int" || input == "bool" || input == "float";
        }

        // Разбор ID1
        private bool ParseID1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить ID1 → , ID ID2 | eps ...");

            if (string.IsNullOrWhiteSpace(input))
            {
                lstSyntax.Items.Add("ID1 пусто, успешно.");
                return true; // eps
            }

            if (input.StartsWith(","))
            {
                string trimmed = input.Substring(1).Trim(); // Убираем запятую
                int spaceIndex = trimmed.IndexOf(' ');
                if (spaceIndex == -1)
                {
                    return ParseID(trimmed); // Осталась только одна переменная
                }
                string idPart = trimmed.Substring(0, spaceIndex);
                string restPart = trimmed.Substring(spaceIndex + 1).Trim();
                return ParseID(idPart) && ParseID2(restPart); // ID ID2
            }

            lstSyntax.Items.Add("Ошибка: Некорректный формат ID1.");
            return false;
        }

        // Парсинг ID2
        private bool ParseID2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить ID2 → , ID ID1 | eps ...");
            return ParseID1(input); // ID2 имеет ту же структуру
        }

        // Разбор OPRT
        private bool ParseOPRT(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OPRT → ASG | COND | FCYC | LOOP | RD | WRT ...");

            if (string.IsNullOrWhiteSpace(input))
            {
                lstSyntax.Items.Add("OPRT пусто, успешно.");
                return true; // eps
            }

            // Проверка на допустимые символы
            if (!Regex.IsMatch(input, @"^[a-zA-Z0-9+\-*/<>=,().\s]*$"))
            {
                lstSyntax.Items.Add("Ошибка: Некорректные символы в строке.");
                return false;
            }

            // Переменная для отслеживания последнего типа элемента
            string lastType = "";
            string lastOperator = "";

            // Попытка распарсить ASG (присваивание)
            if (TryParseAsg(input, ref lastType, ref lastOperator))
            {
                lstSyntax.Items.Add("Операция - присваивание (ASG).");
                return true;
            }

            // Попытка распарсить COND (условие)
            if (TryParseCond(input, ref lastType, ref lastOperator))
            {
                lstSyntax.Items.Add("Операция - условие (COND).");
                return true;
            }

            // Попытка распарсить FCYC (цикл for)
            if (TryParseFcyc(input, ref lastType, ref lastOperator))
            {
                lstSyntax.Items.Add("Операция - цикл for (FCYC).");
                return true;
            }

            // Попытка распарсить LOOP (цикл while)
            if (TryParseLoop(input, ref lastType, ref lastOperator))
            {
                lstSyntax.Items.Add("Операция - цикл while (LOOP).");
                return true;
            }

            // Попытка распарсить RD (чтение)
            if (TryParseRd(input, ref lastType, ref lastOperator))
            {
                lstSyntax.Items.Add("Операция - чтение (RD).");
                return true;
            }

            // Попытка распарсить WRT (вывод)
            if (TryParseWrt(input, ref lastType, ref lastOperator))
            {
                lstSyntax.Items.Add("Операция - вывод (WRT).");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Некорректная операция OPRT.");
            return false;
        }

        // Вспомагалельный парсер TryParseAsg
        private bool TryParseAsg(string input, ref string lastType, ref string lastOperator)
        {
            string pattern = @"^(?<ID>[a-zA-Z][a-zA-Z0-9]*)\s*ass\s*(?<XPR>.+)$";
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string id = match.Groups["ID"].Value;
                string xpr = match.Groups["XPR"].Value;

                // Проверка, что тип не совпадает с последним
                if (lastOperator == "ASSIGNMENT")
                {
                    lstSyntax.Items.Add("Ошибка: Оператор ass не может быть использован дважды подряд.");
                    return false;
                }

                lstSyntax.Items.Add($"Присваивание: {id} ass {xpr}");

                // Обновляем последний использованный тип и оператор
                lastType = "ASSIGNMENT";
                lastOperator = "ASSIGNMENT";
                return true;
            }

            return false;
        }

        // Вспомагалельный парсер TryParseCond
        private bool TryParseCond(string input, ref string lastType, ref string lastOperator)
        {
            string pattern = @"^if\s*(?<XPR>.+?)\s*then\s*(?<OPRT>.+?)\s*(else\s*(?<COND>.+))?$";
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string xpr = match.Groups["XPR"].Value;
                string oprt = match.Groups["OPRT"].Value;
                string cond = match.Groups["COND"].Value;

                // Проверка, что тип не совпадает с последним
                if (lastOperator == "OPERATOR")
                {
                    lstSyntax.Items.Add("Ошибка: Операторы не могут быть использованы дважды подряд.");
                    return false;
                }

                lstSyntax.Items.Add($"Условие: if {xpr} then {oprt} else {cond}");

                // Обновляем последний использованный тип и оператор
                lastType = "OPERATOR";
                lastOperator = "OPERATOR";
                return true;
            }

            return false;
        }

        // Вспомагалельный парсер TryParseFcyc
        private bool TryParseFcyc(string input, ref string lastType, ref string lastOperator)
        {
            string pattern = @"^for\s*(?<ASG>.+?)\s*to\s*(?<XPR>.+?)\s*do\s*(?<OPRT>.+)$";
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string asg = match.Groups["ASG"].Value;
                string xpr = match.Groups["XPR"].Value;
                string oprt = match.Groups["OPRT"].Value;

                // Проверка, что тип не совпадает с последним
                if (lastOperator == "OPERATOR")
                {
                    lstSyntax.Items.Add("Ошибка: Операторы не могут быть использованы дважды подряд.");
                    return false;
                }

                lstSyntax.Items.Add($"Цикл for: for {asg} to {xpr} do {oprt}");

                // Обновляем последний использованный тип и оператор
                lastType = "OPERATOR";
                lastOperator = "OPERATOR";
                return true;
            }

            return false;
        }

        // Вспомагалельный парсер TryParseLoop
        private bool TryParseLoop(string input, ref string lastType, ref string lastOperator)
        {
            string pattern = @"^while\s*(?<XPR>.+?)\s*do\s*(?<OPRT>.+)$";
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string xpr = match.Groups["XPR"].Value;
                string oprt = match.Groups["OPRT"].Value;

                // Проверка, что тип не совпадает с последним
                if (lastOperator == "OPERATOR")
                {
                    lstSyntax.Items.Add("Ошибка: Операторы не могут быть использованы дважды подряд.");
                    return false;
                }

                lstSyntax.Items.Add($"Цикл while: while {xpr} do {oprt}");

                // Обновляем последний использованный тип и оператор
                lastType = "OPERATOR";
                lastOperator = "OPERATOR";
                return true;
            }

            return false;
        }

        // Вспомагалельный парсер TryParseRd
        private bool TryParseRd(string input, ref string lastType, ref string lastOperator)
        {
            string pattern = @"^read\s*\(\s*(?<ID>[a-zA-Z][a-zA-Z0-9]*)(\s*(,\s*(?<ID2>[a-zA-Z][a-zA-Z0-9]*))*)?\s*\)$";
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string id = match.Groups["ID"].Value;
                string id2 = match.Groups["ID2"].Value;

                // Проверка, что тип не совпадает с последним
                if (lastOperator == "ID")
                {
                    lstSyntax.Items.Add("Ошибка: Идентификатор не может быть использован дважды подряд.");
                    return false;
                }

                lstSyntax.Items.Add($"Чтение: read ({id}, {id2})");

                // Обновляем последний использованный тип и оператор
                lastType = "ID";
                lastOperator = "ID";
                return true;
            }

            return false;
        }

        // Вспомагалельный парсер TryParseWrt
        private bool TryParseWrt(string input, ref string lastType, ref string lastOperator)
        {
            string pattern = @"^write\s*\(\s*(?<XPR>.+?)(\s*(,\s*(?<XPR2>.+?))*)?\s*\)$";
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string xpr = match.Groups["XPR"].Value;
                string xpr2 = match.Groups["XPR2"].Value;

                // Проверка, что тип не совпадает с последним
                if (lastOperator == "OPERATOR")
                {
                    lstSyntax.Items.Add("Ошибка: Операторы не могут быть использованы дважды подряд.");
                    return false;
                }

                lstSyntax.Items.Add($"Вывод: write ({xpr}, {xpr2})");

                // Обновляем последний использованный тип и оператор
                lastType = "OPERATOR";
                lastOperator = "OPERATOR";
                return true;
            }

            return false;
        }

        // Разбор TYPE
        private bool ParseTYPE(ref string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить TYPE...");

            // Удаляем начальные пробелы
            input = input.Trim();

            // Проверка на соответствие типу
            if (input.StartsWith("int"))
            {
                input = input.Substring(3).Trim(); // Убираем "int"
                lstSyntax.Items.Add("TYPE успешно распарсено: int.");
                return true;
            }
            else if (input.StartsWith("float"))
            {
                input = input.Substring(5).Trim(); // Убираем "float"
                lstSyntax.Items.Add("TYPE успешно распарсено: float.");
                return true;
            }
            else if (input.StartsWith("bool"))
            {
                input = input.Substring(4).Trim(); // Убираем "bool"
                lstSyntax.Items.Add("TYPE успешно распарсено: bool.");
                return true;
            }
            else
            {
                lstSyntax.Items.Add("Ошибка: Неопознанный тип в TYPE.");
                return false;
            }
        }

        // Метод для парсинга списка ID, разделённых запятыми
        private bool ParseIDList(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить список ID (ID, ID, ID, ID и т.д.)...");

            string[] parts = input.Split(',');
            foreach (var part in parts)
            {
                string trimmedPart = part.Trim();
                if (!ParseID(trimmedPart))
                {
                    return false; // Если хотя бы один ID невалиден, возвращаем false
                }
            }

            return true; // Все ID валидны
        }

        // Метод для парсинга одного ID
        private bool ParseID(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить ID → LETTER LTRDGT1 ...");

            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("Ошибка: пустой ID.");
                return false;
            }

            if (char.IsLetter(input[0])) // Начинается с буквы
            {
                return ParseLTRDGT1(input.Substring(1)); // Проверяем остаток
            }

            lstSyntax.Items.Add("Ошибка: ID должен начинаться с буквы.");
            return false;
        }

        // Разбор LTRDGT1
        private bool ParseLTRDGT1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить LTRDGT1 → LETTER LTRDGT2 | DIGIT LTRDGT2 | eps ...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("LTRDGT1 пусто, корректно.");
                return true; // eps
            }

            return ParseLTRDGT2(input);
        }

        // Разбор LTRDGT2
        private bool ParseLTRDGT2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить LTRDGT2 → LETTER LTRDGT1 | DIGIT LTRDGT1 | eps ...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("LTRDGT2 пусто, корректно.");
                return true; // eps
            }

            if (char.IsLetter(input[0]) || char.IsDigit(input[0]))
            {
                return ParseLTRDGT2(input.Substring(1));
            }
            else
            {
                //return false;
                //lstSyntax.Items.Add("Ошибка: LTRDGT2 может содержать только буквы или цифры.");
                throw new Exception("Ошибка: LTRDGT2 может содержать только буквы или цифры.");
            }
        }

        // Разбор ASG
        private bool ParseASG(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить ASG...");

            string[] parts = input.Split(new[] { " ass " }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                bool validID = ParseID(parts[0].Trim());
                bool validXPR = ParseXPR(parts[1].Trim());

                if (validID && validXPR)
                {
                    lstSyntax.Items.Add("ASG успешно распарсено.");
                    return true;
                }
                else
                {
                    lstSyntax.Items.Add("Ошибка: Некорректные идентификатор или выражение в ASG.");
                }
            }
            else
            {
                lstSyntax.Items.Add("Ошибка: Неверный формат ASG.");
            }
            return false;
        }

        // Разбор COND
        private bool ParseCOND(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить COND...");

            string[] parts = input.Split(new[] { " then " }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                lstSyntax.Items.Add("Ошибка: Неверный формат COND.");
                return false;
            }

            string conditionPart = parts[0].Substring(3).Trim(); // Убираем "if"
            string thenPart = parts[1].Trim();
            string elsePart = null;

            if (thenPart.Contains(" else "))
            {
                string[] thenElseSplit = thenPart.Split(new[] { " else " }, StringSplitOptions.None);
                thenPart = thenElseSplit[0].Trim();
                elsePart = thenElseSplit[1].Trim();
            }

            bool validCondition = ParseXPR(conditionPart);
            bool validThen = ParseOPRT(thenPart);
            bool validElse = elsePart == null || ParseOPRT(elsePart);

            if (validCondition && validThen && validElse)
            {
                lstSyntax.Items.Add("COND успешно распарсено.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Некорректные элементы в COND.");
            return false;
        }

        // Разбор COND1
        private bool ParseCOND1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить COND1...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("COND1 пусто, корректно.");
                return true; // eps
            }

            // Ожидается "else OPRT"
            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 && parts[0] == "else")
            {
                bool validOPRT = ParseOPRT(parts[1]);
                if (validOPRT)
                {
                    lstSyntax.Items.Add("COND1 успешно распарсено.");
                    return true;
                }
                else
                {
                    throw new Exception("Ошибка: Неопознанный оператор в COND1.");
                }
            }

            throw new Exception("Ошибка: Неверный формат COND1.");
        }

        // Разбор FCYC
        private bool ParseFCYC(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить FCYC...");

            // Формат: "for ASG to XPR do OPRT"
            string[] parts = input.Split(new[] { " to ", " do " }, StringSplitOptions.None);
            if (parts.Length < 3)
            {
                lstSyntax.Items.Add("Ошибка: Неверный формат FCYC.");
                return false;
            }

            string asgPart = parts[0].Substring(3).Trim(); // Убираем "for"
            string toPart = parts[1].Trim();
            string oprtPart = parts[2].Trim();

            bool validASG = ParseASG(asgPart);
            bool validTo = ParseXPR(toPart);
            bool validOPRT = ParseOPRT(oprtPart);

            if (validASG && validTo && validOPRT)
            {
                lstSyntax.Items.Add("FCYC успешно распарсено.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Некорректные элементы в FCYC.");
            return false;
        }

        // Разбор LOOP
        private bool ParseLOOP(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить LOOP...");

            string[] parts = input.Split(new[] { " do " }, StringSplitOptions.None);
            if (parts.Length < 2)
            {
                lstSyntax.Items.Add("Ошибка: Неверный формат LOOP.");
                return false;
            }

            string xprPart = parts[0].Substring(5).Trim(); // Убираем "while"
            string oprtPart = parts[1].Trim();

            bool validCondition = ParseXPR(xprPart);
            bool validOPRT = ParseOPRT(oprtPart);

            if (validCondition && validOPRT)
            {
                lstSyntax.Items.Add("LOOP успешно распарсено.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Некорректные элементы в LOOP.");
            return false;
        }

        // Разбор RD
        private bool ParseRD(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить RD...");

            // Проверяем, начинается ли строка с "read("
            if (input.StartsWith("read") && input.Length > 4)
            {
                string param = input.Substring(4).Trim(); // Получаем строку после "read"
                if (param.StartsWith("(") && param.EndsWith(")"))
                {
                    string inside = param.Substring(1, param.Length - 2).Trim(); // Извлекаем содержимое в скобках

                    return ParseIDList(inside); // Парсим список ID
                }
            }
            return false;
        }

        // Разбор RD1
        private bool ParseRD1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить RD1...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("RD1 пусто, корректно.");
                return true; // eps
            }

            // Ожидается ", ID RD2"
            string[] parts = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                bool validID = ParseID(parts[0].Trim());
                bool validRD2 = ParseRD2(string.Join(",", parts.Skip(1)));

                if (validID && validRD2)
                {
                    lstSyntax.Items.Add("RD1 успешно распарсено.");
                    return true;
                }
                else
                {
                    throw new Exception("Ошибка: Неопознанный идентификатор или RD2 в RD1.");
                }
            }

            throw new Exception("Ошибка: Неверный формат RD1.");
        }

        // Разбор RD2
        private bool ParseRD2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить RD2...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("RD2 пусто, корректно.");
                return true; // eps
            }

            // Ожидается ", ID RD1"
            string[] parts = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                bool validID = ParseID(parts[0].Trim());
                bool validRD1 = ParseRD1(string.Join(",", parts.Skip(1)));

                if (validID && validRD1)
                {
                    lstSyntax.Items.Add("RD2 успешно распарсено.");
                    return true;
                }
                else
                {
                    throw new Exception("Ошибка: Неопознанный идентификатор или RD1 в RD2.");
                }
            }

            throw new Exception("Ошибка: Неверный формат RD2.");
        }

        // Разбор WRT
        private bool ParseWRT(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить WRT...");

            if (input.StartsWith("write (") && input.EndsWith(")"))
            {
                string content = input.Substring(7, input.Length - 8).Trim(); // Убираем "write (" и ")"
                if (ParseXPR(content))
                {
                    lstSyntax.Items.Add("WRT успешно распарсено.");
                    return true;
                }
                else
                {
                    lstSyntax.Items.Add("Ошибка: Некорректное выражение в WRT.");
                }
            }
            else
            {
                lstSyntax.Items.Add("Ошибка: Неверный формат WRT.");
            }
            return false;
        }

        // Разбор WRT1
        private bool ParseWRT1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить WRT1...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("WRT1 пусто, корректно.");
                return true; // eps
            }

            // Ожидается ", XPR WRT2"
            string[] parts = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                bool validXPR = ParseXPR(parts[0].Trim());
                bool validWRT2 = ParseWRT2(string.Join(",", parts.Skip(1)));

                if (validXPR && validWRT2)
                {
                    lstSyntax.Items.Add("WRT1 успешно распарсено.");
                    return true;
                }
                else
                {
                    throw new Exception("Ошибка: Неопознанное выражение или WRT2 в WRT1.");
                }
            }

            throw new Exception("Ошибка: Неверный формат WRT1.");
        }

        // Разбор WRT2
        private bool ParseWRT2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить WRT2...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("WRT2 пусто, корректно.");
                return true; // eps
            }

            // Ожидается ", XPR WRT1"
            string[] parts = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                bool validXPR = ParseXPR(parts[0].Trim());
                bool validWRT1 = ParseWRT1(string.Join(",", parts.Skip(1)));

                if (validXPR && validWRT1)
                {
                    lstSyntax.Items.Add("WRT2 успешно распарсено.");
                    return true;
                }
                else
                {
                    throw new Exception("Ошибка: Неопознанное выражение или WRT1 в WRT2.");
                }
            }

            throw new Exception("Ошибка: Неверный формат WRT2.");
        }

        // Разбор LETTER
        private bool ParseLETTER(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить LETTER → A | B | C | ...| X | Y | Z | a | b | c | ... | x | y | z\r\n...");
            // Проверяем, является ли символ буквой
            if (input.Length == 1 && Char.IsLetter(input[0]))
            {
                lstSyntax.Items.Add("LETTER успешно распарсено.");
                return true;
            }
            else
            {
                throw new Exception("Ошибка: Не является буквой.");
            }
        }

        // Разбор DIGIT
        private bool ParseDIGIT(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить DIGIT → 0|1|2|3|4|5|6|7|8|9 ...");
            // Проверяем, является ли символ цифрой
            if (input.Length == 1 && Char.IsDigit(input[0]))
            {
                lstSyntax.Items.Add("DIGIT успешно распарсено.");
                return true;
            }
            else
            {
                throw new Exception("Ошибка: Не является цифрой.");
            }
        }

        // Разбор XPR
        private bool ParseXPR(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить XPR...");

            if (string.IsNullOrWhiteSpace(input))
            {
                lstSyntax.Items.Add("Ошибка: XPR пусто.");
                return false;
            }

            // Разбиваем строку и обрабатываем первое выражение
            string[] parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            string firstPart = parts[0];
            string restPart = parts.Length > 1 ? parts[1] : "";

            if (ParseOPRD(firstPart) && ParseXPR1(restPart))
            {
                lstSyntax.Items.Add("XPR успешно распарсено.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Не удалось распарсить XPR.");
            return false;
        }

        // Разбор XPR1
        private bool ParseXPR1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить XPR1...");

            if (string.IsNullOrWhiteSpace(input))
            {
                lstSyntax.Items.Add("XPR1 пусто, успешно.");
                return true; // eps
            }

            string[] parts = input.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                string operatorPart = parts[0];
                string operandPart = parts[1];
                string rest = parts.Length > 2 ? parts[2] : "";

                // Проверяем ADD или RLSH
                if ((ParseADD(operatorPart) || ParseRLSH(operatorPart)) && ParseOPRD(operandPart) && ParseXPR1(rest))
                {
                    lstSyntax.Items.Add("XPR1 успешно распарсено.");
                    return true;
                }
            }

            lstSyntax.Items.Add("Ошибка: Некорректный формат XPR1.");
            return false;
        }

        // Парсинг XPR2
        private bool ParseXPR2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить XPR2...");
            return ParseXPR1(input); // XPR2 имеет ту же структуру
        }

        // Разбор OPRD
        private bool ParseOPRD(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OPRD...");

            string[] parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            string firstPart = parts[0];
            string restPart = parts.Length > 1 ? parts[1] : "";

            if (ParseTERM(firstPart) && ParseOPRD1(restPart))
            {
                lstSyntax.Items.Add("OPRD успешно распарсено.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Не удалось распарсить OPRD.");
            return false;
        }

        // Парсинг OPRD1
        private bool ParseOPRD1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OPRD1...");

            if (string.IsNullOrWhiteSpace(input))
            {
                lstSyntax.Items.Add("OPRD1 пусто, успешно.");
                return true; // eps
            }

            string[] parts = input.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string add = parts[0];
                string term = parts[1];
                string rest = parts[2];

                if (ParseADD(add) && ParseTERM(term) && ParseOPRD2(rest))
                {
                    lstSyntax.Items.Add("OPRD1 успешно распарсено.");
                    return true;
                }
            }

            lstSyntax.Items.Add("Ошибка: Некорректный формат OPRD1.");
            return false;
        }

        // Разбор OPRD2
        private bool ParseOPRD2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OPRD1...");

            if (string.IsNullOrWhiteSpace(input))
            {
                lstSyntax.Items.Add("OPRD2 пусто, успешно.");
                return true; // eps
            }

            string[] parts = input.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string add = parts[0];
                string term = parts[1];
                string rest = parts[2];

                if (ParseADD(add) && ParseTERM(term) && ParseOPRD1(rest))
                {
                    lstSyntax.Items.Add("OPRD2 успешно распарсено.");
                    return true;
                }
            }

            lstSyntax.Items.Add("Ошибка: Некорректный формат OPRD2.");
            return false;
        }

        // Разбор RLSH (операторы сравнения)
        private bool ParseRLSH(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить RLSH...");

            string[] validOps = { "<>", "=", "<", "<=", ">", ">=" };
            if (validOps.Contains(input))
            {
                lstSyntax.Items.Add("RLSH успешно распарсено.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Некорректный оператор в RLSH.");
            return false;
        }

        // Разбор TERM (термин)
        private bool ParseTERM(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить TERM...");

            string[] parts = input.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                string firstPart = parts[0];
                string operatorPart = parts[1];
                string restPart = string.Join(" ", parts.Skip(2));

                if (ParseMULT(firstPart) && ParseOPMULT(operatorPart) && ParseTERM(restPart))
                {
                    lstSyntax.Items.Add("TERM успешно распарсено.");
                    return true;
                }
            }

            // Если это одиночный MULT
            if (ParseMULT(input))
            {
                lstSyntax.Items.Add("TERM успешно распарсено как одиночный MULT.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: TERM не удалось распарсить.");
            return false;
        }

        // Разбор TERM1 (операции с MULT)
        private bool ParseTERM1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить TERM1...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("TERM1 пусто, корректно.");
                return true; // eps
            }

            // Проверка на "OPMULT MULT TERM2"
            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3)
            {
                bool validOPMULT = ParseOPMULT(parts[0]);
                bool validMULT = ParseMULT(parts[1]);
                bool validTERM2 = ParseTERM2(parts[2]);

                if (validOPMULT && validMULT && validTERM2)
                {
                    lstSyntax.Items.Add("TERM1 успешно распарсено.");
                    return true;
                }
            }

            throw new Exception("Ошибка: Неверный формат TERM1.");
        }

        // Разбор TERM2 (операции с MULT)
        private bool ParseTERM2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить TERM2...");
            if (string.IsNullOrEmpty(input))
            {
                lstSyntax.Items.Add("TERM2 пусто, корректно.");
                return true; // eps
            }

            // Проверка на "OPMULT MULT TERM1"
            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3)
            {
                bool validOPMULT = ParseOPMULT(parts[0]);
                bool validMULT = ParseMULT(parts[1]);
                bool validTERM1 = ParseTERM1(parts[2]);

                if (validOPMULT && validMULT && validTERM1)
                {
                    lstSyntax.Items.Add("TERM2 успешно распарсено.");
                    return true;
                }
            }

            throw new Exception("Ошибка: Неверный формат TERM2.");
        }

        // Разбор ADD (операции сложения)
        private bool ParseADD(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить ADD...");
            string[] validOps = { "+", "-", "or" };

            if (validOps.Contains(input))
            {
                lstSyntax.Items.Add("ADD успешно распарсено.");
                return true;
            }

            lstSyntax.Items.Add("Ошибка: Некорректный оператор в ADD.");
            return false;
        }

        // Разбор MULT (множитель)
        private bool ParseMULT(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить MULT...");

            if (ParseID(input) || ParseNMBR(input))
            {
                lstSyntax.Items.Add("MULT успешно распарсено.");
                return true;
            }

            // Обрабатываем выражения в скобках
            if (input.StartsWith("(") && input.EndsWith(")"))
            {
                string inner = input.Substring(1, input.Length - 2).Trim();
                if (ParseXPR(inner))
                {
                    lstSyntax.Items.Add("MULT успешно распарсено как выражение в скобках.");
                    return true;
                }
            }

            lstSyntax.Items.Add("Ошибка: MULT не удалось распарсить.");
            return false;
        }

        // Разбор OPMULT (операции умножения)
        private bool ParseOPMULT(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OPMULT...");
            // Проверка на допустимые операции умножения
            if (input == "*" || input == "/" || input == "and")
            {
                lstSyntax.Items.Add("OPMULT успешно распарсено.");
                return true;
            }
            else
            {
                throw new Exception("Ошибка: Неопознанная операция умножения.");
            }
        }

        // Разбор NMBR (число)
        private bool ParseNMBR(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить NMBR...");
            // NMBR = INT | REAL
            if (ParseINT(input) || ParseREAL(input))
            {
                lstSyntax.Items.Add("NMBR успешно распарсено.");
                return true;
            }

            throw new Exception("Ошибка: Неопознанное число.");
        }

        // Разбор LGCONST (логическая константа)
        private bool ParseLGCONST(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить LGCONST...");
            // Проверка на логические константы
            if (input == "true" || input == "false")
            {
                lstSyntax.Items.Add("LGCONST успешно распарсено.");
                return true;
            }

            throw new Exception("Ошибка: Не является логической константой.");
        }

        // Разбор UNARY (унарный оператор)
        private bool ParseUNARY(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить UNARY...");
            // Проверка на унарные операторы
            if (input == "not")
            {
                lstSyntax.Items.Add("UNARY успешно распарсено.");
                return true;
            }

            throw new Exception("Ошибка: Не является унарным оператором.");
        }

        // Разбор INT (целое число)
        private bool ParseINT(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить INT...");
            // INT = BIN | OCT | DEC | HEX
            if (ParseBIN(input) || ParseOCT(input) || ParseDEC(input) || ParseHEX(input))
            {
                lstSyntax.Items.Add("INT успешно распарсено.");
                return true;
            }

            throw new Exception("Ошибка: Неопознанное целое число.");
        }

        // Разбор BIN (двоичное число)
        private bool ParseBIN(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BIN...");
            // BIN = BIN1 BIN4
            if (input.StartsWith("0b") && input.Length > 2)
            {
                string binBody = input.Substring(2);
                if (ParseBIN1(binBody) && ParseBIN4(input.Last().ToString()))
                {
                    lstSyntax.Items.Add("BIN успешно распарсено.");
                    return true;
                }
            }

            return false;
        }

        // Разбор BIN1
        private bool ParseBIN1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BIN1...");
            // BIN1 = 0 BIN2 | 1 BIN2
            if (input[0] == '0' || input[0] == '1')
            {
                return ParseBIN2(input.Substring(1));
            }

            return false;
        }

        // Разбор BIN2
        private bool ParseBIN2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BIN2...");
            // BIN2 = 0 BIN3 | 1 BIN3 | eps
            if (input.Length > 0 && (input[0] == '0' || input[0] == '1'))
            {
                return ParseBIN3(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор BIN3
        private bool ParseBIN3(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BIN3...");
            // BIN3 = 0 BIN2 | 1 BIN2 | eps
            if (input.Length > 0 && (input[0] == '0' || input[0] == '1'))
            {
                return ParseBIN2(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор BIN4
        private bool ParseBIN4(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить BIN4...");
            // BIN4 = ( B | b )
            return input == "b" || input == "B";
        }

        // Разбор OCT (восьмеричное число)
        private bool ParseOCT(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OCT...");
            // OCT = OCT1 OCT4
            if (input.StartsWith("0") && input.Length > 1)
            {
                string octBody = input.Substring(1);
                if (ParseOCT1(octBody) && ParseOCT4(input.Last().ToString()))
                {
                    lstSyntax.Items.Add("OCT успешно распарсено.");
                    return true;
                }
            }

            return false;
        }

        // Разбор OCT1
        private bool ParseOCT1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OCT1...");
            // OCT1 = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7
            return "01234567".Contains(input[0]);
        }

        // Разбор OCT2
        private bool ParseOCT2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OCT2...");
            // OCT2 = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | eps
            if (input.Length > 0 && "01234567".Contains(input[0]))
            {
                return ParseOCT3(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор OCT3
        private bool ParseOCT3(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OCT3...");
            // OCT3 = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7
            return input.Length > 0 && "01234567".Contains(input[0]);
        }

        // Разбор OCT4
        private bool ParseOCT4(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить OCT4...");
            // OCT4 = ( O | o )
            return input == "o" || input == "O";
        }

        // Разбор DEC (десятичное число)
        private bool ParseDEC(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить DEC...");
            // DEC = DEC1 DEC4
            if (input.Length > 0 && Char.IsDigit(input[0]))
            {
                string decBody = input.Substring(1);
                if (ParseDEC1(input[0].ToString()) && ParseDEC4(decBody))
                {
                    lstSyntax.Items.Add("DEC успешно распарсено.");
                    return true;
                }
            }

            return false;
        }

        // Разбор DEC1
        private bool ParseDEC1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить DEC1...");
            // DEC1 = DIGIT DEC2
            return Char.IsDigit(input[0]);
        }

        // Разбор DEC2
        private bool ParseDEC2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить DEC2...");
            // DEC2 = DIGIT DEC3 | eps
            if (input.Length > 0 && Char.IsDigit(input[0]))
            {
                return ParseDEC3(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор DEC3
        private bool ParseDEC3(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить DEC3...");
            // DEC3 = DIGIT DEC2 | eps
            if (input.Length > 0 && Char.IsDigit(input[0]))
            {
                return ParseDEC2(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор DEC4
        private bool ParseDEC4(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить DEC4...");
            // DEC4 = D | d | eps
            return input == "d" || input == "D" || string.IsNullOrEmpty(input);
        }

        // Разбор HEX (шестнадцатеричное число)
        private bool ParseHEX(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить HEX...");
            // HEX = DIGIT HEX1 HEX3
            if (Char.IsDigit(input[0]) || "ABCDEFabcdef".Contains(input[0]))
            {
                string hexBody = input.Substring(1);
                if (ParseHEX1(hexBody))
                {
                    lstSyntax.Items.Add("HEX успешно распарсено.");
                    return true;
                }
            }

            return false;
        }

        // Разбор HEX1
        private bool ParseHEX1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить HEX1...");
            // HEX1 = DIGIT HEX2 | A HEX2 | B HEX2 | C HEX | D HEX1 | E HEX2 | F HEX2 | eps
            return Char.IsDigit(input[0]) || "ABCDEFabcdef".Contains(input[0]);
        }

        // Разбор HEX2
        private bool ParseHEX2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить HEX2...");
            // HEX2 = DIGIT HEX1 | A HEX1 | B HEX1 | C HEX1 | D HEX1 | E HEX1 | F HEX1 | eps
            return Char.IsDigit(input[0]) || "ABCDEFabcdef".Contains(input[0]);
        }

        // Разбор HEX3
        private bool ParseHEX3(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить HEX3...");
            // HEX3 = ( H | h )
            return input == "h" || input == "H";
        }

        // Разбор REAL (вещественное число)
        private bool ParseREAL(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить REAL...");
            // REAL = NUMBSTR ORDER | REAL1.NUMBSTR REAL2
            string[] parts = input.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                bool validREAL1 = ParseREAL1(parts[0]);
                bool validNUMBSTR = ParseNUMBSTR(parts[1]);
                bool validREAL2 = ParseREAL2(parts.Length > 1 ? parts[1] : "");

                if (validREAL1 && validNUMBSTR && validREAL2)
                {
                    lstSyntax.Items.Add("REAL успешно распарсено.");
                    return true;
                }
            }
            else
            {
                // Проверка для вещественного числа в формате NUMBSTR ORDER
                bool validNUMBSTR = ParseNUMBSTR(parts[0]);
                bool validORDER = ParseORDER(parts.Length > 1 ? parts[1] : "");

                if (validNUMBSTR && validORDER)
                {
                    lstSyntax.Items.Add("REAL успешно распарсено.");
                    return true;
                }
            }

            throw new Exception("Ошибка: Неопознанное вещественное число.");
        }

        // Разбор NUMBSTR (строка чисел)
        private bool ParseNUMBSTR(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить NUMBSTR...");
            // NUMBSTR = DIGIT NUMBSTR1
            if (Char.IsDigit(input[0]))
            {
                return ParseNUMBSTR1(input.Substring(1));
            }

            throw new Exception("Ошибка: Не строка чисел.");
        }

        // Разбор NUMBSTR1
        private bool ParseNUMBSTR1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить NUMBSTR1...");
            // NUMBSTR1 = DIGIT NUMBSTR2 | eps
            if (input.Length > 0 && Char.IsDigit(input[0]))
            {
                return ParseNUMBSTR2(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор NUMBSTR2
        private bool ParseNUMBSTR2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить NUMBSTR2...");
            // NUMBSTR2 = DIGIT NUMBSTR1 | eps
            if (input.Length > 0 && Char.IsDigit(input[0]))
            {
                return ParseNUMBSTR1(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор ORDER (порядок для вещественного числа)
        private bool ParseORDER(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить ORDER...");
            // ORDER = ORDER1 ORDER2 NUMBSTR
            if (input.Length > 0)
            {
                if (input[0] == 'e' || input[0] == 'E')
                {
                    return ParseORDER2(input.Substring(1));
                }
            }

            throw new Exception("Ошибка: Не является корректным порядком.");
        }

        // Разбор ORDER2
        private bool ParseORDER2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить ORDER2...");
            // ORDER2 = + | - | eps
            if (input.Length > 0 && (input[0] == '+' || input[0] == '-'))
            {
                return ParseNUMBSTR(input.Substring(1));
            }
            return true; // eps
        }

        // Разбор REAL1 (целая часть вещественного числа)
        private bool ParseREAL1(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить REAL1...");
            // REAL1 = NUMBSTR | eps
            if (ParseNUMBSTR(input))
            {
                return true;
            }

            return false; // eps
        }

        // Разбор REAL2 (порядок вещественного числа, например, "e+2")
        private bool ParseREAL2(string input)
        {
            lstSyntax.Items.Add("Пытаемся распарсить REAL2...");
            // REAL2 = ORDER
            if (ParseORDER(input))
            {
                return true;
            }

            return false; // eps
        }

        // Начало генерации ОПЗ
        // Токенизация (разделение на отдельные элементы)
        private List<string> Tokenize(string line)
        {
            List<string> tokens = new List<string>();
            StringBuilder token = new StringBuilder();

            foreach (char c in line)
            {
                // Если пробел или конец токена, добавляем его в список токенов
                if (char.IsWhiteSpace(c))
                {
                    if (token.Length > 0)
                    {
                        tokens.Add(token.ToString());
                        token.Clear();
                    }
                }
                // Обрабатываем отдельные символы, такие как разделители или операторы
                else if (separators.Contains(c.ToString()) || c == ',' || c == ';' || c == '(' || c == ')')
                {
                    if (token.Length > 0)
                    {
                        tokens.Add(token.ToString());
                        token.Clear();
                    }
                    tokens.Add(c.ToString()); // Добавляем текущий разделитель
                }
                // Обрабатываем символы для ключевых слов и переменных
                else
                {
                    token.Append(c);
                }
            }

            if (token.Length > 0)
            {
                tokens.Add(token.ToString()); // Добавляем последний токен, если он есть
            }

            return tokens;
        }

        // Является ли переменной или константой
        private bool IsVariableOrConstant(string token)
        {
            return lstVariablesMain.Items.Contains(token) || lstConstantsMain.Items.Contains(token);
        }

        // Является ли оператором (разделетели)
        private bool IsOperator(string token)
        {
            return separators.Contains(token) && (token == "+" || token == "-" || token == "*" || token == "/" || token == "=" || token == "<>" || token == "<" || token == ">" || token == "<=" || token == ">=" || token == "or" || token == "and" || token == "not");
        }

        // Определение приоритетов
        private int Precedence(string operatorToken)
        {
            switch (operatorToken)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "=":
                case "<>":
                case "<":
                case ">":
                case "<=":
                case ">=":
                    return 3;
                case "and":
                case "or":
                    return 4;
                case "not":
                    return 5;
                default:
                    return 0;
            }
        }

        // Генерация ОПЗ
        private string GenerateRPN(List<string> tokens, List<string> intermediateSteps)
        {
            Stack<string> operators = new Stack<string>(); // Стек для операторов
            Queue<string> output = new Queue<string>(); // Выходная очередь для ОПЗ
            Stack<string> jumpStack = new Stack<string>(); // Стек для меток переходов
            int labelCount = 0; // Счётчик для уникальных меток

            foreach (string token in tokens)
            {
                if (IsVariableOrConstant(token)) // Если переменная или константа
                {
                    output.Enqueue(token);
                }
                else if (keywords.Contains(token)) // Если ключевое слово
                {
                    switch (token)
                    {
                        case "if":
                            string labelStart = $"M{labelCount++}";
                            string labelEnd = $"M{labelCount++}";

                            jumpStack.Push(labelEnd); // Запоминаем конец условного блока
                            output.Enqueue($"!F {labelStart}"); // Условный переход, если условие ложно
                            output.Enqueue($"{labelStart}:"); // Метка начала выполнения
                            break;

                        case "then":
                            if (jumpStack.Count > 0)
                            {
                                string labelEndThen = jumpStack.Peek();
                                output.Enqueue($"goto {labelEndThen}");
                            }
                            else
                            {
                                throw new InvalidOperationException("Ошибка: стек переходов пуст при обработке 'then'");
                            }
                            break;

                        case "else":
                            if (jumpStack.Count > 0)
                            {
                                string labelEndIf = jumpStack.Pop();
                                string labelElse = $"M{labelCount++}";

                                output.Enqueue($"goto {labelEndIf}"); // Переход после выполнения else
                                output.Enqueue($"{labelElse}:");

                                jumpStack.Push(labelElse); // Запоминаем метку начала else
                            }
                            else
                            {
                                throw new InvalidOperationException("Ошибка: стек переходов пуст при обработке 'else'");
                            }
                            break;

                        case "while":
                            string labelLoopStart = $"M{labelCount++}";
                            string labelLoopEnd = $"M{labelCount++}";

                            output.Enqueue($"{labelLoopStart}:"); // Метка начала цикла
                            jumpStack.Push(labelLoopEnd); // Метка конца цикла
                            jumpStack.Push(labelLoopStart); // Метка начала для повторения
                            break;

                        case "do":
                            if (jumpStack.Count > 0)
                            {
                                string labelWhileEnd = jumpStack.Pop();
                                string labelWhileStart = jumpStack.Pop();

                                output.Enqueue($"goto {labelWhileStart}"); // Переход в начало цикла
                                output.Enqueue($"{labelWhileEnd}:"); // Метка конца цикла
                            }
                            else
                            {
                                throw new InvalidOperationException("Ошибка: стек переходов пуст при обработке 'do'");
                            }
                            break;

                        case "for":
                            string labelForStart = $"M{labelCount++}";
                            string labelForEnd = $"M{labelCount++}";

                            output.Enqueue($"{labelForStart}:"); // Метка начала цикла
                            jumpStack.Push(labelForEnd); // Метка конца цикла
                            jumpStack.Push(labelForStart); // Метка начала цикла
                            break;

                        case "to":
                            if (jumpStack.Count > 0)
                            {
                                string labelForCondition = jumpStack.Peek();
                                output.Enqueue($"!F {labelForCondition}"); // Условие выхода из цикла
                            }
                            else
                            {
                                throw new InvalidOperationException("Ошибка: стек переходов пуст при обработке 'to'");
                            }
                            break;

                        default:
                            output.Enqueue(token); // Другие ключевые слова обрабатываются как есть
                            break;
                    }
                }
                else if (IsOperator(token)) // Если оператор
                {
                    while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(token))
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(token);
                }
                else if (token == "(") // Левая скобка
                {
                    operators.Push(token);
                }
                else if (token == ")") // Правая скобка
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Enqueue(operators.Pop());
                    }
                    if (operators.Count > 0 && operators.Peek() == "(")
                    {
                        operators.Pop(); // Удаляем "("
                    }
                    else
                    {
                        throw new InvalidOperationException("Ошибка: несоответствие скобок");
                    }
                }
                else if (token == "ass") // Присваивание
                {
                    operators.Push(token);
                }

                // Добавляем промежуточный шаг в список для отладки
                intermediateSteps.Add($"Token: {token}, Output: {string.Join(" ", output)}, Stack: {string.Join(" ", operators)}");
            }

            // Перемещаем оставшиеся операторы из стека в выходную очередь
            while (operators.Count > 0)
            {
                output.Enqueue(operators.Pop());
            }

            return string.Join(" ", output); // Возвращаем строку ОПЗ
        }
    }
}

// Класс для семантического анализа
public class SemanticAnalyzer
{
    private HashSet<string> declaredVariables = new HashSet<string>(); // Множество для объявленных переменных
    private Dictionary<string, string> variableTypes = new Dictionary<string, string>(); // Типы переменных
    private List<string> keywords; // Список ключевых слов
    private List<string> separators; // Список разделителей
    private List<string> variables; // Список переменных
    private List<string> constants; // Список констант

    public SemanticAnalyzer(List<string> keywords, List<string> separators, List<string> variables, List<string> constants)
    {
        this.keywords = keywords;
        this.separators = separators;
        this.variables = variables;
        this.constants = constants;
    }

    public void Analyze(Block block, List<string> semantics, List<string> results)
    {
        bool hasError = false;

        // Разделяем строку на части по символу ";"
        var lines = block.Line.Split(';')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToList();

        foreach (var line in lines)
        {
            // Правило 1: Проверка на ключевые слова и объявление переменных
            var varMatch = Regex.Match(line, @"(int|float|bool)\s+([a-zA-Z_][a-zA-Z0-9_]*(\s*,\s*[a-zA-Z_][a-zA-Z0-9_]*)*)\s*;?");

            if (varMatch.Success)
            {
                string type = varMatch.Groups[1].Value;
                string variablesDeclaration = varMatch.Groups[2].Value;

                // Разделяем список переменных по запятой
                var variablesList = variablesDeclaration.Split(',')
                    .Select(v => v.Trim())
                    .ToList();

                foreach (var variable in variablesList)
                {
                    if (keywords.Contains(variable))
                    {
                        semantics.Add($"Ошибка: '{variable}' является ключевым словом, его нельзя использовать как переменную.");
                        results.Add($"Ошибка: '{variable}' является ключевым словом, его нельзя использовать как переменную.");
                        hasError = true;
                    }
                    else if (declaredVariables.Contains(variable))
                    {
                        semantics.Add($"Ошибка: переменная '{variable}' уже была объявлена.");
                        results.Add($"Ошибка: переменная '{variable}' уже была объявлена.");
                        hasError = true;
                    }
                    else
                    {
                        declaredVariables.Add(variable);
                        variableTypes[variable] = type;
                        semantics.Add($"Объявлена переменная '{variable}' типа '{type}'");
                    }
                }
            }
            else
            {
                // Проверка на использование переменной без объявления
                var usedVars = Regex.Matches(line, @"[a-zA-Z_][a-zA-Z0-9_]*").Cast<Match>()
                    .Where(m => !declaredVariables.Contains(m.Value) && !keywords.Contains(m.Value)).ToList();

                foreach (var match in usedVars)
                {
                    semantics.Add($"Ошибка: переменная '{match.Value}' не была объявлена.");
                    results.Add($"Ошибка: переменная '{match.Value}' не была объявлена.");
                    hasError = true;
                }
            }

            // Проверка на взаимодействие переменных разных типов
            var varsInLine = Regex.Matches(line, @"[a-zA-Z_][a-zA-Z0-9_]*").Cast<Match>()
                .Select(m => m.Value)
                .Where(v => declaredVariables.Contains(v))
                .ToList();

            if (varsInLine.Count > 1)
            {
                var typesInLine = varsInLine.Select(v => variableTypes[v]).Distinct().ToList();
                if (typesInLine.Count > 1)
                {
                    string conflictingTypes = string.Join(", ", typesInLine);
                    semantics.Add($"Ошибка: взаимодействие переменных разных типов запрещено. Найдены типы: {conflictingTypes}.");
                    results.Add($"Ошибка: взаимодействие переменных разных типов запрещено. Найдены типы: {conflictingTypes}.");
                    hasError = true;
                }
            }

            // Новое правило: проверка количества операторов сравнения
            var comparisonOperators = Regex.Matches(line, @"(<=|>=|<>|<|>|=)")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            if (comparisonOperators.Count > 1)
            {
                string operators = string.Join(", ", comparisonOperators);
                semantics.Add($"Ошибка: в строке найдено несколько операторов сравнения: {operators}. Разрешен только один оператор сравнения на строку.");
                results.Add($"Ошибка: в строке найдено несколько операторов сравнения: {operators}. Разрешен только один оператор сравнения на строку.");
                hasError = true;
            }

            // Правило 2: Проверка на использование переменной
            var assignmentMatch = Regex.Match(line, @"([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(.*);?");
            if (assignmentMatch.Success)
            {
                string variable = assignmentMatch.Groups[1].Value;
                string expression = assignmentMatch.Groups[2].Value;

                if (!declaredVariables.Contains(variable))
                {
                    semantics.Add($"Ошибка: переменная '{variable}' не была объявлена до использования.");
                    results.Add($"Ошибка: переменная '{variable}' не была объявлена до использования.");
                    hasError = true;
                }
                else
                {
                    // Проверка на типы
                    if (expression.Contains(variable))
                    {
                        string varType = variableTypes[variable];
                        if (expression.Contains("+") || expression.Contains("-"))
                        {
                            if (varType != "int")
                            {
                                semantics.Add($"Ошибка: операция с переменной '{variable}' несовместима с типом '{varType}'.");
                                results.Add($"Ошибка: операция с переменной '{variable}' несовместима с типом '{varType}'.");
                                hasError = true;
                            }
                        }
                    }
                }
            }
        }
    }


}


// Класс для представления блока программы
public class Block
{
    public string Line { get; set; }

    public Block(string line)
    {
        Line = line;
    }
}

