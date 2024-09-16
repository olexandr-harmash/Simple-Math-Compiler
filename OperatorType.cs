using System.Security.Cryptography;

namespace Compiler;

public class OperatorType
{
    // Свойство для оператора
    public string Operator { get; private set; }

    // Статические свойства для различных операторов
    public static OperatorType Plus = new ("+");
    public static OperatorType Minus = new ("-");
    public static OperatorType QuestionMark = new ("?");
    public static OperatorType Equals = new ("=");

    // Конструктор для инициализации оператора
    private OperatorType(string op)
    {
        Operator = op;
    }

    // Переопределение метода ToString() для удобного отображения
    public override string ToString()
    {
        return Operator;
    }

    public static OperatorType? GetFromString(string op)
    {
        return op switch
        {
            "+" => Plus,
            "-" => Minus,
            "=" => Equals,
            "?" => QuestionMark,
            _ => null
        };
    }
}
