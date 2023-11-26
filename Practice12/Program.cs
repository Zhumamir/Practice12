using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice12
{
    public delegate void PropertyEventHandler(object sender, PropertyEventArgs e);

    public class PropertyEventArgs : EventArgs
    {
        public string PropertyName { get; }

        public PropertyEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    public interface IPropertyChanged
    {
        event PropertyEventHandler PropertyChanged;
    }

    public class ObservableClass : IPropertyChanged
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChange(nameof(Name));
                }
            }
        }

        public event PropertyEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyEventArgs(propertyName));
        }
    }

    public delegate double MathOperation(double x, double y);

    public class MathOperations
    {
        public static double Add(double x, double y) => x + y;
        public static double Subtract(double x, double y) => x - y;
        public static double Multiply(double x, double y) => x * y;
        public static double Divide(double x, double y) => y != 0 ? x / y : double.NaN;
    }
    public class Program
    {
        public static void Main()
        {
            ObservableClass observable = new ObservableClass();
            observable.PropertyChanged += OnPropertyChanged;

            observable.Name = "John";
            observable.Name = "Jane";

            MathOperation addOperation = MathOperations.Add;
            MathOperation subtractOperation = MathOperations.Subtract;
            MathOperation multiplyOperation = MathOperations.Multiply;
            MathOperation divideOperation = MathOperations.Divide;

            double result1 = PerformOperation(5, 3, addOperation);
            double result2 = PerformOperation(5, 3, subtractOperation);
            double result3 = PerformOperation(5, 3, multiplyOperation);
            double result4 = PerformOperation(5, 3, divideOperation);

            Console.WriteLine($"Addition: {result1}");
            Console.WriteLine($"Subtraction: {result2}");
            Console.WriteLine($"Multiplication: {result3}");
            Console.WriteLine($"Division: {result4}");

            MathOperation anonymousMultiply = delegate (double x, double y) { return x * y; };

            double result5 = PerformOperation(5, 3, anonymousMultiply);
            Console.WriteLine($"Anonymous Multiply: {result5}");

            MathOperation lambdaDivide = (x, y) => y != 0 ? x / y : double.NaN;

            double result6 = PerformOperation(5, 3, lambdaDivide);
            Console.WriteLine($"Lambda Divide: {result6}");

            MathOperation chainedOperation = addOperation + subtractOperation + multiplyOperation;
            double result7 = PerformOperation(5, 3, chainedOperation);
            Console.WriteLine($"Chained Operation: {result7}");
        }

        private static void OnPropertyChanged(object sender, PropertyEventArgs e)
        {
            Console.WriteLine($"Property '{e.PropertyName}' changed");
        }

        private static double PerformOperation(double x, double y, MathOperation operation)
        {
            return operation(x, y);
        }
    }
}
