using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCalculator
{
    public partial class MainWindow : Window
    {
        // Переменные для хранения состояния калькулятора
        private double _firstNumber;           // Первое число
        private string _currentOperation;      // Текущая операция
        private bool _isNewEntry;              // Флаг: начинаем вводить новое число?

        public MainWindow()
        {
            InitializeComponent();
            _isNewEntry = true; // Начинаем с чистого листа
        }

        // Обработчик кнопок с цифрами (0-9)
        private void BtnDigit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string digit)
            {
                // Если это новый ввод или после операции — заменяем "0"
                if (_isNewEntry || Output.Text == "0")
                {
                    Output.Text = digit;
                    _isNewEntry = false;
                }
                else
                {
                    // Иначе добавляем цифру к текущему числу
                    Output.Text += digit;
                }
            }
        }

        // Обработчик десятичной точки
        private void BtnDecimal_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем точку, только если её ещё нет в числе
            if (!Output.Text.Contains("."))
            {
                if (_isNewEntry)
                {
                    Output.Text = "0.";
                    _isNewEntry = false;
                }
                else
                {
                    Output.Text += ".";
                }
            }
        }

        // Обработчик операций (+, -, *, /)
        private void BtnOperation_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string operation)
            {
                // Если уже есть операция и введено второе число — считаем сразу
                if (!string.IsNullOrEmpty(_currentOperation) && !_isNewEntry)
                {
                    CalculateResult();
                }

                // Сохраняем первое число и выбранную операцию
                _firstNumber = double.Parse(Output.Text);
                _currentOperation = operation;

                // Показываем выражение в верхнем поле
                Input.Text = $"{_firstNumber} {_currentOperation}";

                // Готовимся ко вводу второго числа
                _isNewEntry = true;
            }
        }

        // Вычисление результата
        private void CalculateResult()
        {
            if (string.IsNullOrEmpty(_currentOperation)) return;

            double secondNumber = double.Parse(Output.Text);
            double result = 0;

            try
            {
                switch (_currentOperation)
                {
                    case "+":
                        result = _firstNumber + secondNumber;
                        break;
                    case "-":
                        result = _firstNumber - secondNumber;
                        break;
                    case "*":
                        result = _firstNumber * secondNumber;
                        break;
                    case "/":
                        if (secondNumber == 0)
                        {
                            MessageBox.Show("На ноль делить нельзя!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            ClearAll();
                            return;
                        }
                        result = _firstNumber / secondNumber;
                        break;
                }

                // Показываем результат
                Output.Text = result.ToString("G10"); // G10 — убирает лишние нули
                Input.Text = ""; // Очищаем выражение
                _currentOperation = null;
                _isNewEntry = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка вычисления",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAll();
            }
        }

        // Обработчик кнопки "Равно"
        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentOperation) && !_isNewEntry)
            {
                CalculateResult();
            }
        }

        // Обработчик кнопки "Сброс" (C)
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        // Метод полной очистки
        private void ClearAll()
        {
            Input.Text = "";
            Output.Text = "0";
            _firstNumber = 0;
            _currentOperation = null;
            _isNewEntry = true;
        }

        // Обработчик кнопки "Удалить символ" (Backspace)
        private void BtnBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (!_isNewEntry && Output.Text.Length > 1)
            {
                // Удаляем последний символ
                Output.Text = Output.Text.Substring(0, Output.Text.Length - 1);
            }
            else
            {
                // Если одна цифра — возвращаем "0"
                Output.Text = "0";
                _isNewEntry = true;
            }
        }

        // Обработчик кнопки "Сменить знак"
        private void BtnToggleSign_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Output.Text, out double number))
            {
                number = -number; // Меняем знак
                Output.Text = number.ToString("G10");
            }
        }
    }
}