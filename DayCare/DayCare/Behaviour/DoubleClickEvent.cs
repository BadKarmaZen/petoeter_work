using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace DayCare.Behaviour
{
	public static class DoubleClickEvent
	{
		public static readonly DependencyProperty AttachActionProperty =
			DependencyProperty.RegisterAttached(
				"AttachAction",
				typeof(string),
				typeof(DoubleClickEvent),
				new PropertyMetadata(OnAttachActionChanged));

		public static void SetAttachAction(DependencyObject d, string attachText)
		{
			d.SetValue(AttachActionProperty, attachText);
		}

		public static string GetAttachAction(DependencyObject d)
		{
			return d.GetValue(AttachActionProperty) as string;
		}

		private static void OnAttachActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue)
				return;

			var text = e.NewValue as string;
			if (string.IsNullOrEmpty(text))
				return;

			AttachActionToTarget(text, d);
		}

		private static void AttachActionToTarget(string text, DependencyObject d)
		{
			var actionMessage = Parser.CreateMessage(d, text);

			var trigger = new ConditionalEventTrigger
			{
				EventName = "MouseLeftButtonUp",
				Condition = e => DoubleClickCatcher.IsDoubleClick(d, e)
			};
			trigger.Actions.Add(actionMessage);

			Interaction.GetTriggers(d).Add(trigger);
		}

		public class ConditionalEventTrigger : System.Windows.Interactivity.EventTrigger
		{
			public Func<EventArgs, bool> Condition { get; set; }

			protected override void OnEvent(EventArgs eventArgs)
			{
				if (Condition(eventArgs))
					base.OnEvent(eventArgs);
			}
		}

		static class DoubleClickCatcher
		{
			private const int DoubleClickSpeed = 400;
			private const int AllowedPositionDelta = 6;

			static Point clickPosition;
			private static DateTime lastClick = DateTime.Now;
			private static bool firstClickDone;

			internal static bool IsDoubleClick(object sender, EventArgs args)
			{
				var element = sender as UIElement;
				var clickTime = DateTime.Now;

				var e = args as System.Windows.Input.MouseEventArgs;
				if (e == null)
					throw new ArgumentException("MouseEventArgs expected");

				var span = clickTime - lastClick;

				if (span.TotalMilliseconds > DoubleClickSpeed || firstClickDone == false)
				{
					clickPosition = e.GetPosition(element);
					firstClickDone = true;
					lastClick = DateTime.Now;
					return false;
				}

				firstClickDone = false;
				var position = e.GetPosition(element);
				if (Math.Abs(clickPosition.X - position.X) < AllowedPositionDelta &&
						Math.Abs(clickPosition.Y - position.Y) < AllowedPositionDelta)
					return true;
				return false;
			}
		}
	}
}
