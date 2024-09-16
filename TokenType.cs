namespace Compiler;

public class TokenType
{
    // Свойство для регулярного выражения
    public string Regex { get; private set; }

    public static TokenType Space = new (@"\s");
    
    // Лексема для чисел (регулярное выражение для целых чисел)
    public static TokenType Number = new (@"\d+");

    // Лексема для операторов (регулярное выражение для операторов +, -, =, ?)
    public static TokenType Operator = new (@"[\=\+\?\-]");

    // Конструктор для инициализации регулярного выражения
    public TokenType(string regex)
    {
        Regex = regex;
    }
}
