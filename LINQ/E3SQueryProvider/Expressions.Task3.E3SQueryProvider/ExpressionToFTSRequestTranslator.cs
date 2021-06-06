using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                switch (node.Method.Name)
                {
                    case "Equals":
                        Visit(node.Object);
                        var arg = node.Arguments[0];
                        if (arg.NodeType == ExpressionType.Constant)
                        {
                            _resultStringBuilder.Append("(");
                            Visit(arg);
                            _resultStringBuilder.Append(")");
                        }
                        else
                            Visit(arg);
                        return node;

                    case "Contains":
                        Visit(node.Object);
                        arg = node.Arguments[0];
                        if (arg.NodeType == ExpressionType.Constant)
                        {
                            _resultStringBuilder.Append("(*");
                            Visit(arg);
                            _resultStringBuilder.Append("*)");
                        }
                        else
                            Visit(arg);
                        return node;

                    case "StartsWith":
                        Visit(node.Object);
                        arg = node.Arguments[0];
                        if (arg.NodeType == ExpressionType.Constant)
                        {
                            _resultStringBuilder.Append("(");
                            Visit(arg);
                            _resultStringBuilder.Append("*)");
                        }
                        else
                            Visit(arg);
                        return node;

                    case "EndsWith":
                        Visit(node.Object);
                        arg = node.Arguments[0];
                        if (arg.NodeType == ExpressionType.Constant)
                        {
                            _resultStringBuilder.Append("(*");
                            Visit(arg);
                            _resultStringBuilder.Append(")");
                        }
                        else
                            Visit(arg);
                        return node;
                }
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    var memberAccess = node.Left.NodeType == ExpressionType.MemberAccess ? node.Left : node.Right.NodeType == ExpressionType.MemberAccess ? node.Right : null;
                    var constant = node.Left.NodeType == ExpressionType.Constant ? node.Left : node.Right.NodeType == ExpressionType.Constant ? node.Right : null;

                    if (memberAccess is null || constant is null)
                        throw new NotSupportedException($"Expression '{node}' is not supported");

                    Visit(memberAccess);
                    _resultStringBuilder.Append("(");
                    Visit(constant);
                    _resultStringBuilder.Append(")");
                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(" AND ");
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}