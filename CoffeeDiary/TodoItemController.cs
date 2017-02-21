using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace CoffeeDiary
{

	public class TodoItemController
	{
		TodoItemManager manager;
		public bool error = false;

		public ObservableCollection<TodoItem> display_list;

		public TodoItemController()
		{
			manager = TodoItemManager.DefaultManager;
			display_list = new ObservableCollection<TodoItem>();
		}

		public ObservableCollection<TodoItem> GetSource() //returns display list
		{
			return display_list;
		}

		public async void Refresh(bool syncItems) //clears current display list and refreshes 
		{
			try
			{
				List<TodoItem> temp = await RetrieveList(syncItems);
				display_list.Clear();
				foreach (TodoItem item in temp)
				{
					display_list.Add(item);
				}
			}catch{
				error = true;
			}
		}


		public async Task<List<TodoItem>> RetrieveList (bool syncItems) {
			List<TodoItem> temp = new List<TodoItem>();

			try{
				temp = new List<TodoItem>(await manager.GetTodoItemsAsync(syncItems));
			}catch {
				throw;
			}
			return temp;
		}

		public async Task Delete(TodoItem item) //deletes item from Azure list
		{
			await manager.DeleteTaskAsync(item);
		}

	}
}
