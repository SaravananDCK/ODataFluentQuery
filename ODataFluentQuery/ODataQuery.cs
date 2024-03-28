using System.Linq.Expressions;
using System.Text;

namespace ODataFluentQuery
{
    public class ODataFilter<T>
    {
        private readonly List<string> _filters = new List<string>();
        private string _logicalOperator = string.Empty;
        private bool _useGrouping = false;

        public ODataFilter<T> BeginGroup()
        {
            _filters.Add("("); // Start a new group
            _useGrouping = true;
            return this;
        }

        public ODataFilter<T> EndGroup()
        {
            _filters.Add(")"); // End the current group
            _useGrouping = false;
            return this;
        }

        public ODataFilter<T> FilterBy(Expression<Func<T, bool>> predicate)
        {
            var filterString = ConvertExpressionToODataFilter(predicate);
            if (_useGrouping && _filters.Last() == "(")
            {
                _filters.Add(filterString); // Add filter string inside the group
            }
            else
            {
                _filters.Add($"{_logicalOperator}{filterString}");
            }
            _logicalOperator = string.Empty; // Reset after use
            return this;
        }

        public ODataFilter<T> And()
        {
            _logicalOperator = " and ";
            return this;
        }

        public ODataFilter<T> Or()
        {
            _logicalOperator = " or ";
            return this;
        }

        public string Build()
        {
            return string.Join(string.Empty, _filters);
        }
        public ODataFilter<T> Contains(Expression<Func<T, string>> predicate, string substring)
        {
            // Convert the expression into an OData 'contains' function
            var filterString = ConvertExpressionToODataFunction(predicate, "contains", substring);
            _filters.Add(filterString);
            return this;
        }

        public ODataFilter<T> StartsWith(Expression<Func<T, string>> predicate, string startString)
        {
            // Convert the expression into an OData 'startsWith' function
            var filterString = ConvertExpressionToODataFunction(predicate, "startsWith", startString);
            _filters.Add(filterString);
            return this;
        }

        private string ConvertExpressionToODataFunction(Expression<Func<T, string>> predicate, string functionName, string value)
        {
            if (predicate.Body is MemberExpression memberExpression)
            {
                // Construct the OData function string
                return $"{functionName}({memberExpression.Member.Name}, '{value}')";
            }

            throw new NotSupportedException("This expression type is not supported yet.");
        }
        private string ConvertExpressionToODataFilter(Expression<Func<T, bool>> predicate)
        {
            // This will be a simple implementation that only handles 'Equal' binary expressions
            if (predicate.Body is BinaryExpression binaryExpression)
            {
                if (binaryExpression.NodeType == ExpressionType.Equal)
                {
                    // Assuming the left side of the equation is a member expression (e.g., p.Name)
                    if (binaryExpression.Left is MemberExpression memberExpression)
                    {
                        string propertyName = GetFullPropertyPath(memberExpression);
                        // Assuming the right side of the equation is a constant expression (e.g., "Table")
                        if (binaryExpression.Right is ConstantExpression constantExpression)
                        {
                            // Construct the OData filter string
                            return $"{propertyName} eq '{constantExpression.Value}'";
                        }
                    }
                }
                else if (binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual)
                {
                    // Assuming the left side of the equation is a member expression (e.g., p.Name)
                    if (binaryExpression.Left is MemberExpression memberExpression)
                    {
                        string propertyName = GetFullPropertyPath(memberExpression);
                        // Assuming the right side of the equation is a constant expression (e.g., "Table")
                        if (binaryExpression.Right is ConstantExpression constantExpression)
                        {
                            // Construct the OData filter string
                            return $"{propertyName} ge '{constantExpression.Value}'";
                        }
                    }
                }
                else if (binaryExpression.NodeType == ExpressionType.GreaterThan)
                {
                    // Assuming the left side of the equation is a member expression (e.g., p.Name)
                    if (binaryExpression.Left is MemberExpression memberExpression)
                    {
                        string propertyName = GetFullPropertyPath(memberExpression);
                        // Assuming the right side of the equation is a constant expression (e.g., "Table")
                        if (binaryExpression.Right is ConstantExpression constantExpression)
                        {
                            // Construct the OData filter string
                            return $"{propertyName} gt '{constantExpression.Value}'";
                        }
                    }
                }
                else if (binaryExpression.NodeType == ExpressionType.LessThanOrEqual)
                {
                    // Assuming the left side of the equation is a member expression (e.g., p.Name)
                    if (binaryExpression.Left is MemberExpression memberExpression)
                    {
                        string propertyName = GetFullPropertyPath(memberExpression);
                        // Assuming the right side of the equation is a constant expression (e.g., "Table")
                        if (binaryExpression.Right is ConstantExpression constantExpression)
                        {
                            // Construct the OData filter string
                            return $"{propertyName} le '{constantExpression.Value}'";
                        }
                    }
                }
                else if (binaryExpression.NodeType == ExpressionType.LessThan)
                {
                    // Assuming the left side of the equation is a member expression (e.g., p.Name)
                    if (binaryExpression.Left is MemberExpression memberExpression)
                    {
                        string propertyName = GetFullPropertyPath(memberExpression);
                        // Assuming the right side of the equation is a constant expression (e.g., "Table")
                        if (binaryExpression.Right is ConstantExpression constantExpression)
                        {
                            // Construct the OData filter string
                            return $"{propertyName} lt '{constantExpression.Value}'";
                        }
                    }
                }
            }
            else if (predicate.Body is MemberExpression memberExpression)
            {
                // Check if the member is a property of a navigation property
                if (memberExpression.Expression is MemberExpression parentMemberExpression)
                {
                    // Construct the OData filter string with navigation property
                    return $"{parentMemberExpression.Member.Name}/{memberExpression.Member.Name} eq '{GetConstantValue(memberExpression)}'";
                }
                else
                {
                    // Handle the simple case where there is no navigation property
                    return $"{memberExpression.Member.Name} eq '{GetConstantValue(memberExpression)}'";
                }
            }

            throw new NotSupportedException("This expression type is not supported yet.");
        }
        private string GetFullPropertyPath(MemberExpression expression)
        {
            // Recursively get the full property path for nested properties
            var propertyPath = new List<string>();
            while (expression != null)
            {
                propertyPath.Insert(0, expression.Member.Name);
                expression = expression.Expression as MemberExpression;
            }
            return string.Join("/", propertyPath);
        }
        private object GetConstantValue(MemberExpression memberExpression)
        {
            // This method should extract the value from the right side of the expression
            // You will need to implement this based on your specific requirements
            throw new NotImplementedException();
        }
    }
}
