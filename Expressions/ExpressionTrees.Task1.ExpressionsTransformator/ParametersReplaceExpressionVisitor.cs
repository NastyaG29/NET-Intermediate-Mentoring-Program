using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ParametersReplaceExpressionVisitor : ExpressionVisitor
    {
        private IDictionary<string, object> _paramsDictionary;

        public Expression Convert(LambdaExpression expr, IDictionary<string, object> paramsDictionary)
        {
            _paramsDictionary = paramsDictionary;
            var paramsToReplaceResult = Visit(expr.Body);
            var parameters = expr.Parameters.Where(p => !_paramsDictionary.ContainsKey(p.Name)).ToList();
            return Expression.Lambda(paramsToReplaceResult, parameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _paramsDictionary.TryGetValue(node.Name, out var value) ?
                Expression.Constant(value) : 
                base.VisitParameter(node);
        }
    }
}