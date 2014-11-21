using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using IronPython.Hosting;

namespace IronPythonWithDynamicObject
{
    public class A : IDynamicMetaObjectProvider
    {
        public B GetB()
        {
            return new B();
        }

        private C GetC()
        {
            return new C();
        }

        public DynamicMetaObject GetMetaObject(Expression expression)
        {
            return new ADynamicMetaObject(expression, this);
        }

        private class ADynamicMetaObject : DynamicMetaObject
        {
            internal ADynamicMetaObject(Expression expression, A creator)
                : base(expression, BindingRestrictions.Empty, creator)
            {
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                if (binder.Name == "Test")
                {
                    // Expression to call GetMethod and return a MethodInfo
                    var typeAExpression = Expression.Constant(typeof(A));
                    var getMethodMethodInfo = typeof(Type).GetMethod("GetMethod", new Type[] { typeof(string), typeof(BindingFlags) });
                    var methodNameExpression = Expression.Constant("GetC", typeof(string));
                    var bindingFlagsExpression = Expression.Constant(BindingFlags.Instance | BindingFlags.NonPublic, typeof(BindingFlags));
                    var callTestMethodExpression = Expression.Call(typeAExpression, getMethodMethodInfo, methodNameExpression, bindingFlagsExpression);

                    // Expression to create a delegate and return it
                    var createDelegateMethodInfo = typeof(Delegate).GetMethod("CreateDelegate", new Type[] { typeof(Type), typeof(object), typeof(MethodInfo), });
                    var delegateTypeExpression = Expression.Constant(typeof(Func<C>));
                    var delegateTargetExpression = Expression.Constant(Value);
                    var createDelegateExpression = Expression.Call(createDelegateMethodInfo, delegateTypeExpression, delegateTargetExpression, callTestMethodExpression);

                    return new DynamicMetaObject(createDelegateExpression, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
                }
                return base.BindGetMember(binder);
            }
        }
    }

    public class B
    {
        public string Name { get { return "B"; } }
    }

    public class C
    {
        public string Name { get { return "C"; } }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            scope.SetVariable("input", new A());

            string python = @"
print input.GetB().Name
print input.Test().Name";

            engine.Execute(python, scope);
        }
    }
}