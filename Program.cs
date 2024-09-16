/*
    Программа создана для компиляции элементарных математических выражений.

    Грамматика языка описывается следующим образом:
        "
            <инструкция> ::= <выражение> <присваивание> <результат>
            <выражение> ::= <терм> 
                        | <терм> <оператор> <выражение>
            <терм> ::= <число>
            <оператор> ::= "+" | "-"
            <присваивание> ::= "="
            <результат> ::= "?"
            <число> ::= [0-9]+
        ".

    Программа:
    - Выполняет разбор цепочки символов.
    - Разбивает входящую строку на лексемы.
    - Строит синтаксическое дерево.
    
    Минусы:
    - Данная грамматика не позволяет учитывать приоритеты для операций *, /.
    - Данное дерево строиться таким образом что обход слева-направо выполняет мат выражение наоборот.

    *Для правильного решения задачи парсинга мат выражений с учетом приоритетов,
     правильно использовать грамматику вида:
        "
            E -> T | E + T | E - T
            T -> M | T * M | T / M
            M -> a | b | c (E)
        ".
    Смотреть: "Конструирование компиляторов, Сергей Свердлов, 37 ст.".
*/

using System.Text.RegularExpressions;

namespace Compiler;

class Program
{
    public static void Main(string[] args)
    {
        // Проверьте наличие аргументов
        if (args.Length == 0)
        {
            Console.WriteLine("No input provided.");
            return;
        }

        string expr = args[0];
        Console.WriteLine($"Input expression: {expr}");

        // Используйте Regex.Split для разделения на лексемы
        string[] lexemes = Regex.Split(expr, TokenType.Space.Regex);

        // Печать количества лексем
        Console.WriteLine($"Number of lexemes: {lexemes.Length}");
        
        foreach (var lexeme in lexemes)
        {
            string res = lexeme switch
            {
                _ when Regex.IsMatch(lexeme, TokenType.Number.Regex) => $"Lexeme {lexeme} is Number.",
                _ when Regex.IsMatch(lexeme, TokenType.Operator.Regex) => $"Lexeme {lexeme} is Operator.",
                _ => throw new Exception("Unknown lexem.")
            };

            Console.WriteLine(res);
        }

        try
        {
            int index = 0;

            // Создаем синтаксическое дерево
            var instr = Instruction.BuildSyntaxTree(lexemes, ref index);

            // Логируем результаты
            Console.WriteLine("Parsed Instruction:");
            Console.WriteLine($"Equals Operator: {instr.EqualsOperator}");
            Console.WriteLine($"Expression Term: {instr.Expression.Term.Number}");
            Console.WriteLine($"Instruction: {instr}");

            // Логируем все детали выражения
            Console.WriteLine("\nDetailed Log:");
            Console.WriteLine($"Instruction ToString: {instr}");
            Console.WriteLine($"Expression ToString: {instr.Expression}");
            Console.WriteLine($"EqualsOperator: {instr.EqualsOperator}");
            Console.WriteLine($"QuestionMarkOperator: {instr.QuestionMarkOperator}");

            Console.WriteLine("\nSyntax Tree:");
            Console.WriteLine(FormatSyntaxTree(instr.Expression, 0));

            instr.Compile();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static string FormatSyntaxTree(Expression expression, int level)
    {
        if (expression == null)
            return string.Empty;

        var indent = new string(' ', level * 4);
        var result = $"{indent}Term: {expression.Term.Number}, Operator: {expression.ArithmeticOperator}\n";

        if (expression.SubExpression != null)
        {
            result += $"{FormatSyntaxTree(expression.SubExpression, level + 1)}";
        }

        return result;
    }
}