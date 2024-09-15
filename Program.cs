/*
    Программа создана для компиляции элементарных математических выражений.

    Грамматика языка описывается следующим образом:
        "
            <инструкция> ::= <выражение> <присваивание> <результат>
            <выражение> ::= <терм> 
                        | <терм> <оператор> <выражение>
            <терм> ::= <число>
            <оператор> ::= "+" | "-" | "*" | "/"
            <присваивание> ::= "="
            <результат> ::= "?"
            <число> ::= [0-9]+
        ".

    Программа:
    - Разбивает входящую строку на лексемы.
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
            Console.WriteLine($"Equals Operator: {instr.EqualsOperator.Operator}");
            Console.WriteLine($"Expression Term: {instr.Expression.Term.Number}");
            Console.WriteLine($"Instruction: {instr}");

            // Логируем все детали выражения
            Console.WriteLine("\nDetailed Log:");
            Console.WriteLine($"Instruction ToString: {instr}");
            Console.WriteLine($"Expression ToString: {instr.Expression}");
            Console.WriteLine($"EqualsOperator: {instr.EqualsOperator}");
            Console.WriteLine($"QuestionMarkOperator: {instr.QuestionMarkOperator}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}