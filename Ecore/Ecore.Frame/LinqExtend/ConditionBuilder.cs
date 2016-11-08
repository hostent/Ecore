using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Ecore.Frame.LinqExtend
{
    /// <summary>
    /// 条件生成器
    /// </summary>
    public class ConditionBuilder : ExpressionVisitor
    {
        private List<object> m_arguments;
        private Stack<string> m_conditionParts;

        public string Condition { get; set; }

        public object[] Arguments { get; set; }

        public void Build(Expression expression)
        {
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(expression);

            this.m_arguments = new List<object>();
            this.m_conditionParts = new Stack<string>();

            this.Visit(evaluatedExpression);


            this.Arguments = this.m_arguments.ToArray();
            this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : null;



        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b == null) return b;

            bool isDeal = false;

            string opr;
            switch (b.NodeType)
            {
                case ExpressionType.Equal:
                    if (b.Right.ToString().ToLower() == "null")
                    {
                        opr = " is null ";
                        isDeal = true;
                    }
                    else
                    {
                        opr = "=";
                    }
                    break;
                case ExpressionType.NotEqual:
                    if (b.Right.ToString().ToLower() == "null")
                    {
                        opr = " is not null ";
                        isDeal = true;
                    }
                    else
                    {
                        opr = "<>";
                    }
                    break;
                case ExpressionType.GreaterThan:
                    opr = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    opr = ">=";
                    break;
                case ExpressionType.LessThan:
                    opr = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    opr = "<=";
                    break;
                case ExpressionType.AndAlso:
                    opr = "AND";
                    break;
                case ExpressionType.OrElse:
                    opr = "OR";
                    break;
                case ExpressionType.Add:
                    opr = "+";
                    break;
                case ExpressionType.Subtract:
                    opr = "-";
                    break;
                case ExpressionType.Multiply:
                    opr = "*";
                    break;
                case ExpressionType.Divide:
                    opr = "/";
                    break;
                default:
                    throw new NotSupportedException(b.NodeType + "is not supported.");
            }
            //这种条件下生成 in 语法
            if (b.Left.ToString().Contains(".ToString()") && b.Right.ToString().StartsWith("\"(") && b.Right.ToString().EndsWith(")\""))
            {
                opr = "in";
            }

            this.Visit(b.Left);
            this.Visit(b.Right);

            string right = this.m_conditionParts.Pop();
            string left = this.m_conditionParts.Pop();

            if (isDeal)
            {
                right = "";
                this.m_arguments.RemoveAt(this.m_arguments.Count - 1);
            }

            string condition = String.Format("({0} {1} {2})", left, opr, right);
            this.m_conditionParts.Push(condition);

            return b;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.Name == "Contains")
            {
                this.Visit(m.Arguments[0]);
                this.Visit(m.Object);

                m_conditionParts.Pop();//清掉不需要的数据
                string col = m_conditionParts.Pop();

                string inSql = col + " in (";

                IEnumerable par = m_arguments.Last() as IEnumerable;
                m_arguments.Remove(m_arguments.Last());

                if (par == null)
                {
                    m_conditionParts.Push(" 1<>1 ");
                }
                else
                {
                    foreach (var item in par)
                    {
                        m_arguments.Add(item);
                        inSql = inSql + String.Format("@q__{0},", this.m_arguments.Count - 1);
                    }
                    inSql = inSql.Trim(',');
                    inSql = inSql + ") ";

                    m_conditionParts.Push(inSql);
                }

                return null;
            }
            else
            {
                return base.VisitMethodCall(m);
            }
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c == null) return c;

            this.m_arguments.Add(c.Value);
            this.m_conditionParts.Push(String.Format("@q__{0}", this.m_arguments.Count - 1));

            return c;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m == null) return m;

            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            if (propertyInfo == null) return m;

            this.m_conditionParts.Push(String.Format("{0}", propertyInfo.Name));

            //switch (DbManager.GetSqlType())
            //{
            //    case SqlType.MySql:
            //        this.m_conditionParts.Push(String.Format("`{0}`", propertyInfo.Name));
            //        break;
            //    default:
            //        this.m_conditionParts.Push(String.Format("[{0}]", propertyInfo.Name));
            //        break;
            //}


            return m;
        }
    }
}
