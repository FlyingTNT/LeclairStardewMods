using System;

using Microsoft.Xna.Framework;

using StardewValley;

namespace Leclair.Stardew.Almanac {
	public static class WeatherHelper {

		public static string GetSeasonName(int season) {
			return season switch {
				0 => "spring",
				1 => "summer",
				2 => "fall",
				3 => "winter",
				_ => throw new ArgumentException(Convert.ToString(season))
			};
		}

		public static string LocalizeWeather(int weather) {
			return weather switch {
				0 => I18n.Weather_Sunny(),
				1 => I18n.Weather_Rain(),
				2 => I18n.Weather_Debris(),
				3 => I18n.Weather_Lightning(),
				// 4 = Festival, Sunny
				5 => I18n.Weather_Snow(),
				// 6 = Wedding, Sunny?
				_ => I18n.Weather_Sunny()
			};
		}

		public static string GetWeatherName(int weather) {
			return weather switch {
				0 => "sunny",
				1 => "rain",
				2 => "debris",
				3 => "lightning",
				4 => "festival",
				5 => "snow",
				6 => "wedding",
				_ => "unknown"
			};
		}

		public static Rectangle GetWeatherIcon(int weather, string season) {
			int offset = weather switch {
				-1 => 5,
				0 => 0,
				1 => 2,
				2 => 1,
				3 => 3,
				4 => 0,
				5 => 4,
				_ => 0
			};

			return new Rectangle(134, 249 + offset * 18, 18, 18);
		}

		public static int GetWeatherForDate(int seed, WorldDate date) {
			return GetWeatherForDate(seed, date, GameLocation.LocationContext.Default);
		}

		public static int GetWeatherForDate(int seed, WorldDate date, GameLocation.LocationContext context) {
			Random rnd = new Random(seed + date.TotalDays);
			string season = date.Season;
			int totalDays = date.TotalDays;
			int dayOfMonth = date.DayOfMonth;
			int result = 0;

			if (context == GameLocation.LocationContext.Default) {
				if (Utility.isFestivalDay(dayOfMonth, season))
					result = 4;
				else if (totalDays == 3U)
					result = 1;
				else {
					double rainChance = !season.Equals("summer") ? (!season.Equals("winter") ? 0.183 : 0.63) : (dayOfMonth > 1 ? 0.12 + dayOfMonth * (3.0 / 1000.0) : 0.0);
					if (rnd.NextDouble() < rainChance) {
						if (season.Equals("winter"))
							result = 5;
						else if (season.Equals("summer") && rnd.NextDouble() < 0.85 || !season.Equals("winter") && rnd.NextDouble() < 0.25 && dayOfMonth > 2 && totalDays > 28U)
							result = 3;
						else
							result = 1;
					} else {
						result = totalDays <= 2U || (!season.Equals("spring") || rnd.NextDouble() >= 0.2) && (!season.Equals("fall") || rnd.NextDouble() >= 0.6) ? 0 : 2;
					}
				}

			} else if (context == GameLocation.LocationContext.Island) {
				if (rnd.NextDouble() < 0.24)
					result = 1;
			}

			return Game1.getWeatherModificationsForDate(date, result);
		}

		public static int GetWeatherForDate(int seed, int day) {
			return GetWeatherForDate(seed, day, GameLocation.LocationContext.Default);
		}

		public static int GetWeatherForDate(int seed, int day, GameLocation.LocationContext context) {
			WorldDate date = new();
			date.TotalDays = day;
			return GetWeatherForDate(seed, date, context);
		}

	}
}
