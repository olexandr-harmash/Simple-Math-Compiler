using System.Security.Cryptography;

namespace Compiler;

public class OperatorType
{
    // Свойство для оператора
    public string Operator { get; private set; }

    // Статические свойства для различных операторов
    public static OperatorType Plus = new ("+");
    public static OperatorType Minus = new ("-");
    public static OperatorType Multiply = new ("*");
    public static OperatorType Divide = new ("/");

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
}
