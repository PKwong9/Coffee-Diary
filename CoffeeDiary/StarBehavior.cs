using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CoffeeDiary
{
	public class StarBehavior : Behavior<View>
	{
		TapGestureRecognizer tapRecognizer;
		static List<StarBehavior> defaultBehaviors = new List<StarBehavior>();
		static Dictionary<string, List<StarBehavior>> starGroups = new Dictionary<string, List<StarBehavior>>();

		public static readonly BindableProperty GroupNameProperty =
			BindableProperty.Create("GroupName",
									typeof(string),
									typeof(StarBehavior),
									null,
									propertyChanged: OnGroupNameChanged);

		public string GroupName
		{
			set { SetValue(GroupNameProperty, value); }
			get { return (string)GetValue(GroupNameProperty); }
		}


		static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			StarBehavior behavior = (StarBehavior)bindable;
			string oldGroupName = (string)oldValue;
			string newGroupName = (string)newValue;

			if (String.IsNullOrEmpty(oldGroupName)) // remove existing behavior from Group
			{
				defaultBehaviors.Remove(behavior);
			}
			else
			{
				List<StarBehavior> behaviors = starGroups[oldGroupName];
				behaviors.Remove(behavior);

				if (behaviors.Count == 0)
				{
					starGroups.Remove(oldGroupName);
				}
			}

	
			if (String.IsNullOrEmpty(newGroupName)) // add New Behavior to the group
			{
				defaultBehaviors.Add(behavior);
			}
			else
			{
				List<StarBehavior> behaviors = null;

				if (starGroups.ContainsKey(newGroupName))
				{
					behaviors = starGroups[newGroupName];
				}
				else
				{
					behaviors = new List<StarBehavior>();
					starGroups.Add(newGroupName, behaviors);
				}

				behaviors.Add(behavior);
			}

		}


		public static readonly BindableProperty IsStarredProperty = //binds star behavior to propertychanged
			BindableProperty.Create("IsStarred",
									typeof(bool),
									typeof(StarBehavior),
									false,
									propertyChanged: OnIsStarredChanged);

		public bool IsStarred
		{
			set { SetValue(IsStarredProperty, value); }
			get { return (bool)GetValue(IsStarredProperty); }
		}

		static void OnIsStarredChanged(BindableObject bindable, object oldValue, object newValue) //sets star rating 
		{
			StarBehavior behavior = (StarBehavior)bindable;

			if ((bool)newValue)
			{
				string groupName = behavior.GroupName;
				List<StarBehavior> behaviors = null;

				if (string.IsNullOrEmpty(groupName))
				{
					behaviors = defaultBehaviors;
				}
				else
				{
					behaviors = starGroups[groupName];
				}

				bool itemReached = false;
				int count = 1;
				// all positions to left IsStarred = true and all position to the right is false
				foreach (var item in behaviors)
				{
					if (item != behavior && !itemReached)
					{
						item.IsStarred = true;
					}
					if (item == behavior)
					{
						itemReached = true;
						item.IsStarred = true;
					}
					if (item != behavior && itemReached)
						item.IsStarred = false;
					count++;
				}

			}


		}

		public StarBehavior()
		{
			defaultBehaviors.Add(this);
		}

		protected override void OnAttachedTo(View view) //creates new tap recognizer for star rating
		{
			tapRecognizer = new TapGestureRecognizer();
			tapRecognizer.Tapped += OnTapRecognizerTapped;
			view.GestureRecognizers.Add(tapRecognizer);
		}

		protected override void OnDetachingFrom(View view) //removes tap recognizer 
		{
			view.GestureRecognizers.Remove(tapRecognizer);
			tapRecognizer.Tapped -= OnTapRecognizerTapped;
		}

		void OnTapRecognizerTapped(object sender, EventArgs args)
		{
			IsStarred = false;
			IsStarred = true;
		}
	}
}
