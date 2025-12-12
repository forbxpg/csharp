/*
 * М25 - 514 Киргизов Темурмалик Кутбиддин угли.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csWinFkey
{
    public partial class Form1 : Form
    {
        private int spaceCount = 0;
        private bool isTemplateSet = false;
        private string inputTemplate = "";
        private bool isAutoFormatting = false;

        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Button btnClear;
        private ListBox listResults;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.listResults = new System.Windows.Forms.ListBox();
            this.SuspendLayout();

            // textBox1
            this.textBox1.Location = new System.Drawing.Point(12, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(300, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);

            // label1
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Автоматизированный ввод ФИО с шаблоном";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(350, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Введите ФИО (например: новиков-прибой ас) - пробел завершает ввод";

            // btnClear
            this.btnClear.Location = new System.Drawing.Point(318, 48);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Очистить";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // listResults
            this.listResults.FormattingEnabled = true;
            this.listResults.Location = new System.Drawing.Point(12, 80);
            this.listResults.Name = "listResults";
            this.listResults.Size = new System.Drawing.Size(381, 160);
            this.listResults.TabIndex = 4;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 250);
            this.Controls.Add(this.listResults);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Лабораторная работа №1 - Ввод ФИО";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Обработчик нажатия клавиш - валидация и автоматическое форматирование
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем управляющие клавиши (Backspace, Delete, стрелки и т.д.)
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Разрешаем только буквы, пробел и дефис
            if (Char.IsLetter(e.KeyChar) || (e.KeyChar == ' ') || (e.KeyChar == '-'))
            {
                int cursorPosition = textBox1.SelectionStart;
                string currentText = textBox1.Text;

                // Обработка первого символа в строке
                if (cursorPosition == 0 && currentText.Length == 0)
                {
                    // Запрещаем пробел или дефис в начале
                    if (e.KeyChar == ' ' || e.KeyChar == '-')
                    {
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        // Первую букву делаем заглавной
                        e.KeyChar = Char.ToUpper(e.KeyChar);
                    }
                }
                // Обработка пробела (разделитель фамилии и инициалов)
                else if (e.KeyChar == ' ')
                {
                    if (spaceCount == 0)
                    {
                        spaceCount = 1; // Отмечаем конец фамилии

                        // Если это первая строка - сохраняем как шаблон
                        if (!isTemplateSet)
                        {
                            inputTemplate = textBox1.Text + " ";
                            isTemplateSet = true;
                            label2.Text = "Шаблон установлен. Вводите следующие ФИО...";
                        }
                    }
                    else
                    {
                        // Запрещаем лишние пробелы
                        e.Handled = true;
                    }
                }
                // Обработка дефиса в фамилии
                else if (e.KeyChar == '-' && spaceCount == 0)
                {
                    // Разрешаем дефис только в фамилии и только после буквы
                    if (cursorPosition > 0 && Char.IsLetter(currentText[cursorPosition - 1]))
                    {
                        // Дефис разрешен
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                // Обработка инициалов (после пробела)
                else if (spaceCount > 0)
                {
                    // Инициалы делаем заглавными
                    e.KeyChar = Char.ToUpper(e.KeyChar);

                    // Увеличиваем счетчик только для букв (не для дефиса и т.д.)
                    if (Char.IsLetter(e.KeyChar))
                    {
                        spaceCount++;
                    }
                }
                // Обработка букв в фамилии (не первая буква)
                else if (spaceCount == 0)
                {
                    // Проверяем, стоит ли перед текущей позицией дефис
                    if (cursorPosition > 0 && currentText.Length >= cursorPosition &&
                        currentText[cursorPosition - 1] == '-')
                    {
                        // Буква после дефиса - делаем заглавной
                        e.KeyChar = Char.ToUpper(e.KeyChar);
                    }
                    else if (cursorPosition > 0)
                    {
                        // Обычная буква в фамилии - делаем строчной
                        e.KeyChar = Char.ToLower(e.KeyChar);
                    }
                }
            }
            else
            {
                // Запрещаем все остальные символы
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик отпускания клавиши - автоматическая расстановка точек
        /// </summary>
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // Пропускаем управляющие клавиши
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete ||
                e.KeyCode == Keys.Left || e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                return;
            }

            // Добавляем точку после первого инициала
            if (spaceCount == 2 && !isAutoFormatting)
            {
                isAutoFormatting = true;
                int cursorPos = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text + ".";
                textBox1.SelectionStart = cursorPos + 1;
                isAutoFormatting = false;
            }

            // Добавляем точку после второго инициала
            if (spaceCount == 3 && !isAutoFormatting)
            {
                isAutoFormatting = true;
                int cursorPos = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text + ".";
                textBox1.SelectionStart = cursorPos + 1;

                // Выделяем весь текст для удобства редактирования
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = textBox1.Text.Length;

                isAutoFormatting = false;
            }

            // Если введено больше 3 символов после пробела, сбрасываем счетчик
            if (spaceCount > 3)
            {
                spaceCount = 0;
            }
        }

        /// <summary>
        /// Обработчик кнопки очистки
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            spaceCount = 0;
            textBox1.Focus();
        }

        /// <summary>
        /// Обработчик нажатия Enter для завершения ввода
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Enter && textBox1.Focused)
            {
                // Если ввод завершен (есть два инициала с точками)
                if (textBox1.Text.Contains(".") && textBox1.Text.Length > 5)
                {
                    // Добавляем в список результатов
                    listResults.Items.Add(textBox1.Text);

                    // Выводим в консоль
                    Console.WriteLine("Введено ФИО: " + textBox1.Text);

                    // Очищаем поле для следующего ввода
                    textBox1.Clear();
                    spaceCount = 0;

                    // Если установлен шаблон, подставляем его
                    if (isTemplateSet && !string.IsNullOrEmpty(inputTemplate))
                    {
                        // Находим позицию первого пробела в шаблоне
                        int spaceIndex = inputTemplate.IndexOf(' ');
                        if (spaceIndex > 0)
                        {
                            // Вставляем фамилию из шаблона
                            textBox1.Text = inputTemplate.Substring(0, spaceIndex + 1);
                            textBox1.SelectionStart = textBox1.Text.Length;
                        }
                    }
                }
                e.Handled = true;
            }
        }
    }
}