using SampleAPI.Domain;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SampleAPI.Models
{
	public class Tray
	{
		[JsonIgnore]
		public int Columns => (TraySizes)TraySize switch
		{
			TraySizes.Small => 12,
			TraySizes.Large => 24,
			_ => throw new ArgumentOutOfRangeException(nameof(TraySize), "Invalid TraySize"),
		};

		[JsonIgnore]
		public int Rows => (TraySizes)TraySize switch
		{
			TraySizes.Small => 8,
			TraySizes.Large => 16,
			_ => throw new ArgumentOutOfRangeException(nameof(TraySize), "Invalid TraySize"),
		};

		public int TraySize { get; set; }

		public IList<Well> Wells { get; set; } = new List<Well>();
	}
}