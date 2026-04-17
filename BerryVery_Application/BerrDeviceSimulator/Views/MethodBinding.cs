using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace BerrDeviceSimulator.Views
{
    public class MethodBinding : MarkupExtension
    {
        public string MethodName { get; set; }

        public MethodBinding(string methodName)
        {
            MethodName = methodName;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (valueTarget == null)
                return null;

            var frameworkElement = valueTarget.TargetObject as FrameworkElement;
            if (frameworkElement == null)
                return null;

            RoutedEventHandler handler = (sender, args) =>
            {
                var dataContext = frameworkElement.DataContext;

                // 단일 파라미터 적용
                // InvokeWithSingleParameter(dataContext, MethodName, args); // 단일 파라미터 적용

                // 다중 파라미터 적용
                InvokeWithMatchingParameters(dataContext, MethodName, sender, args);
            };

            return handler;
                    }

        private void InvokeWithSingleParameter(object target, string methodName, object parameter)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
                return;

            var method = target.GetType().GetMethod(
                methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (method == null)
            {
                System.Diagnostics.Debug.WriteLine(
                    "[MethodBinding] Method '" + methodName + "' not found on " + target.GetType().Name + "."
                );
                return;
            }

            var parameters = method.GetParameters();

            try
            {
                if (parameters.Length == 1 && typeof(RoutedEventArgs).IsAssignableFrom(parameters[0].ParameterType))
                {
                    method.Invoke(target, new object[] { parameter });
                }
                else
                {
                    method.Invoke(target, null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "[MethodBinding] Error invoking method '" + MethodName + "': " + ex.Message
                );
            }
        }

        /// <summary>
        /// 다중 파라미터 적용
        /// </summary>
        /// <param name="target">dataContext</param>
        /// <param name="methodName">methodName</param>
        /// <param name="availableArgs">args</param>
        private void InvokeWithMatchingParameters(object target, string methodName, params object[] availableArgs)
        {
            if (target == null || string.IsNullOrEmpty(methodName)) return;

            var method = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(m =>
                {
                    if (m.Name != methodName) return false;
                    var parameters = m.GetParameters();
                    return parameters.All(p =>
                        availableArgs.Any(arg => arg != null && p.ParameterType.IsInstanceOfType(arg)));
                });

            if (method == null)
            {
                Debug.WriteLine($"[MethodBinding] Method '{methodName}' not found.");
                return;
            }

            var argsToPass = method.GetParameters()
                .Select(p => availableArgs.FirstOrDefault(arg => p.ParameterType.IsInstanceOfType(arg)))
                .ToArray();

            method.Invoke(target, argsToPass);
        }
    }
}
