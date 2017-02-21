using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace CoffeeDiary
{
	public class TodoItem{
		string id;
		string name;
		string coffee;
		string review;
		string address;
		string file_path;
		DateTime createdAt;

		public TodoItem( string name, string coffee, string review, string address, string file_path) {
			this.name = name;
			this.coffee = coffee;
			this.review = review;
			this.address = address;
			this.file_path = file_path;
		}

		public string ItemtoString()
		{
			return this.name;
		}

		public string GetItemId() {
			return this.id;
		}

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value; }
		}


		[JsonProperty(PropertyName = "name")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		[JsonProperty(PropertyName = "coffee")]
		public string Coffee
		{
			get { return coffee; }
			set { coffee = value; }
		}

		[JsonProperty(PropertyName = "review")]
		public string Review
		{
			get { return review; }
			set { review = value; }
		}

		[JsonProperty(PropertyName = "address")]
		public string Address
		{
			get { return address; }
			set { address = value; }
		}

		[JsonProperty(PropertyName = "path")]
		public string Path
		{
			get { return file_path; }
			set { file_path = value; }
		}

		[JsonProperty(PropertyName = "createdAt")]
		public DateTime CreatedAt
		{
			get { return createdAt; }
			set { createdAt = value; }
		}

		[Version]
		public string Version { get; set; }
	}
}

