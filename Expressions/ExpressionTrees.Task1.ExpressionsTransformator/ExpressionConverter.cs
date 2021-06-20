using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ExpressionConverter
    {
        private readonly IncDecExpressionVisitor _incDecExpressionVisitor = new IncDecExpressionVisitor();

        private readonly ParametersReplaceExpressionVisitor _parametersReplaceExpressionVisitor = new ParametersReplaceExpressionVisitor();

        public Expression Convert(Expression expr, Dictionary<string, object> paramsToReplace)
        {
            var inDecResult = _incDecExpressionVisitor.VisitAndConvert(expr, "");
            return _parametersReplaceExpressionVisitor.Convert(inDecResult as LambdaExpression, paramsToReplace);
        }
    }
}