namespace Compiler;

// Основной базовый класс для всех синтаксических узлов
public abstract class Syntax
{

}

// Класс для инструкций, состоящих из выражения, знака равно и знака вопроса
public class Instruction : Syntax
{
    public Expression Expression { get; private set; }
    public EqualsOperator EqualsOperator { get; private set; }
    public QuestionMarkOperator QuestionMarkOperator { get; private set; }

    public Instruction(Expression expression, EqualsOperator equalsOperator, QuestionMarkOperator questionMarkOperator)
    {
        Expression = expression;
        EqualsOperator = equalsOperator;
        QuestionMarkOperator = questionMarkOperator;
    }

    public override string ToString()
    {
        return $"{Expression} {EqualsOperator} {QuestionMarkOperator}";
    }

    public static Instruction BuildSyntaxTree(string[] lexemes, ref int index)
    {
        Console.WriteLine("Building syntax tree for instruction...");

        if (index != 0)
        {
            throw new ArgumentException("Instruction must start from 0 position.");
        }

        if (lexemes.Length < 3)
        {
            throw new ArgumentException("Insufficient number of lexemes to build instruction.");
        }

        // Построение выражения
        Console.WriteLine("Parsing expression...");
        var expr = Expression.BuildSyntaxTree(lexemes, ref index);
        Console.WriteLine($"Parsed expression: {expr}");

        if (lexemes.Length <= index || lexemes[index] != "=")
        {
            throw new FormatException("Expected '=' after expression.");
        }

        var op = new EqualsOperator();
        index++;
        Console.WriteLine("Parsed equals operator: =");

        if (lexemes.Length <= index || lexemes[index] != "?")
        {
            throw new FormatException("Expected '?' after '='.");
        }

        var qmop = new QuestionMarkOperator();
        index++;
        Console.WriteLine("Parsed question mark operator: ?");

        if (index != lexemes.Length)
        {
            throw new FormatException("Extra lexemes found after '?' operator.");
        }

        Console.WriteLine("Instruction successfully parsed.");
        return new Instruction(expr, op, qmop);
    }
}

// Класс для оператора вопроса
public class QuestionMarkOperator : Syntax
{
    public char Operator { get; } = '?';

    public override string ToString()
    {
        return $"{Operator}";
    }
}

// Класс для оператора равно
public class EqualsOperator : Syntax
{
    public char Operator { get; } = '=';

    public override string ToString()
    {
        return $"{Operator}";
    }
}

// Класс для выражений, состоящих из терма, арифметического оператора и вложенного выражения
public class Expression : Syntax
{
    public Term Term { get; private set; }
    public ArithmeticOperator? ArithmeticOperator { get; private set; }
    public Expression? SubExpression { get; private set; }

    public Expression(Term term, ArithmeticOperator? arithmeticOperator = null, Expression? subExpression = null)
    {
        Term = term;
        ArithmeticOperator = arithmeticOperator;
        SubExpression = subExpression;
    }

    public override string ToString()
    {
        return SubExpression == null
            ? Term.ToString()
            : $"{Term} {ArithmeticOperator} {SubExpression}";
    }

    public static Expression BuildSyntaxTree(string[] lexemes, ref int index)
    {
        Console.WriteLine($"Building syntax tree for expression starting at index {index}...");

        if (index >= lexemes.Length)
        {
            throw new ArgumentException("Index is out of range.");
        }

        // Пытаемся распарсить число
        if (!int.TryParse(lexemes[index], out int number))
        {
            throw new FormatException($"Expected number but found {lexemes[index]}.");
        }

        var term = new Term(number);
        Console.WriteLine($"Parsed term: {term}");
        index++;

        // Проверяем, есть ли еще лексемы
        if (index >= lexemes.Length)
        {
            // Нет больше лексем, возвращаем Expression с только термом
            Console.WriteLine("No more lexemes found, returning simple expression.");
            return new Expression(term);
        }

        // Проверяем, является ли следующая лексема оператором
        bool isOp = lexemes[index] switch
        {
            "*" or "/" or "+" or "-" => true,
            _ => false
        };

        if (!isOp)
        {
            // Если оператор отсутствует, возвращаем Expression с только термом
            Console.WriteLine("No operator found, returning expression with only term.");
            return new Expression(term);
        }

        var op = new ArithmeticOperator(lexemes[index][0]); // Предполагается, что лексема состоит из одного символа
        Console.WriteLine($"Parsed arithmetic operator: {op}");
        index++;

        // Рекурсивно строим подвыражение
        var subExpression = BuildSyntaxTree(lexemes, ref index);
        Console.WriteLine($"Parsed subexpression: {subExpression}");

        return new Expression(term, op, subExpression);
    }
}

// Класс для термов, представляющих числа
public class Term : Syntax
{
    public int Number { get; private set; }

    public Term(int number)
    {
        Number = number;
    }

    public override string ToString()
    {
        return Number.ToString();
    }
}

// Класс для арифметических операторов
public class ArithmeticOperator : Syntax
{
    public char Operator { get; private set; }

    public ArithmeticOperator(char op)
    {
        Operator = op;
    }

    public override string ToString()
    {
        return Operator.ToString();
    }
}
