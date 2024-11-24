using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
public partial class Date
{
    private int day;
    private int month;
    private int year;
    public readonly int ID;
    private static int objectCounter;
    private const int minYear = 1900;
    public static void PrintClassInfo()
    {
        Console.WriteLine($"Object count: {objectCounter}");
    }
    private string GetMonthName()
    {
        return new DateTime(year, month, day).ToString("MMMM");
    }
    static Date()
    {
        objectCounter = 0;
    }
    private Date()
    {
        ID = GetHashCode();
    }
    public Date(int day, int month, int year)
    {
        this.day = day;
        this.month = month;
        this.year = year;
        objectCounter++;
    }
    public Date(int day = 1, int month = 12) : this(day, month, minYear) { }
    public int Day
    {
        get { return day; }
        set
        {
            if (value < 1 || value > DateTime.DaysInMonth(year, month))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Неверно указан день.");
            }
            day = value;
        }
    }
    public int Month
    {
        get { return month; }
        set
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Неверно указан месяц.");
            }
            month = value;
        }
    }
    public int Year
    {
        get { return year; }
        set
        {
            if (year < minYear || year > 2024)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Неверно указан год.");
            }
            year = value;
        }
    }
    public void PrintDate()
    {
        Console.WriteLine($"{day}/{month}/{year}");
        Console.WriteLine($"{day} {GetMonthName()} {year} года");
    }

    public void UpdateDate(ref int newDay, ref int newMonth, out int updatedYear)
    {
        if (newDay < 1 || newDay > DateTime.DaysInMonth(year, newMonth))
        {
            throw new ArgumentOutOfRangeException(nameof(newDay), "Неверно указан день.");
        }

        if (newMonth < 1 || newMonth > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(newMonth), "Неверно указан месяц.");
        }

        day = newDay;
        month = newMonth;

        updatedYear = year;
    }

    public override bool Equals(object obj)
    {
        return obj is Date other && day == other.day && month == other.month && year == other.year;
    }
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + day.GetHashCode();
        hash = hash * 23 + month.GetHashCode();
        return hash;
    }

    public override string ToString()
    {
        return $"Date [day: {day}, month: {month}, year: {year}";
    }
}

public class StringMethods
{
    public static string RemovePunctuation(string input)
    {
        var result = new StringBuilder();
        foreach (char c in input)
        {
            if (!char.IsPunctuation(c))
            {
                result.Append(c);
            }

        }
        return result.ToString();
    }
    public static string AddSymbol(string input, char symbol)
    {
        return input + symbol;
    }
    public static string ToUpperCase(string input)
    {
        return input.ToUpper();
    }
    public static string RemoveExtraSpaces(string input)
    {
        return input.Replace(" ", "");

    }
    public static string ReplaceSpacesWithUnderscores(string input)
    {
        return input.Replace(' ', '_');
    }
}

public static class Reflection
{
    public static string GetAssemblyName(string className)
    {
        Type type = Type.GetType(className);

        return type.Assembly.GetName().Name;
    }
    public static bool HasPublicConstructors(string className)
    {
        Type type = Type.GetType(className);

        var constructors = type?.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        return constructors.Length > 0;
    }
    public static IEnumerable<string> GetPublicMethods(string className)
    {
        Type type = Type.GetType(className);

        return type?.GetMethods(BindingFlags.Public | BindingFlags.Instance).Select(method => method.Name);
    }
    public static IEnumerable<string> GetFieldsAndProperties(string className)
    {
        Type type = Type.GetType(className);

        var fields = type?.GetFields(BindingFlags.Public | BindingFlags.Instance).Select(field => field.Name);

        var properties = type?.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(prop => prop.Name);

        return fields?.Concat(properties);
    }
    public static IEnumerable<string> GetImplementedInterfaces(string className)
    {
        Type type = Type.GetType(className);

        return type?.GetInterfaces().Select(i => i.Name);
    }
    public static IEnumerable<string> GetMethodsByClassName(string className, string parameterTypeName)
    {
        Type type = Type.GetType(className);

        Type parameterType = Type.GetType(parameterTypeName);

        return type?.GetMethods()
                   .Where(method => method.GetParameters().Any(parameter => parameter.ParameterType == parameterType))
                   .Select(method => method.Name);

    }
    public static void InvokeMethod(object obj, string methodName, object[] parameters)
    {
        MethodInfo method = obj.GetType().GetMethod(methodName);
        method?.Invoke(obj, parameters);
    }
    
    public static void SaveToFile(string className, string path)
    {
        string filePath = $"C:\\Users\\vlad\\source\\repos\\OOP11\\{path}";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"Информация о классе: {className}");
            writer.WriteLine($"Имя сборки: {GetAssemblyName(className)}");
            writer.WriteLine($"Есть публичные конструкторы: {HasPublicConstructors(className)}");

            writer.WriteLine("Публичные методы:");
            foreach (var method in GetPublicMethods(className))
            {
                writer.WriteLine($"- {method}");
            }

            writer.WriteLine("Поля и свойства:");
            foreach (var fieldOrProperty in GetFieldsAndProperties(className))
            {
                writer.WriteLine($"- {fieldOrProperty}");
            }

            writer.WriteLine("Реализованные интерфейсы:");
            foreach (var interfaceName in GetImplementedInterfaces(className))
            {
                writer.WriteLine($"- {interfaceName}");
            }
        }
    }
    public static T Create<T>(params object[] args)
    {
        return (T)Activator.CreateInstance(typeof(T), args);
    }
}

namespace OOP11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Reflection.SaveToFile("Date", "DateClassInfo.txt");

            Reflection.SaveToFile("StringMethods", "StringMethodsClassInfo.txt");

            Reflection.SaveToFile("System.String", "StringClassInfo.txt");

            Reflection.SaveToFile("System.Object", "ObjectClassInfo.txt");

            var createResult = Reflection.Create<Date>(1, 4, 2006);

            Console.WriteLine($"Class 'Date' info: day {createResult.Day} month {createResult.Month} year {createResult.Year}");


        }
    }
}
